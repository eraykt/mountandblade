using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    public TwoBoneIKConstraint _twoBoneIK; // Two Bone IK Constraint bileşeni
    private Animator _animator;

    [SerializeField] private List<Image> _attackWays;

    private Vector2 _mouseDelta;
    private string _currentWay = "";
    public float _threshold;

    private Vector2 _accumulatedDelta = Vector2.zero;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Mouse delta değerini oku
            _mouseDelta = Mouse.current.delta.ReadValue() * 0.01f; // Çok küçük hareketleri küçültmek için çarpan
            if (_mouseDelta.magnitude < 0.05f) return; // Küçük hareketleri yok say
        
            _accumulatedDelta += _mouseDelta;

            Debug.Log($"Accumulated Delta: {_accumulatedDelta}");

            // Yön tayini
            if (Mathf.Abs(_accumulatedDelta.x) > Mathf.Abs(_accumulatedDelta.y))
            {
                if (_accumulatedDelta.x > _threshold) AttackWay("right");
                else if (_accumulatedDelta.x < -_threshold) AttackWay("left");
            }
            else
            {
                if (_accumulatedDelta.y > _threshold/3f) AttackWay("up");
                else if (_accumulatedDelta.y < -_threshold/3f) AttackWay("down");
            }

            // Birikimi yavaşça sıfırla (yumuşak sıfırlama)
            _accumulatedDelta = Vector2.Lerp(_accumulatedDelta, Vector2.zero, Time.deltaTime * 2);
            
            
            
            // IK ağırlığını artır (kol kalksın)
            DOVirtual.Float(_twoBoneIK.weight, 1f, 0.5f, val => _twoBoneIK.weight = val);
        }

        else if (Input.GetMouseButtonUp(0))
        {
            _accumulatedDelta = Vector2.zero;
            _animator.SetTrigger("attack");
            DOVirtual.Float(_twoBoneIK.weight, 0f, 0.5f, val => _twoBoneIK.weight = val);
        }

           
    }

    private void AttackWay(string way)
    {
        if (_currentWay.Equals(way)) return;
        _attackWays.ForEach(x => x.gameObject.SetActive(false));
        _currentWay = way;

        switch (way)
        {
            case "right":
                _attackWays[1].gameObject.SetActive(true);

                break;

            case "left":
                _attackWays[0].gameObject.SetActive(true);

                break;
            case "up":
                _attackWays[2].gameObject.SetActive(true);

                break;
            case "down":
                _attackWays[3].gameObject.SetActive(true);

                break;
        }
    }

    public void Hit()
    {
        Debug.Log("Hit animation event triggered!");
    }
}