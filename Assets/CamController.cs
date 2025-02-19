//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Cinemachine;
//using System;
//public class CamController : MonoBehaviour
//{
//    [SerializeField]
//    private float moveSpeed = 20f;
//    [SerializeField]
//    private CinemachineFreeLook topCam;
//    [SerializeField]
//    private CinemachineFreeLook closeCam;
//    [SerializeField]
//    private float rotationSpeed = 100f;
//    private bool canMove;

//    private CinemachineFreeLook activeCam;
//    private CinemachineFreeLook unActiveCam;
//    private CinemachineTransposer transposer;

//    private void Start()
//    {
//        // CinemachineTransposer bileþenine eriþim
//      //  transposer = closeCam.GetCinemachineComponent<CinemachineTransposer>();
//    }
//    private void Update()
//    {
//        UpdateActiveCamera();
//        deneme();
//        CloseDistanceMovementFixer();
//    }

//    private void UpdateActiveCamera()
//    {
//        if (activeCam == null) Debug.LogWarning("ACTIVE CAM CAN NOT FOUND");
//        if (unActiveCam == null) Debug.LogWarning("UNACTIVE CAM CAN NOT FOUND");
//        // Daha yüksek Priority deðerine sahip olan kamera activeCam olarak atanýr
//        activeCam = closeCam.m_Priority > topCam.m_Priority ? closeCam : topCam;
//        unActiveCam = closeCam.m_Priority > topCam.m_Priority ? topCam : closeCam;

//        Debug.Log("Active Camera: " + activeCam.name);
//    }

//    private void MovementHandler()
//    {
//        if (canMove)
//        {
//            float horizontal = Input.GetAxis("Horizontal");
//            float vertical = Input.GetAxis("Vertical");

//            Vector3 moveDir = new Vector3(vertical, 0, horizontal).normalized;

//            transform.Translate(moveDir * moveSpeed * Time.deltaTime);

//            float mouseX = Input.GetAxis("Mouse X");
//            transform.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime);
//        }
//    }

//    private void CloseDistanceMovementFixer()
//    {
//        canMove = topCam.m_YAxis.Value > 0.5f ? true : false;
//        if (!canMove)
//        {
//            transform.Translate(Vector3.zero);
//        }
//    }

//    private void deneme()
//    {
//        if (activeCam.m_YAxis.Value == 1f && unActiveCam.m_YAxis.Value < 0.1f)
//        {
//            activeCam.m_Priority = 11;
//            unActiveCam.m_Priority = 9;
//        }
//        //if(activeCam.m_YAxis.Value < 0.1f && unActiveCam.m_YAxis.Value == 1f)
//        else
//        {
//            activeCam.m_Priority = 9;
//            unActiveCam.m_Priority = 11;
//        }
//    }
//}


using System.Collections;
using UnityEngine;
using Cinemachine;
using MountAndBlade;

public class CamController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 20f;
    [SerializeField]
    private CinemachineFreeLook topCam;
    [SerializeField]
    private CinemachineFreeLook closeCam;
    [SerializeField]
    //private float rotationSpeed = 100f;
    private bool canMove;

    private CinemachineFreeLook activeCam;

    private void Awake()
    {
        // Ýlk baþta closeCam'i aktif kamera olarak ayarla
        activeCam = closeCam;
        SetInitialPriorities();
    }

    private void Update()
    {
        AdjustCameraPriorities(); // Priority'leri kontrol et
        UpdateActiveCamera();     // Aktif kamerayý güncelle
        MovementHandler();
        CloseDistanceMovementFixer(); // Hareket kontrolü
    }

    private void SetInitialPriorities()
    {
        // Ýlk baþlangýçta kameralarýn Priority deðerlerini ayarla
        closeCam.m_Priority = 10;
        topCam.m_Priority = 20; // CloseCam varsayýlan olarak aktif
    }

    private void UpdateActiveCamera()
    {
        // Daha düþük Priority deðerine sahip olan kamera aktif olur
        activeCam = closeCam.m_Priority < topCam.m_Priority ? topCam : closeCam;
        Debug.Log($"Aktif Kamera: {activeCam.name}");
    }

    private void AdjustCameraPriorities()
    {
        // Eðer closeCam aktif ve Y ekseni 1'e ulaþýrsa, öncelik deðiþimi
        if (activeCam.m_YAxis.Value == 1f)
        {
            activeCam = closeCam;
            closeCam.m_Priority = 10;
            topCam.m_Priority = 20; // TopCam aktif hale gelir
        }
        // Eðer topCam aktif ve Y ekseni 0.1'den küçükse, öncelik deðiþimi
        else if (activeCam.m_YAxis.Value < 0.1f)
        {
            activeCam = topCam;
            closeCam.m_Priority = 20; // CloseCam aktif hale gelir
            topCam.m_Priority = 10;
        }
    }
    private void MovementHandler()
    {
        //if (canMove)
        
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDir = new Vector3(horizontal, 0, vertical).normalized;

            transform.Translate(moveDir * moveSpeed * Time.deltaTime);

            
        
    }
    private void CloseDistanceMovementFixer()
    {
        // TopCam'deki Y ekseni hareketi belirler
        bool canMove = topCam.m_YAxis.Value > 0.5f;

        if (!canMove)
        {
            transform.Translate(Vector3.zero);
        }
    }

   
}

