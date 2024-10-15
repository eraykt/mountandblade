using UnityEngine;

public class PlayerTopDownController : MonoBehaviour
{
    public CameraRaycast cameraRaycast;  // Kameradaki raycast scriptine referans
    public float rotationSpeed = 5f;     // Dönüþ hýzý
    public float moveSpeed = 5f;         // Hareket hýzý

    private Vector3 targetPosition;      // Hedef pozisyon
    private bool isMoving = false;       // Hareket edip etmediðini kontrol eden bayrak

    void Update()
    {
        // Mouse sol tuþuna bir kez týkladýðýnda hedef pozisyonu belirle
        if (Input.GetMouseButtonDown(0))
        {
            targetPosition = cameraRaycast.GetMouseWorldPosition();
            isMoving = true;
        }

        // Eðer hareket ediliyorsa karakteri hedefe döndür ve hareket ettir
        if (isMoving)
        {
            RotateTowardsMouse();
            MoveToTarget();
        }
    }

    // Fare pozisyonuna doðru döndürme iþlemi
    void RotateTowardsMouse()
    {
        // Karakterin pozisyonundan fareye olan yönü bul
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Y eksenini sýfýrla, sadece yatay düzlemde dönmesini istiyoruz.

        // Eðer yön vektörü sýfýr deðilse karakteri o yöne doðru döndür
        if (direction != Vector3.zero)
        {
            // Hedef yönüne doðru bakýþ açýsýný hesapla
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Karakterin dönüþünü, yumuþak bir þekilde hedef rotasyona doðru yap
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    // Karakteri hedef pozisyona doðru hareket ettir
    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Eðer karakter hedefe ulaþtýysa hareketi durdur
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }
    }
}
