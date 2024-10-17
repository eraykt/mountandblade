using System;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{

    public float panSpeed = 20f;              // Kamera hareket h�z� (WASD)
    public Vector2 panLimit;                  // Kamera i�in s�n�rlar (bound)
    public float scrollSpeed = 10f;           // Zoom h�z�
    public float minY = 20f;                  // En yak�n zoom mesafesi
    public float maxY = 120f;                 // En uzak zoom mesafesi
    public float rotationSpeed = 100f;        // Kamera rotasyon h�z�

    public float zoomSpeed = 10f; // Zoom h�z�n� ayarlamak i�in
    public float minZoom = 5f;    // Minimum zoom de�eri
    public float maxZoom = 20f;   // Maksimum zoom de�eri

    private Camera cam;                       // Kamera referans�

    public GameObject closeCam;
    public GameObject topCam;

    private void Start()
    {
        cam = Camera.main;  // Ana kamera referans�n� al
    }

    void Update()
    {
        HandleMovement();   // WASD ile gezinme
        HandleZoom();       // Zoom in/out i�lemi
        HandleRotation();   // Kamera rotasyonu
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 10;
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        if (scroll > 0) // Zoom in
        {
            transform.position = Vector3.Lerp(transform.position, closeCam.transform.position, Time.deltaTime * zoomSpeed * scroll);
            transform.rotation = Quaternion.Slerp(transform.rotation, closeCam.transform.rotation, Time.deltaTime * zoomSpeed * scroll);
        }
        if (scroll < 0) // Zoom out
        {
            transform.position = Vector3.Lerp(transform.position, topCam.transform.position, Time.deltaTime * zoomSpeed * Mathf.Abs(scroll));
            transform.rotation = Quaternion.Slerp(transform.rotation, topCam.transform.rotation, Time.deltaTime * zoomSpeed * Mathf.Abs(scroll));
        }
    }


    // WASD ile haritada gezinme ve s�n�rlarla s�n�rlama
    // TODO : HATALARI FIXLE
    void HandleMovement()
    {
        bool canMoveOnMap = false;
        Vector3 pos = transform.position;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        canMoveOnMap = (horizontal != 0);
        canMoveOnMap = (vertical != 0);
        Debug.Log(canMoveOnMap);

        while (canMoveOnMap)
        {
            // WASD ile hareket
            if (Input.GetKeyDown(KeyCode.W))
                pos.x += panSpeed * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.S))
                pos.x -= panSpeed * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.D))
                pos.y -= panSpeed * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.A))
                pos.y += panSpeed * Time.deltaTime;

            // Pozisyonu s�n�rlar i�inde tut
            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

            // Kameray� yeni pozisyona ta��
            transform.position = pos;
        }

    }


    //void HandleZoom123()
    //{
    //    float scroll = Input.GetAxis("Mouse ScrollWheel");

    //    Vector3 pos = transform.position;

    //    // Fare ekran�n �st k�sm�na yak�nsa zoom in
    //    if (Input.mousePosition.y >= Screen.height - 10)
    //    {
    //        pos.y -= scrollSpeed * Time.deltaTime;
    //    }
    //    // Fare ekran�n alt k�sm�na yak�nsa zoom out
    //    else if (Input.mousePosition.y <= 10)
    //    {
    //        pos.y += scrollSpeed * Time.deltaTime;
    //    }
    //    // Fare tekerle�i ile zoom kontrol�
    //    else if (scroll != 0)
    //    {
    //        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
    //    }

    //    // Zoom mesafesini s�n�rla
    //    pos.y = Mathf.Clamp(pos.y, minY, maxY);

    //    // Kameray� yeni pozisyona ta��
    //    transform.position = pos;
    //}


   

    // Ekran�n soluna ve sa��na gidince kameray� d�nd�r
    void HandleRotation()
    {
        // Ekran�n sa� taraf�na yak�nsa sa�a d�n
        if (Input.mousePosition.x >= Screen.width - 10)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
        // Ekran�n sol taraf�na yak�nsa sola d�n
        else if (Input.mousePosition.x <= 10)
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    internal Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // Fare pozisyonundan bir ���n g�nder
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);      // Y = 0 d�zleminde bir zemin d�zlemi olu�tur
        float enter;

        if (groundPlane.Raycast(ray, out enter))
        {
            return ray.GetPoint(enter);  // I��n�n kesi�ti�i d�nya pozisyonunu d�nd�r
        }

        return Vector3.zero;  // E�er ���n bir �eyle kesi�mezse
    }

}
