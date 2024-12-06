using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    public TwoBoneIKConstraint _twoBoneIK;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _hint;
    
    private Animator _animator;
    
    [SerializeField] private List<AttackWay> _attackWays ;

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
            
        }

        else if (Input.GetMouseButtonUp(0))
        {
            _accumulatedDelta = Vector2.zero;
            _animator.SetTrigger(_currentWay == "" ? "right" : _currentWay);
            _attackWays.ForEach(x => x._indicator.gameObject.SetActive(false));
            DOVirtual.Float(_twoBoneIK.weight, 0f, 0.5f, val => _twoBoneIK.weight = val);
        }

           
    }

    private void AttackWay(string way)
    {
        if (_currentWay.Equals(way)) return;
        if (way.Equals(""))
        {
            _currentWay = "";
            return;
        }
        
        _attackWays.ForEach(x => x._indicator.gameObject.SetActive(false));
        _currentWay = way;
        var newAttackWay = _attackWays.Find(x => x._name.Equals(way));
        newAttackWay._indicator.gameObject.SetActive(true);
        
        _target.DOLocalMove(newAttackWay._target.localPosition, 0.5f);
        _target.DOLocalRotateQuaternion(newAttackWay._target.localRotation, 0.5f);
        _hint.DOLocalMove(newAttackWay._hint.localPosition, 0.5f);
        _hint.DOLocalRotateQuaternion(newAttackWay._hint.localRotation, 0.5f);
        
        
        DOVirtual.Float(_twoBoneIK.weight, 1f, 0.5f, val => _twoBoneIK.weight = val);

    }

    public void Hit()
    {
        Debug.Log("Hit animation event triggered!");
    }
}

[System.Serializable]
public class AttackWay
{
    public string _name;
    public Transform _target;
    public Transform _hint;
    public Image _indicator;
}