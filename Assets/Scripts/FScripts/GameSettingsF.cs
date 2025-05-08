using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettingsF : MonoBehaviour
{
    public static GameSettingsF Instance { get; private set; }

    public TMP_InputField enemyInput;
    public TMP_InputField allyInput;

    public int enemyCount;
    public int allyCount;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this.gameObject);
        else Instance = this;

        DontDestroyOnLoad(this.gameObject);
        Debug.Log("Caisti");
    }

    // Butona basýldýðýnda bu fonksiyon çaðrýlýr
    public void CountGetter(string SceneName)
    {
        if (int.TryParse(enemyInput.text, out int res_enemy) && int.TryParse(allyInput.text, out int res_allie))
        {
            enemyCount = res_enemy;
            allyCount = res_allie;
            SceneLoader(SceneName);
        }
        else
        {
            Debug.LogError("Invalid input values.");
        }
    }

    private void SceneLoader(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
