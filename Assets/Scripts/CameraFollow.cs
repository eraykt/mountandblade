using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125F;
    public Vector3 offset; //kamera ile karakter arasýndaki mesafe

    private Vector3 smoothedPosition;

    void FixedUpdate() 
    {
        Vector3 desiredPosition = target.position + offset;
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;  
    }
}
