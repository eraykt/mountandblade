using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class EnemyHandler : MonoBehaviour
    {
        public NavMeshAgent agent;
        public Transform player;
        private StateMachine stateMachine;

        public int askerSayisi;
        public TMP_Text soldierCountText;

        private void Start()
        {




            stateMachine = new StateMachine(this); // Bu, EnemyHandler'ý StateMachine yapýcýsýna geçirir
            playerManager = FindObjectOfType<PlayerManager>();
            if (playerManager == null)
            {
                Debug.LogError("PlayerHandler bulunamadý! Lütfen sahnede bir PlayerHandler olduðundan emin olun.");
                return;
            }
            UpdateStrengthStatus();
            UpdateSoldierCountText();
            enemyAgent = GetComponent<NavMeshAgent>();
            currentState = States.patrol;

            // StateMachine'e ilk state'i ata
            stateMachine.ChangeState(new PatrolState(agent, transform, boundsMin, boundsMax));
        }

        private void Update()
        {
            stateMachine.Tick();
            CheckDistance();
        }

        private void CheckDistance()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= 10f)  // Chase range
                stateMachine.ChangeState(States.chase);
            else if (distanceToPlayer > 15f)  // Stop range
                stateMachine.ChangeState(States.retreat);
        }

        public enum States { patrol, chase, retreat }
    }
}
