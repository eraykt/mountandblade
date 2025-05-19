using MountAndBlade;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManagerF : MonoBehaviour
{
    public static GameManagerF Instance;
    public int enemyCount;
    public int allyCount;
    

    public GameObject enemyPrefab;
    public GameObject allyPrefab;
    public GameObject playerPrefab;

    // StateF SO'lar� i�in referanslar
    private StateF attackStateSO;
    private StateF chaseStateSO;
    private StateF idleStateSO;

    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> allies = new List<GameObject>();
    private bool CanCheckLists = false;


    public GameObject AlliesWonPanel;
    public GameObject EnemiesWonPanel;
    public GameObject EndGameInfo;


    public GameObject enemySpawnPointL;
    public GameObject enemySpawnPointR;

    public GameObject allySpawnPointL;
    public GameObject allySpawnPointR;

    public GameObject PlayerSpawnPoint;



    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateEnemyAndAllyCount();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var enemy in enemies)   
            {
                enemy.GetComponent<EnemyBaseF>().TakeDamage(1000);
            }
        }
    }

    public void UpdateEnemyAndAllyCount()
    {
        if (GameManager.instance.GetEnemyCount() != 0)
            enemyCount = GameManager.instance.GetEnemyCount();
        else Debug.Log($"Enemy Count �ekilirken : {GameManager.instance.GetEnemyCount()}");

        if (InterSceneManager.Instance.playerData.troopCount != 0) 
            allyCount = InterSceneManager.Instance.playerData.troopCount;
        else Debug.Log($"Ally Count �ekilirken : {GameManager.instance.GetAllyCount()}");

        SpawnEnemiesAndAllies();
    }

    public void Register(GameObject unit)
    {
        if (unit.CompareTag("Enemy"))
        {
            enemies.Add(unit); // Listeye Ekliyoruz
        }
        else if (unit.CompareTag("Allies") || unit.CompareTag("Player"))
        {
            allies.Add(unit);  // Listeye Ekliyoruz
        }
    }

    public void Unregister(GameObject unit)
    {
        if (unit.CompareTag("Enemy"))
        {
            enemies.Remove(unit); // Listeden siliyoruz ve liste 0 olursa Victory triggerlan�yor
            if (enemies.Count == 0)
            {
                TriggerVictory("Allies");
                GameManager.extraUnitAmount++;

            }
        }
        else if (unit.CompareTag("Allies") || unit.CompareTag("Player"))
        {
            allies.Remove(unit);
            if (allies.Count == 0) TriggerVictory("Enemy");
        }
        Debug.Log($"Unregistered : {unit.name}");
    }

    private void TriggerVictory(string winnerTag)
    {
        if (CanCheckLists)
        {
            Debug.Log($"{winnerTag} kazand�!");

            List<GameObject> winners = winnerTag == "Allies" ? allies : enemies;
            InterSceneManager.Instance.playerWonLastBattle = true;


            foreach (var unit in winners)
            {
                var enemyBase = unit.GetComponent<EnemyBaseF>();
                if (enemyBase != null)
                    enemyBase.animator.SetBool("IsVictory", true);
                if (winnerTag == "Allies")
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    Animator playerAnimator = player.GetComponentInChildren<Animator>();
                    playerAnimator.SetBool("IsVictory", true);
                    StartCoroutine(AlliesWin());
                }
            }
        }
    }

    public IEnumerator AlliesWin()
    {
        Cursor.lockState = CursorLockMode.None;
        
        yield return new WaitForSeconds(3);

        AlliesWonPanel.SetActive(true);


        GameManager.instance.OnBattleWon();
    }

    public void EnemiesWin()
    {
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Enemiler Kazandı Kardeşim");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            EnemyBaseF enemyScript = enemy.GetComponent<EnemyBaseF>();
            enemyScript.animator.SetBool("IsVictory", true);
        }

        GameManager.extraUnitAmount--;

    

        EndGameInfo.SetActive(true);
        EnemiesWonPanel.SetActive(true);
    }

    private void SpawnEnemiesAndAllies()
    {
        // Eski d��man ve ally'leri sil
        DestroyExistingUnits();

        if (enemyPrefab != null)
        {
            float spacing = 2f; // Her düşman arasındaki boşluk
            int halfCount = enemyCount / 2;

            for (int i = 0; i < enemyCount; i++)
            {
                Vector3 spawnPos;
                if (i % 2 == 0) // Sol spawn hattı (Left)
                {
                    spawnPos = enemySpawnPointL.transform.position + new Vector3(i / 2 * spacing, 0f, 0f);
                }
                else // Sağ spawn hattı (Right)
                {
                    spawnPos = enemySpawnPointR.transform.position + new Vector3(i / 2 * spacing, 0f, 0f);
                }

                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                Register(enemy);
                Debug.Log("Düşman " + i + " oluşturuldu");

                CreateAndAssignStateSO(enemy);
            }
        }
        else Debug.LogError("EnemyPrefab boş");


        if (enemyPrefab != null)
        {
            float spacing = 2f; // Her düşman arasındaki boşluk
            int halfCount = enemyCount / 2;

            for (int i = 0; i < enemyCount; i++)
            {
                Vector3 spawnPos;
                if (i % 2 == 0) // Sol spawn hattı (Left)
                {
                    spawnPos = allySpawnPointL.transform.position + new Vector3(i / 2 * spacing, 0f, 0f);
                }
                else // Sağ spawn hattı (Right)
                {
                    spawnPos = allySpawnPointR.transform.position + new Vector3(i / 2 * spacing, 0f, 0f);
                }

                GameObject enemy = Instantiate(allyPrefab, spawnPos, Quaternion.identity);
                Register(enemy);
                Debug.Log("Düşman " + i + " oluşturuldu");

                CreateAndAssignStateSO(enemy);
            }
        }
        else Debug.LogError("EnemyPrefab boş");

        if (playerPrefab != null)
        {
            GameObject player = Instantiate(playerPrefab, PlayerSpawnPoint.transform.position, Quaternion.identity);
            Register(player);
        }

        CanCheckLists = true;
        Debug.Log($"Spawned {enemyCount} enemies and {allyCount} allies.");
    }

    // Sahnedeki eski d��man ve ally objelerini silmek
    private void DestroyExistingUnits()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }

        GameObject[] allies = GameObject.FindGameObjectsWithTag("Allies");
        foreach (var ally in allies)
        {
            Destroy(ally);
        }
    }



    // StateF SO'lar�n� olu�turur ve prefab'a atar
    // Burada SO olarak olu�turlan Stateler enemylere referans olarak veriliyor otomatik olarak
    private void CreateAndAssignStateSO(GameObject unit)
    {
        // Runtime s�ras�nda yeni StateF SO'lar� olu�tur
        attackStateSO = ScriptableObject.CreateInstance<AttackStateF>();
        chaseStateSO = ScriptableObject.CreateInstance<ChaseStateF>();
        idleStateSO = ScriptableObject.CreateInstance<IdleStateF>();

        // Prefab'a StateF SO'lar�n� atamak i�in
        EnemyBaseF enemyBase = unit.GetComponent<EnemyBaseF>();
        if (enemyBase != null)
        {
            enemyBase.attackState = attackStateSO;
            enemyBase.chaseState = chaseStateSO;
            enemyBase.idleState = idleStateSO;
        }

        Debug.Log($"Runtime s�ras�nda {unit.name} i�in State SO'lar� olu�turuldu ve atand�.");
    }

    public void GoBackMapScene()
    {
        SceneManager.LoadScene("02_MapScene");
    }

    public void LostGame() => SceneManager.LoadScene("00_Credits");
    
}
