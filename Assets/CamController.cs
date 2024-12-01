using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
public class CamController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 20f;
    [SerializeField]
    private CinemachineFreeLook freelookCam;
    [SerializeField]
    private float rotationSpeed = 100f;
    private bool canMove;
    private void Update()
    {
        MovementHandler();
        CloseDistanceMovementFixer();
    }

    private void MovementHandler()
    {
        if (canMove)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDir = new Vector3(vertical, 0, horizontal).normalized;

            transform.Translate(moveDir * moveSpeed * Time.deltaTime);

            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime);
        }
    }

    private void CloseDistanceMovementFixer()
    {
        canMove = freelookCam.m_YAxis.Value > 0.5f ? true : false;
        if (!canMove)
        {
            transform.Translate(Vector3.zero);
        }
    }
}
