using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManagerF : MonoBehaviour
{
    public static GameManagerF Instance;
    public int enemyCount;
    public int allyCount;

    public GameObject enemyPrefab;
    public GameObject allyPrefab;

    // StateF SO'larý için referanslar
    private StateF attackStateSO;
    private StateF chaseStateSO;
    private StateF idleStateSO;

    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> allies = new List<GameObject>();
    private bool CanCheckLists = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateEnemyAndAllyCount();
    }

    public void UpdateEnemyAndAllyCount()
    {
        if (GameSettingsF.Instance.enemyCount != 0)
            enemyCount = GameSettingsF.Instance.enemyCount;
        else Debug.LogWarning("GameSettingsF.Instance.enemyCount 0");
        if (GameSettingsF.Instance.allyCount != 0)
            allyCount = GameSettingsF.Instance.allyCount;
        else Debug.LogWarning("GameSettingsF.Instance.allyCount 0");

        SpawnEnemiesAndAllies();
    }

    public void Register(GameObject unit)
    {
        if (unit.CompareTag("Enemy"))
        {
            enemies.Add(unit); // Listeye Ekliyoruz
        }
        else if (unit.CompareTag("Allies"))
        {
            allies.Add(unit);  // Listeye Ekliyoruz
        }
    }

    public void Unregister(GameObject unit)
    {
        if (unit.CompareTag("Enemy"))
        {
            enemies.Remove(unit); // Listeden siliyoruz ve liste 0 olursa Victory triggerlanýyor
            if (enemies.Count == 0) TriggerVictory("Allies");
        }
        else if (unit.CompareTag("Allies"))
        {
            allies.Remove(unit);
            if (allies.Count == 0) TriggerVictory("Enemy");
        }
    }

    private void TriggerVictory(string winnerTag)
    {
        if (CanCheckLists)
        {
            Debug.Log($"{winnerTag} kazandý!");

            List<GameObject> winners = winnerTag == "Allies" ? allies : enemies;

            foreach (var unit in winners)
            {
                var enemyBase = unit.GetComponent<EnemyBaseF>();
                if (enemyBase != null)
                    enemyBase.animator.SetBool("IsVictory", true);
            }
        }
    }


    private void SpawnEnemiesAndAllies()
    {
        // Eski düþman ve ally'leri sil
        DestroyExistingUnits();

        if (enemyPrefab != null)
        {
            // Düþmanlarý spawn et
            for (int i = 0; i < enemyCount; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab, GetRandomPosition(), Quaternion.identity);
                Register(enemy);
                Debug.Log("Düþman " + i + " oluþturuldu");
                // Düþman için StateF SO'larýný oluþtur ve ata
                CreateAndAssignStateSO(enemy);
            }
        }
        else Debug.LogError("EnemyPrefab boþ");

        if (allyPrefab != null)
        {
            // Ally'leri spawn et
            for (int i = 0; i < allyCount; i++)
            {
                GameObject ally = Instantiate(allyPrefab, GetRandomPosition(), Quaternion.identity);
                Register(ally);
                Debug.Log("Ally " + i + " oluþturuldu");

                // Ally için StateF SO'larýný oluþtur ve ata
                CreateAndAssignStateSO(ally);
            }
        }
        else Debug.LogError("AllyPrefab boþ");

        CanCheckLists = true;
        Debug.Log($"Spawned {enemyCount} enemies and {allyCount} allies.");
    }

    // Sahnedeki eski düþman ve ally objelerini silmek
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

    // Düþman ve ally'ler rastgele bir pozisyonda spawn olsun
    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, 0f, z);
    }

    // StateF SO'larýný oluþturur ve prefab'a atar
    // Burada SO olarak oluþturlan Stateler enemylere referans olarak veriliyor otomatik olarak
    private void CreateAndAssignStateSO(GameObject unit)
    {
        // Runtime sýrasýnda yeni StateF SO'larý oluþtur
        attackStateSO = ScriptableObject.CreateInstance<AttackStateF>();
        chaseStateSO = ScriptableObject.CreateInstance<ChaseStateF>();
        idleStateSO = ScriptableObject.CreateInstance<IdleStateF>();

        // Prefab'a StateF SO'larýný atamak için
        EnemyBaseF enemyBase = unit.GetComponent<EnemyBaseF>();
        if (enemyBase != null)
        {
            enemyBase.attackState = attackStateSO;
            enemyBase.chaseState = chaseStateSO;
            enemyBase.idleState = idleStateSO;
        }

        Debug.Log($"Runtime sýrasýnda {unit.name} için State SO'larý oluþturuldu ve atandý.");
    }

}
