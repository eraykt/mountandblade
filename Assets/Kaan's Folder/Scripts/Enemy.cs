using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform playerTransform;
    public float moveSpeed = 5f;
    private Vector3 moveDir;
    public Rigidbody enemyRb;
    private IState currentState;
    public bool isAttack = false;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        currentState = new ChaseState();
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);

    }

    public void ChangeState(IState state)
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isAttack = true;
        }
    }
}
