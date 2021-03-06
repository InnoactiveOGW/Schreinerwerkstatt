﻿using UnityEngine;
using System.Collections;
using System;

public class PencilDraw : Pickup
{
    bool painting = false;
    Texture2D tex;
    Renderer paintRender;
	GameObject paintedWood;

    // Update is called once per frame
    void Update()
    {
        if (painting && paintRender != null)
        {
            RaycastHit hit;
            if (!Physics.Raycast(this.gameObject.transform.position,
                -this.gameObject.transform.forward,
                out hit,
                float.PositiveInfinity,
                LayerMask.GetMask("WoodLayer"),
                QueryTriggerInteraction.Ignore))
                return;

			Debug.Log ("Hit: " + hit.point.ToString ());
            Debug.Log("Hit object: " + hit.collider.gameObject.tag);

            Renderer rend = hit.transform.GetComponent<Renderer>();
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                return;
            if (tex == null)
            {
                tex = new Texture2D(512, 1024, TextureFormat.RGB24, false);
            }
            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= tex.width;
            pixelUV.y *= tex.height;

			Debug.Log ("x: " +  pixelUV.x);
			Debug.Log ("x: " +  pixelUV.y);

            tex.SetPixel(Mathf.FloorToInt(pixelUV.x), Mathf.FloorToInt(pixelUV.y), Color.black);
            tex.Apply();
            if (paintRender != null) paintRender.material.SetTexture("_MainTex", tex);
        }
    }

    public void OnTriggerEnter(Collider colider)
    {
		if (colider.gameObject.tag != "Wood") return;

        paintRender = colider.gameObject.GetComponent<Renderer>();
        painting = true;
        tex = paintRender.material.GetTexture("_MainTex") as Texture2D;

		paintedWood = colider.gameObject;
		Rigidbody rbWood = paintedWood.GetComponent<Rigidbody> ();
		if (rbWood != null) {
			rbWood.isKinematic = true;
		}
		MeshCollider[] woodColliders = paintedWood.GetComponents<MeshCollider> ();
		foreach (MeshCollider mc in woodColliders) {
			if (!mc.isTrigger)
				mc.convex = false;
		}
    }


    public void OnTriggerExit(Collider colider)
    {
		if (colider.gameObject.tag != "Wood" ) return;
        paintRender = colider.gameObject.GetComponent<Renderer>();
        painting = false;

		if (paintedWood != null) {
			MeshCollider[] woodColliders = paintedWood.GetComponents<MeshCollider> ();
			foreach (MeshCollider mc in woodColliders) {
				if (!mc.convex)
					mc.convex = true;
			}
			Rigidbody rbWood = paintedWood.GetComponent<Rigidbody> ();
			if (rbWood != null) {
				rbWood.isKinematic = false;
			}
			paintedWood = null;
		}
    }
}

