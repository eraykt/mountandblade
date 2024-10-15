using System;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{

    public float panSpeed = 20f;              // Kamera hareket hýzý (WASD)
    public Vector2 panLimit;                  // Kamera için sýnýrlar (bound)
    public float scrollSpeed = 10f;           // Zoom hýzý
    public float minY = 20f;                  // En yakýn zoom mesafesi
    public float maxY = 120f;                 // En uzak zoom mesafesi
    public float rotationSpeed = 100f;        // Kamera rotasyon hýzý

    private Camera cam;                       // Kamera referansý

    private void Start()
    {
        cam = Camera.main;  // Ana kamera referansýný al
    }

    void Update()
    {
        HandleMovement();   // WASD ile gezinme
        HandleZoom();       // Zoom in/out iþlemi
        HandleRotation();   // Kamera rotasyonu
    }

    // WASD ile haritada gezinme ve sýnýrlarla sýnýrlama
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

        // Pozisyonu sýnýrlar içinde tut
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        // Kamerayý yeni pozisyona taþý
        transform.position = pos;
    }

    // Ekranýn üstüne gidince yakýnlaþtýr, altýna gidince uzaklaþtýr
    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;

        // Fare ekranýn üst kýsmýna yakýnsa zoom in
        if (Input.mousePosition.y >= Screen.height - 10)
        {
            pos.y -= scrollSpeed * Time.deltaTime;
        }
        // Fare ekranýn alt kýsmýna yakýnsa zoom out
        else if (Input.mousePosition.y <= 10)
        {
            pos.y += scrollSpeed * Time.deltaTime;
        }
        // Fare tekerleði ile zoom kontrolü
        else if (scroll != 0)
        {
            pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
        }

        // Zoom mesafesini sýnýrla
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Kamerayý yeni pozisyona taþý
        transform.position = pos;
    }

    // Ekranýn soluna ve saðýna gidince kamerayý döndür
    void HandleRotation()
    {
        // Ekranýn sað tarafýna yakýnsa saða dön
        if (Input.mousePosition.x >= Screen.width - 10)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
        // Ekranýn sol tarafýna yakýnsa sola dön
        else if (Input.mousePosition.x <= 10)
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    internal Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // Fare pozisyonundan bir ýþýn gönder
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);      // Y = 0 düzleminde bir zemin düzlemi oluþtur
        float enter;

        if (groundPlane.Raycast(ray, out enter))
        {
            return ray.GetPoint(enter);  // Iþýnýn kesiþtiði dünya pozisyonunu döndür
        }

        return Vector3.zero;  // Eðer ýþýn bir þeyle kesiþmezse
    }

}
