using UnityEditor;
using UnityEngine;

public class EnemySOAutoAssignerF : EditorWindow
{
    [MenuItem("Tools/Assign States to Selected Enemies")]
    public static void AssignStates()
    {
        foreach (var obj in Selection.gameObjects)
        {
            EnemyBaseF enemy = obj.GetComponent<EnemyBaseF>();
            if (enemy == null)
            {
                Debug.LogWarning($"{obj.name} doesn't have EnemyBase component.");
                continue;
            }

            string path = "Assets/EnemyStatesGenerated/";

            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder("Assets", "EnemyStatesGenerated");

            // Create copies of SOs
            IdleStateF idle = ScriptableObject.CreateInstance<IdleStateF>();
            ChaseStateF chase = ScriptableObject.CreateInstance<ChaseStateF>();
            AttackStateF attack = ScriptableObject.CreateInstance<AttackStateF>();

            AssetDatabase.CreateAsset(idle, $"{path}{obj.name}_Idle.asset");
            AssetDatabase.CreateAsset(chase, $"{path}{obj.name}_Chase.asset");
            AssetDatabase.CreateAsset(attack, $"{path}{obj.name}_Attack.asset");

            AssetDatabase.SaveAssets();

            enemy.idleState = idle;
            enemy.chaseState = chase;
            enemy.attackState = attack;

            EditorUtility.SetDirty(enemy);
            Debug.Log($"State'ler {obj.name} için oluþturuldu ve atandý.");
        }
    }
}
