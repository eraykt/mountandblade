using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public class EnemyTD : MonoBehaviour, IMoveable
    {
        public NavMeshAgent agent{ get; set; }
        public GameObject player { get; set; }//Eraya bi sorarýz get set yazmak sadece ne iþe yarýyor.

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            agent = GetComponent<NavMeshAgent>();
        }
        public void MoveToDestination(Vector3 position)
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
