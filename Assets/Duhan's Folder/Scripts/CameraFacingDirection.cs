using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class CameraFacingDirection : MonoBehaviour
    {
        //private Transform mainCamera;

        CamController controller;
        //private Transform mainCamera;
        [SerializeField] private Camera mainCamera;

       

        void LateUpdate()
        {
            if(mainCamera==null)
                return;

            // Kameraya doðru dönmek için yön vektörü oluþtur
            Vector3 directionToCamera = mainCamera.transform.position - transform.position;

            // Kameranýn eðim açýsýna göre Canvas'ý hizalama
            // Kamera tamamen yataysa (top-down), Canvas yatay hizalanmalý
            float cameraPitch = Vector3.Angle(Vector3.up, mainCamera.transform.forward);

            if (cameraPitch < 45f) // Kamera neredeyse tamamen yukarýdan bakýyorsa
            {
                transform.rotation = Quaternion.Euler(90f, mainCamera.transform.eulerAngles.y, 0f);
            }
            else // Kamera eðimli veya yataysa
            {
                transform.rotation = Quaternion.LookRotation(-directionToCamera.normalized);
            }
        }

    }
}
