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

    private bool canMove = true;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {

        if (!canMove)
            return;

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

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public float GetSpeed() => agent.speed;
    public bool GetIsStopped() => agent.isStopped;

}
