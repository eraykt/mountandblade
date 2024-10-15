using UnityEngine;

public class PlayerTopDownController : MonoBehaviour
{
    public CameraRaycast cameraRaycast;  // Kameradaki raycast scriptine referans
    public float rotationSpeed = 5f;     // D�n�� h�z�
    public float moveSpeed = 5f;         // Hareket h�z�

    private Vector3 targetPosition;      // Hedef pozisyon
    private bool isMoving = false;       // Hareket edip etmedi�ini kontrol eden bayrak

    void Update()
    {
        // Mouse sol tu�una bir kez t�klad���nda hedef pozisyonu belirle
        if (Input.GetMouseButtonDown(0))
        {
            targetPosition = cameraRaycast.GetMouseWorldPosition();
            isMoving = true;
        }

        // E�er hareket ediliyorsa karakteri hedefe d�nd�r ve hareket ettir
        if (isMoving)
        {
            RotateTowardsMouse();
            MoveToTarget();
        }
    }

    // Fare pozisyonuna do�ru d�nd�rme i�lemi
    void RotateTowardsMouse()
    {
        // Karakterin pozisyonundan fareye olan y�n� bul
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Y eksenini s�f�rla, sadece yatay d�zlemde d�nmesini istiyoruz.

        // E�er y�n vekt�r� s�f�r de�ilse karakteri o y�ne do�ru d�nd�r
        if (direction != Vector3.zero)
        {
            // Hedef y�n�ne do�ru bak�� a��s�n� hesapla
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Karakterin d�n���n�, yumu�ak bir �ekilde hedef rotasyona do�ru yap
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    // Karakteri hedef pozisyona do�ru hareket ettir
    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // E�er karakter hedefe ula�t�ysa hareketi durdur
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }
    }
}
