//using UnityEngine;
//using Cinemachine;

//public class VirtualCameraController : MonoBehaviour
//{
//    [Header("Virtual Cameras")]
//    public CinemachineVirtualCamera[] virtualCameras;

//    [Header("Scroll Settings")]
//    public int maxPriority = 10; // Top Down kamera için maksimum öncelik
//    public int minPriority = 5;  // Third Person kamera için minimum öncelik
//    public float smoothTime = 0.2f; // Yay çizme etkisi

//    private int currentPriorityIndex;
//    private float currentSmoothValue;
//    private float targetSmoothValue;

//    private void Start()
//    {
//        // Baþlangýç kamera ayarýný yap
//        currentPriorityIndex = virtualCameras.Length - 1;
//        UpdateCameraPriorities();
//    }

//    private void Update()
//    {
//        // Scroll giriþini oku
//        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

//        if (scrollInput > 0) // Yaklaþma
//        {
//            currentPriorityIndex = Mathf.Max(0, currentPriorityIndex - 1);
//        }
//        else if (scrollInput < 0) // Uzaklaþma
//        {
//            currentPriorityIndex = Mathf.Min(virtualCameras.Length - 1, currentPriorityIndex + 1);
//        }

//        // Hedef smooth deðerini güncelle
//        targetSmoothValue = currentPriorityIndex;

//        // Yumuþak geçiþ için interpolasyon
//        currentSmoothValue = Mathf.Lerp(currentSmoothValue, targetSmoothValue, Time.deltaTime / smoothTime);

//        UpdateCameraPriorities();
//    }

//    private void UpdateCameraPriorities()
//    {
//        for (int i = 0; i < virtualCameras.Length; i++)
//        {
//            if (i == Mathf.RoundToInt(currentSmoothValue))
//            {
//                virtualCameras[i].Priority = maxPriority; // Aktif kamera en yüksek öncelikte
//            }
//            else
//            {
//                virtualCameras[i].Priority = minPriority; // Diðer kameralar düþük öncelikte
//            }
//        }
//    }
//}
using UnityEngine;
using Cinemachine;

public class VirtualCameraController : MonoBehaviour
{
    [Header("Virtual Cameras")]
    public CinemachineVirtualCamera[] virtualCameras;

    [Header("Scroll Settings")]
    public int maxPriority = 10; // Top Down kamera için maksimum öncelik
    public int minPriority = 5;  // Third Person kamera için minimum öncelik
    public float offsetStep = 2.0f; // Scroll baþýna offset deðiþimi
    public float maxOffsetY = 50.0f; // Maksimum offset yüksekliði
    public float minOffsetY = 10.0f; // Minimum offset yüksekliði

    private int currentPriorityIndex;
    private CinemachineTransposer topDownTransposer;

    private void Start()
    {
        // Baþlangýç kamera ayarýný yap
        currentPriorityIndex = virtualCameras.Length - 1;
        UpdateCameraPriorities();

        // Priority 10 olan kameranýn Transposer'ýný al
        foreach (var cam in virtualCameras)
        {
            if (cam.Priority == maxPriority)
            {
                var body = cam.GetCinemachineComponent<CinemachineTransposer>();
                if (body != null)
                {
                    topDownTransposer = body;
                }
            }
        }
    }

    private void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0 && currentPriorityIndex == virtualCameras.Length - 1) // Top Down kameradaysak
        {
            AdjustFollowOffset(scrollInput);
        }
        else if (scrollInput > 0) // Yaklaþma
        {
            currentPriorityIndex = Mathf.Max(0, currentPriorityIndex - 1);
        }
        else if (scrollInput < 0) // Uzaklaþma
        {
            currentPriorityIndex = Mathf.Min(virtualCameras.Length - 1, currentPriorityIndex + 1);
        }

        UpdateCameraPriorities();
    }

    private void AdjustFollowOffset(float scrollInput)
    {
        if (topDownTransposer != null)
        {
            Vector3 offset = topDownTransposer.m_FollowOffset;

            // Scroll giriþine göre y deðerini artýr/azalt
            offset.y -= scrollInput * offsetStep;
            offset.y = Mathf.Clamp(offset.y, minOffsetY, maxOffsetY);

            topDownTransposer.m_FollowOffset = offset;

            // Eðer y deðeri maksimuma ulaþtýysa kamerayý deðiþtir
            if (offset.y >= maxOffsetY && scrollInput > 0)
            {
                currentPriorityIndex--;
            }
        }
    }

    private void UpdateCameraPriorities()
    {
        for (int i = 0; i < virtualCameras.Length; i++)
        {
            if (i == currentPriorityIndex)
            {
                virtualCameras[i].Priority = maxPriority;
            }
            else
            {
                virtualCameras[i].Priority = minPriority;
            }
        }
    }
}
