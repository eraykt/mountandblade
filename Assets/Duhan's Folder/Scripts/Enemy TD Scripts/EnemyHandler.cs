using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class EnemyHandler : MonoBehaviour
    {
        private PlayerManager playerManager;
        private StateMachine stateMachine;
        private NavMeshAgent agent;
        public Transform player;
        public int askerSayisi;
        public TMP_Text soldierCountText;
        private bool isPlayerStronger;

        [Header("Bounds")]
        public Vector3 boundsMin;
        public Vector3 boundsMax;

        public float chaseRange = 10f;
        public float stopRange = 15f;
        public float maxDistance = 50f;
        public float retreatSpeed = 8f;

        public bool isInReach;

        void Start()
        {
            playerManager = FindObjectOfType<PlayerManager>();
            agent = GetComponent<NavMeshAgent>();
            stateMachine = new StateMachine();

            UpdateStrengthStatus();
            UpdateSoldierCountText();

            stateMachine.ChangeState(new PatrolState(agent, transform, boundsMin, boundsMax));
        }

        void Update()
        {
            stateMachine.Update();
            CheckDistance();
        }

        private void UpdateStrengthStatus()
        {
            if (playerManager != null)
            {
                isPlayerStronger = playerManager.playerSoldierAmount <= askerSayisi;
            }
        }

        public void UpdateSoldierCountText()
        {
            if (soldierCountText != null)
            {
                soldierCountText.text = askerSayisi.ToString();
            }
        }

        private void CheckDistance()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseRange && !isInReach)
                isInReach = true;
            else if (distanceToPlayer > stopRange && isInReach)
                isInReach = false;

            if (distanceToPlayer > maxDistance)
                stateMachine.ChangeState(new PatrolState(agent, transform, boundsMin, boundsMax));
            else if (isInReach && isPlayerStronger)
                stateMachine.ChangeState(new ChaseState(agent, player));
            else if (isInReach && !isPlayerStronger)
                stateMachine.ChangeState(new RetreatState(agent, player, retreatSpeed));
        }
    }
}