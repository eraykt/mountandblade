using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MountAndBlade
{
    public class CameraRay : MonoBehaviour
    {
        public static CameraRay instance;

        [SerializeField]private Camera mainCamera; // Reference to your main camera

        private RaycastHit hit; // Stores information about what the ray hits
        private Ray ray;
        [SerializeField] LayerMask layerMask;
        public bool isItEnemy = false;

        //aktif kameranun bilgisini al
        //herzaman aktif kameradan transform forward yönünde ray at.
        //neye çarptýðý bilgisine eriþ.
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else 
            {
                Destroy(gameObject);
            }
        }


        public void ignoreTriggersEnemyCheck()
        {
            if (Input.GetMouseButtonDown(0))// daha sonra event sistemiyle onClicked yap.
            {
                // Create a ray from the camera to the mouse position
                ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                // Perform the raycast
                if (Physics.Raycast(ray, out hit, Mathf.Infinity,layerMask,QueryTriggerInteraction.Ignore))
                {
                    
                    if (hit.collider.GetComponent<EnemyHandler>() != null) 
                    {
                        isItEnemy = true;
                    }
                    else
                    {
                        isItEnemy = false;
                    }
                }
                
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * 100f); // Draw the ray 100 units long
        }
    }
}
