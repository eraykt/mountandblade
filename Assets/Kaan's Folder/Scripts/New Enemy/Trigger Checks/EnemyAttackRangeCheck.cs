using MountAndBlade;
using UnityEngine;

public class EnemyAttackRangeCheck : MonoBehaviour
{
    private EnemyBase _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<EnemyBase>();
    }

    // Animasyon event'i tarafýndan çaðrýlacak
    public void EndAttack()
    {
        if (_enemy != null)
        {
            // Saldýrý bittiðini bildir
            _enemy.OnAttackFinished();

            // Hasar ver (eðer target varsa)
            if (_enemy.target != null && _enemy.targetScript != null)
            {
                _enemy.targetScript.Damage(10);
            }
        }
    }
}