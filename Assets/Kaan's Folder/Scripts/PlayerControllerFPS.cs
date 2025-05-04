using MountAndBlade;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerFPS : MonoBehaviour, IDamagable
{
    private Animator animator;
    private GameObject enemyObject;
    private Transform enemy;
    public LayerMask enemyLayers;

    public float moveSpeed = 5f; // Hareket hýzý
    public float jumpForce = 5f; // Zýplama gücü
    public float gravityScale = 1f; // Yerçekimi ölçeði
    public bool isGrounded; // Karakterin yerde olup olmadýðýný kontrol etmek için

    public int health = 1000000;
    public int damage = 2;
    public Transform hitPoint;
    public float hitRange = 0.2f;
    public bool isAttack = false;
    public bool isAnimPlaying = false;


    private Rigidbody rb;


    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        if(enemyObject  != null )
            enemy = enemyObject.transform;
    }

    void Update()
    {
        enemyObject = GameObject.FindWithTag("Enemy");
        if( enemyObject != null)
        {
            MovementHandler();
            JumpHandler();
            if (Input.GetMouseButtonDown(1)) AttackHandler();
        }
        else
        {
            // KAZANMA FONKSIYONLARI
            Debug.Log("KAZANDIN AMK NE BEKLIYON CIK");
        }
        //Debug.Log($"Player Health : {health}");
    }

    void MovementHandler()
    {
        // Hareket
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        transform.LookAt(enemyObject.transform);
        Vector3 movement = new Vector3(moveX, 0, moveZ).normalized * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    //    rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }

    void JumpHandler()
    {
        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            }

            // Yerçekimi
            rb.velocity += Vector3.up * Physics.gravity.y * gravityScale * Time.deltaTime;
        }
    }


    void AttackHandler()
    {
        if (!isAnimPlaying && enemyObject)
        {
            Debug.Log("Attack Handler Working");
            StartCoroutine(AttackAnimHandler());
            isAttack = true;
            EnemyHit();
        }
    }
    IEnumerator AttackAnimHandler()
    {
        //animator.SetBool("isAttacking", true);
        isAnimPlaying = true;
        yield return new WaitForSeconds(0.3f);
       // animator.SetBool("isAttacking", false);
        isAnimPlaying = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<IDamagable>() != null)
        {
            Debug.Log(3131313);
        }
    }

    //public void EnemyHit()
    //{
    //    Collider[] hitColliders = Physics.OverlapSphere(hitPoint.position, hitRange);

    //    foreach (Collider collider in hitColliders)
    //    {
    //        Enemy enemy = collider.GetComponent<Enemy>();
    //        if (enemy != null && isAttack && enemy.GetComponent<IDamagable>() != null)
    //        {
    //            enemy.TakeDamage(damage);
    //            isAttack = false;
    //            Debug.Log($"{enemy.name} has take damage by {gameObject.name}");
    //            Debug.Log($"{gameObject.name}'s health = {health}");
    //        }
    //    }
    //}

    public void EnemyHit() // => Hit fonksiyonu çalýþýnca burasý çalýþacal
    {
        Collider[] hitColliders = Physics.OverlapSphere(hitPoint.position, hitRange, enemyLayers);

        foreach (Collider collider in hitColliders) // TO-DO : Make changes for SOLDIRES 
        {
            IDamagable obj = collider.gameObject.GetComponent<IDamagable>();
            if (obj != null)
            {
                obj.TakeDamage(damage);
                isAttack = false;
                Debug.Log($"{obj} has take damage by {gameObject.name}");
                Debug.Log($"{gameObject.name}'s health = {health}");
            }
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(hitPoint.position, hitRange);
    }
}