using MountAndBlade;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public PlayerManager playerManager;
    
    public Camera cam;

    public NavMeshAgent agent;
    private bool canMove = true;
    private float defaultAgentSpeed;
    [SerializeField]private float SlowMultipler;

    private Animator playerAnimator;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        defaultAgentSpeed = agent.speed;
        UptadeSpeedRelativeToUnitAmount();
        playerAnimator = GetComponentInChildren<Animator>();

        if (InterSceneManager.Instance.hasPlayerData)
        {
            transform.position = InterSceneManager.Instance.playerData.playerPosition;
            playerManager.playerSoldierAmount = InterSceneManager.Instance.playerData.troopCount;
            playerManager.UpdateSoldierCountText();
        }
    }

    private void Update()
    {
        if (!canMove)
            return;

        if (playerAnimator != null)
        {
            HandleAnimation();
        }

        CastAgentDestinationRay();
    }

    private void CastAgentDestinationRay() {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.gameObject.CompareTag("Unclickable"))
                    return;

                agent.SetDestination(hit.point);

            }
        }
    }
    private void UptadeSpeedRelativeToUnitAmount()
    {
        float SlowAmount = playerManager.playerSoldierAmount * SlowMultipler;
        agent.speed = defaultAgentSpeed / SlowAmount;
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
