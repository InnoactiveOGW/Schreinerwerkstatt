/*
 * inspired by
 * http://unitycoder.com/blog/2015/03/24/mesh-melt-shader-test/
 * 
 * This script and the content of the package are provided as-is.
 * You are free to modify and redistribute the package and code as you need.
 * 
 * As I put some effort into it I would he happy to see you mention ths use of the package in your project
 * Tell me about it at mailme[at]tobiaspott.de
 * 
 */
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Deformable : MonoBehaviour
{
    /// <summary>
    /// protected field to store the deformer associated for this deformable.
    /// </summary>
    [SerializeField()]
    protected List<Deformer> _deformers = new List<Deformer>();
    /// <summary>
    /// protected field to determine whether the deformable should restore it's state.
    /// </summary>
    [SerializeField()]
    protected bool _restore = true;
    /// <summary>
    /// protected field to determine whether collider data should be updated
    /// </summary>
    [SerializeField()]
    protected bool _updateCollider = true;

    /// <summary>
    /// protected field to determine whether the deformable should use dynamic normals instead of mesh's normals.
    /// </summary>
    [SerializeField()]
    protected bool _useDynamicNormal = false;
    /// <summary>
    /// protected field to store the dynamic normal. Set to Vector.zero to force deformer-dependend dynamic normals.
    /// </summary>
    [SerializeField()]
    protected Vector3 _dynamicNormal = Vector3.zero;
    /// <summary>
    /// protected field to store the orientation of deformation when fixed mesh's normals are used.
    /// </summary>
    [SerializeField()]
    protected float _fixedNormalDir = -1.0f;

    /// <summary>
    /// protected field to store the deform magnifier value applied to a displaced vertex
    /// </summary>
    [SerializeField()]
    protected float _deformMagnifier = 1.0f;

    /// <summary>
    /// protected field to store the effective range of a deformer on the deformable
    /// </summary>
    [SerializeField()]
    protected float _effectRange = 1.0f;
    /// <summary>
    /// protected field to store the minimum distance at which a deformer affects the deformable
    /// </summary>
    [SerializeField()]
    protected float _deformerMinDistance = 4.0f;
    /// <summary>
    /// protected field to the speed at which the deformation is processed (the higher the faster).
    /// </summary>
    [SerializeField()]
    protected float fadeSpeed = 2.0f;

    float viewDistance = 1.0f;

    // components
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    // mesh duplication
    private Mesh _mesh0;
    private Vector3[] _bVertices, _bNormals;
    private Vector2[] _bUVs;
    private Color[] _bColors;
    private int[] _bTriangles;
    // mesh deformation
    private bool modified = false;
    private float orient = 0.0f;
    private float minDist = 0.0f;
    private float vtxDist = 0.0f;
    private float remapDist = 0.0f;
    private int i = 0;
    private int j = 0;
    private int closestDeformer = -1;
    private List<Vector3> _localDeformerPosition = new List<Vector3>();
    private Vector3 dynNormal = Vector3.zero;


    void Start()
    {
        // retrieve the gameObject's mesh filter and renderer
        _meshFilter = this.GetComponent<MeshFilter>();
        _meshCollider = this.GetComponent<MeshCollider>();
        // get the current mesh into a temporary object 
        Mesh baseMesh = _meshFilter.sharedMesh;

        // when no colors
        if (baseMesh.colors == null || baseMesh.colors.Length == 0)
        {
            Debug.LogWarning("Missing vertex colors, original mesh will be duplicated and reassigned to the mesh filter.");
            _mesh0 = new Mesh();

            // copy vertex positions
            _bVertices = new Vector3[baseMesh.vertexCount];
            Array.Copy(baseMesh.vertices, _bVertices, baseMesh.vertexCount);
            // copy mesh normals if any exist
            if (baseMesh.normals != null && baseMesh.normals.Length != 0)
            {
                _bNormals = new Vector3[baseMesh.vertexCount];
                Array.Copy(baseMesh.normals, _bNormals, baseMesh.vertexCount);
            }
            // copy mesh uvs if any exist
            if (baseMesh.uv != null && baseMesh.uv.Length != 0)
            {
                _bUVs = new Vector2[baseMesh.vertexCount];
                Array.Copy(baseMesh.uv, _bUVs, baseMesh.vertexCount);
            }
            // init color and flush with black
            _bColors = new Color[baseMesh.vertexCount];
            for (int i = 0; i < baseMesh.vertexCount; i++)
                _bColors[i] = Color.black;
            // copy mesh triangle indices
            _bTriangles = new int[baseMesh.triangles.Length];
            Array.Copy(baseMesh.triangles, _bTriangles, baseMesh.triangles.Length);

            // assign copied mesh data array to working mesh
            _mesh0.vertices = _bVertices;
            _mesh0.normals = _bNormals;
            _mesh0.uv = _bUVs;
            _mesh0.colors = _bColors;
            _mesh0.triangles = _bTriangles;

            // assign working mesh to the gameObject's mesh filter
            _meshFilter.mesh = null;
            _meshFilter.sharedMesh = _mesh0;
        }
        else
        {
            Color[] colors = baseMesh.colors;
            for (int i = 0; i < baseMesh.vertexCount; i++)
            {
                colors[i] = Color.black;
            }
            baseMesh.colors = colors;
            _meshFilter.sharedMesh = baseMesh;
        }

        if (baseMesh.normals == null || baseMesh.normals.Length == 0)
        {
            // otherwise activate dynamic normal usage (which does not require mesh normals)
            _useDynamicNormal = true;
            Debug.LogWarning("Missing vertex normals, dynamic normal will be used. "
            + "Either assign a value to the dynamic normal or runtime normals will apply.");
        }



        // assigned mesh to the collider if one exists
        if (_updateCollider && _meshCollider != null)
        {
            _meshCollider.sharedMesh = null;
            _meshCollider.sharedMesh = _mesh0;
        }
    }

    void Update()
    {
        // get the shared mesh from the gameObjects mesh filter
        _mesh0 = _meshFilter.sharedMesh;
        // get references to the mesh's vertices and vertex colors
        Vector3[] vertices = _mesh0.vertices;
        Color[] colors = _mesh0.colors;

        // process either using dynamic normals or mesh's normals
        if (_useDynamicNormal)
        {
            if (!this.DeformByDynamicNormal(vertices, colors))
                return;
        }
        else
        {
            // get reference to the mesh's normals
            Vector3[] normals = _mesh0.normals;
            if (!this.DisplaceByMeshNormal(vertices, normals, colors))
                return;
        }

        // push back verticies and colors to the working mesh
        _mesh0.vertices = vertices;
        _mesh0.colors = colors;

        // assigned & update mesh to the collider if one exists
        if (_updateCollider && _meshCollider != null) // && (Time.frameCount % 10 == 0))
        {
            _meshCollider.sharedMesh = null;
            _meshCollider.sharedMesh = _meshFilter.sharedMesh;
        }
    }

    /// <summary>
    /// transform the heat sources world position to deformable local space positions
    /// </summary>
    private void LocalizeHeatSources()
    {
        // if number if previous localized deformers differs from current number of heat sources
        if (_localDeformerPosition.Count != _deformers.Count)
        {
            // clear the previous localized list 
            _localDeformerPosition.Clear();
            // iterate over all deformers and localize their positions
            for (j = 0; j < _deformers.Count; j++)
                _localDeformerPosition.Add(this.transform.InverseTransformPoint(_deformers[j].transform.position));
        }
        // if the number of deformer didn't changed just update the values
        else
        {
            // iterate over all deformers and localize their positions
            for (j = 0; j < _deformers.Count; j++)
                _localDeformerPosition[j] = this.transform.InverseTransformPoint(_deformers[j].transform.position);
        }
    }

    /// <summary>
    /// displace the passed vertices and colors using the given normals
    /// </summary>
    /// <param name="verts">array of vertices to deform</param>
    /// <param name="normals">array of normals used for deformation</param>
    /// <param name="clrs">array of vertex colors to map the deformation back </param>
    /// <returns>true if any deformation was applied, false otherwise</returns>
    private bool DisplaceByMeshNormal(Vector3[] verts, Vector3[] normals, Color[] clrs)
    {
        // init values for processing
        modified = false;
        orient = Mathf.Clamp(_fixedNormalDir, -1.0f, 1.0f);
        minDist = 0.0f;
        vtxDist = 0.0f;
        remapDist = 0.0f;
        j = 0;
        // localize the deformers' positions
        this.LocalizeHeatSources();

        for (i = 0; i < _mesh0.vertexCount; i++)
        {
            // init minDist with maximum value
            minDist = float.MaxValue;
            for (j = 0; j < _deformers.Count; j++)
            {
                // if deformer was not transformed since last frame
                // and deformer component is disabled contine with next
                if (!_deformers[j].Transformed || !_deformers[j].enabled)
                    continue;
                // calculate the distance between current vertex and the current localized deformer position
                // (subtracting the deformer's spacing)
                vtxDist = Vector3.Distance(verts[i], _localDeformerPosition[j]) - _deformers[j].Spacing;
                // if distance to vertex is above minimum distance for deformers to apply to the deformable
                if (vtxDist > _deformerMinDistance)
                    continue;
                // if the distance to the deformer is lower than the minDist set it as new minDist
                if (vtxDist < minDist)
                    minDist = vtxDist;
            }

            // if minDist is unchanged
            if (minDist == float.MaxValue)
                continue;

            // if not modified before mark it now!
            if (!modified) modified = true;

            // remap the actual distance to the range 0..1 using a range of 0.._effectRange
            remapDist = this.Remap(minDist, 0.0f, _effectRange, 0.0f, 1.0f);

            // check whether the mesh shout restore itseld
            if (_restore)
                clrs[i].a = Mathf.Clamp01(clrs[i].a - (viewDistance - remapDist) * Time.deltaTime * fadeSpeed);
            else
                // using Mathf.Min keeps the color at it's lowest ever set value which results in non-restoring mesh
                clrs[i].a = Mathf.Min(clrs[i].a,
                Mathf.Clamp01(clrs[i].a - (viewDistance - remapDist) * Time.deltaTime * fadeSpeed));

            // displace the vertex by the mesh's normal
            verts[i] = _bVertices[i] + (normals[i] * (1.0f - clrs[i].a) * orient * _deformMagnifier);
        }

        // return false if the mesh wasn't deformed
        if (!modified)
            return false;
        return true;
    }

    /// <summary>
    /// deforms the passed vertices and colors using dynamic normals
    /// </summary>
    /// <param name="verts">array of vertices to deform</param>
    /// <param name="clrs">array of vertex colors to map the deformation back </param>
    /// <returns>true if any deformation was applied, false otherwise</returns>
    private bool DeformByDynamicNormal(Vector3[] verts, Color[] clrs)
    {
        // init values for processing
        modified = false;
        orient = Mathf.Clamp(_fixedNormalDir, -1.0f, 1.0f);
        minDist = 0.0f;
        vtxDist = 0.0f;
        remapDist = 0.0f;
        closestDeformer = -1;

        // set later used dynamic normal to _dynamicNormal
        dynNormal = this.transform.InverseTransformDirection(_dynamicNormal);
        // localize the deformers' positions
        this.LocalizeHeatSources();

        // iterate over each vertex of the mesh
        for (i = 0; i < _mesh0.vertexCount; i++)
        {
            // init minDist with maximum value
            minDist = float.MaxValue;
            // iterate over all deformers
            for (j = 0; j < _deformers.Count; j++)
            {
                // if deformer was not transformed since last frame
                // and deformer component is disabled contine with next
                if (!_deformers[j].Transformed || !_deformers[j].enabled)
                    continue;
                // calculate the distance between current vertex and the current localized deformer position 
                // (subtracting the deformer's spacing)
                vtxDist = Vector3.Distance(verts[i], _localDeformerPosition[j]) - _deformers[j].Spacing;
                // if distance to vertex is above minimum distance for deformers to apply to the deformable
                if (vtxDist > _deformerMinDistance)
                    continue;
                // if the distance to the deformer is lower than the minDist
                // set it as new minDist and save the index of the deformer
                if (vtxDist < minDist)
                {
                    minDist = vtxDist;
                    closestDeformer = j;
                }
            }

            // if minDist is unchanged continue with next iteration
            if (minDist == float.MaxValue) continue;
            // if no closes source was detected continue with next iteration
            if (closestDeformer == -1) continue;

            // if not modified before mark it now!
            if (!modified) modified = true;

            // update dynamic normal when no custom dynamic normal is set
            if (_dynamicNormal == Vector3.zero)
                // calculate dynamic normal by the direction from the current vertex to the closest deformer
                dynNormal = (_bVertices[i] - _localDeformerPosition[closestDeformer]).normalized;// this.transform.InverseTransformDirection((verts[i] - _localizedHeatPosition[closestSource]).normalized);

            // remap the actual distance to the range 0..1 using a range of 0.._effectRange
            remapDist = this.Remap(minDist, 0.0f, _effectRange, 0.0f, 1.0f);

            // check whether the mesh shout restore itseld
            if (_restore)
                clrs[i].a = Mathf.Clamp01(clrs[i].a - (viewDistance - remapDist) * Time.deltaTime * fadeSpeed);
            else
                // using Mathf.Min keeps the color at it's lowest ever set value which results in non-restoring mesh
                clrs[i].a = Mathf.Min(clrs[i].a,
                Mathf.Clamp01(clrs[i].a - (viewDistance - remapDist) * Time.deltaTime * fadeSpeed));

            // displace the vertex by the dynamic normal
            verts[i] = _bVertices[i] + (dynNormal * (1.0f - clrs[i].a) * _deformMagnifier);

        }

        // return false if the mesh wasn't deformed
        if (!modified)
            return false;
        return true;
    }

    // helper function
    float Remap(float val, float inLowerEnd, float inUpperEnd, float outLowerEnd, float outUpperEnd)
    {
        return outLowerEnd + (val - inLowerEnd) * (outUpperEnd - outLowerEnd) / (inUpperEnd - inLowerEnd);
    }

}