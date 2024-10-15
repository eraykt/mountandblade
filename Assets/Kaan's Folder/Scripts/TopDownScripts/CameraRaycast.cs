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

    private Camera cam;                       // Kamera referans�

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

    // WASD ile haritada gezinme ve s�n�rlarla s�n�rlama
    void HandleMovement()
    {
        Vector3 pos = transform.position;

        // WASD ile hareket
        if (Input.GetKeyDown(KeyCode.S))
            pos.z += panSpeed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.W))
            pos.z -= panSpeed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.D))
            pos.x -= panSpeed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.A))
            pos.x += panSpeed * Time.deltaTime;

        // Pozisyonu s�n�rlar i�inde tut
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        // Kameray� yeni pozisyona ta��
        transform.position = pos;
    }

    // Ekran�n �st�ne gidince yak�nla�t�r, alt�na gidince uzakla�t�r
    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;

        // Fare ekran�n �st k�sm�na yak�nsa zoom in
        if (Input.mousePosition.y >= Screen.height - 10)
        {
            pos.y -= scrollSpeed * Time.deltaTime;
        }
        // Fare ekran�n alt k�sm�na yak�nsa zoom out
        else if (Input.mousePosition.y <= 10)
        {
            pos.y += scrollSpeed * Time.deltaTime;
        }
        // Fare tekerle�i ile zoom kontrol�
        else if (scroll != 0)
        {
            pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
        }

        // Zoom mesafesini s�n�rla
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Kameray� yeni pozisyona ta��
        transform.position = pos;
    }

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
