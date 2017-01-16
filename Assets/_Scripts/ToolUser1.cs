using UnityEngine;
using System.Collections;

public class ToolUser1 : MonoBehaviour {

	public Material capMaterial;

    public GameObject[] cut(Vector3 position, Quaternion rotation, GameObject victim1)
    {
        this.gameObject.transform.position = position;
        this.gameObject.transform.rotation = rotation;
        GameObject victim = victim1;
        if (capMaterial == null)
            capMaterial = victim.GetComponent<Renderer>().material;
        return BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, capMaterial);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5.0f);
        Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * 5.0f);
        Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * 5.0f);
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + -transform.up * 0.5f);
    }
}
