using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MountAndBlade
{
    public interface IMoveable
    {
        Rigidbody rb { get; set; }
        NavMeshAgent agent { get; set; }

        void MoveEnemy(Vector3 pos);

    }
}
