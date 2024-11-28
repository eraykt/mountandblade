using UnityEngine;
using UnityEngine.Animations.Rigging;
using DG.Tweening;

public class AttackController : MonoBehaviour
{
    public TwoBoneIKConstraint twoBoneIK;    // Two Bone IK Constraint bileşeni
    public Transform target;                 // Elin kalkacağı hedef pozisyon
    public Transform curvePoint;             // Kavisin ara noktası
    public Transform endPoint;               // Nihai hedef pozisyon

    private Quaternion startRotation;

    private Vector3[] pathPoints;            // Yol noktaları
    
    private Animator _animator;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        
        startRotation = target.rotation;
        // Kavis yolunu belirle (başlangıç, kavis ve nihai hedef)
        pathPoints = new Vector3[] 
        {
            target.localPosition,       // Başlangıç pozisyonu
            curvePoint.localPosition,   // Ara nokta (kavis)
            endPoint.localPosition      // Nihai hedef
        };
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target.localPosition = pathPoints[0];
            target.localRotation = startRotation;

            // IK ağırlığını artır (kol kalksın)
            DOVirtual.Float(twoBoneIK.weight, 1f, 0.5f, val => twoBoneIK.weight = val);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _animator.SetTrigger("attack");
            DOVirtual.Float(twoBoneIK.weight, 0f, 0.5f, val => twoBoneIK.weight = val);

            // // IK ağırlığını azaltırken kavisli hareket yap
            // twoBoneIK.weight = 1f; // Kol hedefteyken hareket etmeli
            //
            // var seq = DOTween.Sequence();
            //
            // // İlk olarak pozisyonu kavisli yolda hareket ettir
            // seq.Append(target.DOLocalPath(pathPoints, curveDuration, PathType.CatmullRom)
            //     .SetEase(Ease.InOutSine) // Kavisli hareket için yumuşak geçiş
            //     .SetOptions(true));
            //
            // // Rotasyonu kavisli hareketle birlikte senkronize et
            // seq.Join(target.DOLocalRotateQuaternion(endPoint.rotation, curveDuration)
            //     .SetEase(Ease.InOutSine)); // Rotasyonu da yumuşak bir şekilde gerçekleştir
            //
            // // Sonraki hareketi başlat
            // seq.Append(DOVirtual.Float(twoBoneIK.weight, 0f, weightSpeed, val => twoBoneIK.weight = val)).Play();
        }
    }
    public void Hit()
    {
        Debug.Log("Hit animation event triggered!");
        // Add your hit logic here
    }

}
