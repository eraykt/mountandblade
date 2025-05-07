using UnityEngine;
using UnityEngine.SceneManagement;

namespace MountAndBlade
{
    public class Intercene : MonoBehaviour
    {
        public void LoadScene()
        {
            SceneManager.LoadScene("WsScene");
        }
    }
}
