using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class glueMe : MonoBehaviour
{

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Wood")
        {
            //ContactPoint contact = collision.contacts[0];
            GameObject collidedObject = collider.gameObject;
            if (this.gameObject.transform.parent == null)
            {
                Rigidbody rigi = this.gameObject.GetComponent<Rigidbody>();
                if (rigi != null)
                {
                    Destroy(rigi);
                }
                this.gameObject.transform.rotation = collidedObject.transform.rotation;
				this.gameObject.transform.position += transform.up * 0.028f; 
                //Rigidbody rigi = this.gameObject.GetComponent<Rigidbody>();
                //rigi.constraints = RigidbodyConstraints.FreezeAll;
                //rigi.freezeRotation = true;
                this.transform.SetParent(collidedObject.transform);
            }
            else if(collidedObject.transform.parent != transform.parent && collidedObject.transform != transform.parent)
            {
                Pickup pu = collidedObject.GetComponent<Pickup>();
                if (pu != null)
                    pu.GetReleased();

                collidedObject.transform.SetParent(this.gameObject.transform.parent);

                Rigidbody rbWood = collidedObject.GetComponent<Rigidbody>();
                if (rbWood != null)
                {
                    Destroy(rbWood);
                    //rbWood.constraints = RigidbodyConstraints.FreezeAll;
                    //rbWood.freezeRotation = true;
                }
                collidedObject.tag = "GluedWood";

                // combineColliderWithParent(collider.gameObject);

            }
        }
    }

    /// <summary>
    /// experiemental!
    /// </summary>
    /// <param name="collidingGO"></param>
    void combineColliderWithParent(GameObject collidingGO)
    {
        
        MeshFilter parentMF = transform.parent.gameObject.GetComponent<MeshFilter>();
        MeshFilter toCombineMF = collidingGO.GetComponent<MeshFilter>();
        if(parentMF != null && toCombineMF != null)
        {
            List<MeshFilter> mfList = new List<MeshFilter>();
            mfList.Add(parentMF);
            mfList.Add(toCombineMF);

            CombineInstance[] combine = new CombineInstance[mfList.Count];
            int i = 0;
            while (i < mfList.Count)
            {
                combine[i].mesh = mfList[i].sharedMesh;
                combine[i].transform = mfList[i].transform.localToWorldMatrix;
                mfList[i].gameObject.SetActive(false);
                i++;
            }
            parentMF.mesh = new Mesh();
            parentMF.mesh.CombineMeshes(combine);
            parentMF.gameObject.SetActive(true);
        }
    }

}
