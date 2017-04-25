using UnityEngine;
using System.Collections;

public class MagneticMarker : MonoBehaviour {
    
    public Vector3 targetPosition;

    public Renderer currentRenderer;

    void Start()
    {
        // TODO
        // targetPosition
        currentRenderer = GetComponent<Renderer>();
    }
}
