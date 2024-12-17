using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class EnemyTD : MonoBehaviour, IMoveable
    {
        public NavMeshAgent agent{ get; set; }
        public GameObject player { get; set; }//Eraya bi sorarýz get set yazmak sadece ne iþe yarýyor.
        public EnemyTDStateMachine stateMachine { get; set; }
        public EnemyTDPatrolState patrolState { get; set; }  
        //public EnemyTDMoveToDestinationState moveToDestState { get; set; }  
        //public EnemyTDGeneratePosState genRandPosState { get; set; }

        public Vector3 randPos;
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            agent = GetComponent<NavMeshAgent>();

            stateMachine = new EnemyTDStateMachine();
            patrolState = new EnemyTDPatrolState(this, stateMachine);
            //genRandPosState = new EnemyTDGeneratePosState(this,stateMachine);
            //moveToDestState = new EnemyTDMoveToDestinationState(this, stateMachine); // buradaki rand posu yazmadan nasýl kullanabilrim


        }
        
        

        // Start is called before the first frame update
        void Start()
        {
            stateMachine.InitializeState(patrolState);
            
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.currentState.FrameUpdate();
        }

        public void MoveToDestination(Vector3 position)
        {
            agent.SetDestination(position);
        }
    }
}
