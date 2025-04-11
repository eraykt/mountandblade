using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Camera cam;
    public NavMeshAgent agent;

    private Animator playerAnimator;
    
    private bool canMove = true;
    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {

        Debug.Log(agent.velocity.magnitude);
        if (!canMove)
            return;

        if (playerAnimator != null)
        {
            HandleAnimation();
        } 

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.CompareTag("Unclickable"))
                    return;

                agent.SetDestination(hit.point);

            }
            
        }

    }
    
    private void HandleAnimation()
    {
        float clampedSpeed = Mathf.Clamp01(agent.velocity.magnitude);
        playerAnimator.SetFloat("CurrentSpeed", clampedSpeed);
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public float GetSpeed() => agent.speed;
    public bool GetIsStopped() => agent.isStopped;

}
