using UnityEngine;

public class FreeCameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f; // Kamera hareket hýzý
    public float rotationSpeed = 100f; // Kamera rotasyon hýzý
    [SerializeField] private Transform activeCameraTransform;

    void Start()
    {
        if (activeCameraTransform == null)
        {
            // Eðer Inspector'da atanmamýþsa Main Camera'yý otomatik ata
            if (Camera.main != null)
            {
                activeCameraTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogError("Main Camera bulunamadý! activeCameraTransform atayýn.");
            }
        }
    }


    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        // WASD giriþlerini al
        float horizontalInput = Input.GetAxis("Horizontal"); // A ve D için
        float verticalInput = Input.GetAxis("Vertical");   // W ve S için

        // Hareket yönünü hesapla
        Vector3 forwardMovement = activeCameraTransform.forward * verticalInput;
        Vector3 rightMovement = activeCameraTransform.right * horizontalInput;

        // Kamerayý hareket ettir
        Vector3 movement = (forwardMovement + rightMovement).normalized * moveSpeed * Time.deltaTime;
        activeCameraTransform.position += movement;
    }

    private void HandleRotation()
    {
        // Fare sað týk ile rotasyon
        if (Input.GetMouseButton(1)) // Sað týk basýlýysa
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Kamerayý y ekseninde döndür (yatay)
            activeCameraTransform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime, Space.World);

            // Kamerayý x ekseninde döndür (dikey)
            activeCameraTransform.Rotate(Vector3.left, mouseY * rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}
