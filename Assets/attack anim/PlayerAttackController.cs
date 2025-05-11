using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using DG.Tweening;
using MountAndBlade;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAttackController : MonoBehaviour, IDamagable
{
    public CinemachineVirtualCamera virtualCamera;
    
    public TwoBoneIKConstraint _twoBoneIK;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _hint;
    
    private Animator _animator;
    
    [SerializeField] private List<AttackWayStruct> _attackWays ;

    private Vector2 _mouseDelta;
    private AttackWay _currentWay = MountAndBlade.AttackWay.Right;
    public float _threshold;

    private Vector2 _accumulatedDelta = Vector2.zero;
    public SwordController swordController;

    public bool isAttacking;
    public float attackingTimer = 0.5f;
    private Vector3 _shakeDirection = Vector3.right;


    public int currentHealth;
    public int MAX_HEALTH = 100;
    public bool isDead = false;
    public Transform deadCamTransform;

    private void Awake()
    {
        EventManager.RegisterEvent<EventManager.OnSwordChange>(OnSwordChange);
    }


    void Start()
    {
        _animator = GetComponent<Animator>();
        currentHealth = MAX_HEALTH;
    }

    void Update()
    {
        // if (isAttacking) return;

        if (isDead || _animator.GetBool("IsVictory")) return;
        
        if (Input.GetMouseButton(0))
        {
            // Mouse delta değerini oku
            _mouseDelta = Mouse.current.delta.ReadValue() * 0.01f; // Çok küçük hareketleri küçültmek için çarpan
            if (_mouseDelta.magnitude < 0.05f) return; // Küçük hareketleri yok say
        
            _accumulatedDelta += _mouseDelta;
            
            // Yön tayini
            if (Mathf.Abs(_accumulatedDelta.x) > Mathf.Abs(_accumulatedDelta.y))
            {
                if (_accumulatedDelta.x > _threshold) AttackWay(MountAndBlade.AttackWay.Right);
                else if (_accumulatedDelta.x < -_threshold) AttackWay(MountAndBlade.AttackWay.Left);
            }
            else
            {
                if (_accumulatedDelta.y > _threshold/3f) AttackWay(MountAndBlade.AttackWay.Up);
                else if (_accumulatedDelta.y < -_threshold/3f) AttackWay(MountAndBlade.AttackWay.Down);
            }

            // Birikimi yavaşça sıfırla (yumuşak sıfırlama)
            _accumulatedDelta = Vector2.Lerp(_accumulatedDelta, Vector2.zero, Time.deltaTime * 2);
            
        }

        else if (Input.GetMouseButtonUp(0))
        {
            isAttacking = true;
            _accumulatedDelta = Vector2.zero;
            _animator.SetTrigger(_currentWay.ToString());
            _attackWays.ForEach(x => x._indicator.gameObject.SetActive(false));
            DOVirtual.Float(_twoBoneIK.weight, 0f, 0.5f, val => _twoBoneIK.weight = val);
        }

           
    }

    private void AttackWay(MountAndBlade.AttackWay way)
    {
        if (_currentWay.Equals(way)) return;
        // if (way.Equals(""))
        // {
        //     _currentWay = "";
        //     return;
        // }
        
        _attackWays.ForEach(x => x._indicator.gameObject.SetActive(false));
        _currentWay = way;
        var newAttackWay = _attackWays.Find(x => x._way.Equals(way));
        newAttackWay._indicator.gameObject.SetActive(true);
        _shakeDirection = newAttackWay._shakeDirection;
        
        
        _target.DOLocalMove(newAttackWay._target.localPosition, 0.5f);
        _target.DOLocalRotateQuaternion(newAttackWay._target.localRotation, 0.5f);
        _hint.DOLocalMove(newAttackWay._hint.localPosition, 0.5f);
        _hint.DOLocalRotateQuaternion(newAttackWay._hint.localRotation, 0.5f);
        
        
        DOVirtual.Float(_twoBoneIK.weight, 1f, 0.5f, val => _twoBoneIK.weight = val);

    }

    public void Hit()
    {
        Debug.Log("sword can attack");
        swordController.canAttack = true;
        swordController.virtualCamera.m_DefaultVelocity = _shakeDirection;
    }

    public void ExitHit()
    {
        swordController.canAttack = false;
        isAttacking = false;
    }
    
    private void OnSwordChange(EventManager.OnSwordChange obj)
    {
        swordController = obj.sword;
    }

    public void TakeDamage(int _takenDamage)
    {
        if (currentHealth >= _takenDamage)
        {
            currentHealth -= _takenDamage;

            if (currentHealth <= 0)
            {
                Die();

            }
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        _animator.SetBool("IsDead", true);

        GameManagerF.Instance.Unregister(gameObject);

        // Cinemachine Virtual Camera'yı deadCamTransform'a taşımak
        virtualCamera.transform.DOMove(deadCamTransform.position, 1.5f).SetEase(Ease.InOutSine);
        virtualCamera.transform.DORotateQuaternion(deadCamTransform.rotation, 1.5f).SetEase(Ease.InOutSine);
    }
}

[System.Serializable]
public struct AttackWayStruct
{
    public AttackWay _way;
    public Transform _target;
    public Transform _hint;
    public Image _indicator;
    public Vector3 _shakeDirection;
}
