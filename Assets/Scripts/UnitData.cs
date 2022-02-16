using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class UnitData : MonoBehaviour
{
    [SerializeField] private UnityEvent<UnitData> _onUnitStand;
    [SerializeField] private Material _material;
    public event UnityAction<UnitData> OnUnitStand
    {
        add => _onUnitStand.AddListener(value);
        remove => _onUnitStand.RemoveListener(value);
    }
    [SerializeField] private UnityEvent _onCameToEnd;
    public event UnityAction OnCameToEnd
    {
        add => _onCameToEnd.AddListener(value);
        remove => _onCameToEnd.RemoveListener(value);
    }
    [SerializeField] private UnityEvent _onAlmostCameToEnd;
    public event UnityAction OnAlmostCameToEnd
    {
        add => _onAlmostCameToEnd.AddListener(value);
        remove => _onAlmostCameToEnd.RemoveListener(value);
    }
    [SerializeField] private AnimationClip _winClip;
    [SerializeField] private AnimationClip _loseClip;
    public int LineIndex { get; private set; }
    public float DistanceToUnit => _distanceToUnit;
    public Vector3 TargetPosition => _targetTransform.position;
    public Transform TargetTransform => _targetTransform;
    public bool IsMerging;
    public Vector3 UnitPosition => transform.position;
    [SerializeField] private Transform _raycastTransform;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _distanceToUnit;
    [SerializeField] private float _distanceToAlmost;
    [SerializeField] private float _rayDistance;
    [SerializeField] private int _framesScale;
    public float DistanceToAlmost=>_distanceToAlmost;
    public UnitLevel LevelUnit => _unitLevel;
    private UnitLevel _unitLevel;
    private UnitHealth _unitHealth;
    private UnitSkinSetter _unitSkinSetter;
    private Transform _targetTransform;
    [SerializeField]private Animator _animator;
    private bool _isMain;
    public bool IsMain => _isMain;
    public AllUnitAnimation Animations; 
    private RaycastHit _enemyHit;
    private Fighting _fighting;
    private void Awake()
    {
        _unitHealth = GetComponent<UnitHealth>();
        _unitLevel = GetComponent<UnitLevel>();
        _unitSkinSetter = GetComponent<UnitSkinSetter>();
        _unitLevel.OnStart();
        _unitHealth.OnHealthEnd += _unitLevel.ReduceLevel;
        _unitLevel.OnLevelUp +=()=>_unitHealth.SetHealth(_unitLevel.Level);
    }
    private void OnEnable()
    {
        _unitLevel.OnLevelReduce += (context) =>_unitHealth.SetHealth(_unitLevel.Level);
        _unitLevel.OnLevelEnd+=(context)=>
        {
            ParticleManager.Instance.PlayParticle(ParticleTypes.Death, transform.position, Vector3.zero,
                    Vector3.one * 2, null);
        };
        _unitLevel.OnLevelReduce += (context) =>
        {
            _unitSkinSetter.ChangeSkin(_unitLevel.Level);
            Animations = _unitSkinSetter.SkinSettings.Animations;
            _unitSkinSetter.CurrentSkin.GetComponentInChildren<SkinnedMeshRenderer>().material = _material;
            _fighting=_unitSkinSetter.CurrentSkin.GetComponent<Fighting>();
            _animator=_unitSkinSetter.CurrentSkin.GetComponentInChildren<Animator>();
            ChangeScale();
        };
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        _isMain = false;
        transform.localScale = Vector3.zero;
        _targetTransform = null;
        IsMerging = false;
        _onUnitStand.RemoveAllListeners();
        _onCameToEnd.RemoveAllListeners();
        _onAlmostCameToEnd.RemoveAllListeners();
    }
    public void CameToEnd()
    {
        PlayWinOrLoseAnimation(true);
        _onCameToEnd?.Invoke();
    }
    public void OnCameAlmost()
    {
        _onAlmostCameToEnd?.Invoke();
    }
    public void PlayAnimation(UnitAnimations animation)=>_animator.Play(animation.ToString());
    public void PlayWinOrLoseAnimation(bool isWin)=>_animator.Play(isWin ? _winClip.name : _loseClip.name);
    
    public void OnUnitWait()
    {
        if (_isMain) return;
        _onUnitStand?.Invoke(this);
    }
    public void SetUnitMain()
    {
        _isMain = true;
    }
    private void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    public bool TryFindEnemy()
    {
        if (_isMain == false) return false;
        return Physics.Raycast(_raycastTransform.position,_raycastTransform.forward,out _enemyHit,_rayDistance);
    }
    private IEnumerator ChangeUnitScale(Transform currentScale, Vector3 targetScale, int frames,Action onEndMove)
    {
        var delta = (targetScale - currentScale.localScale)/frames;
        for (int i = 0; i < frames; i++)
        {
            currentScale.localScale += delta;
            yield return Waiters.fixedUpdate;
        }
        onEndMove?.Invoke();
    }
    private Vector3 CalculateScale()
    {
        var scaleCount=1+_unitLevel.Level*0.05f;
        return Vector3.one*scaleCount;
    }
    public void AttackEnemy()
    {
        _fighting.Enemyhit = _enemyHit;
    }

    public void ChangeScale()
    {
        if (_unitLevel.IsHaveLevel == false||transform.gameObject.activeSelf==false) return;
        StartCoroutine(ChangeUnitScale(transform,CalculateScale(),_framesScale,null));
    }
    public void MoveToTarget()
    {
        SetPosition(Vector3.MoveTowards(UnitPosition, TargetPosition, _moveSpeed));
    }
    public void SetTargetTransform(Transform targetTransform)
    {
        _targetTransform = targetTransform;
    }
    public void SetParameters(int lineIndex,int unitLevel,bool isMain,Transform targetTransform)
    {
        _isMain = isMain;
        LineIndex = lineIndex;
        _targetTransform = targetTransform;
        _unitHealth.SetHealth(unitLevel);
        _unitLevel.SetUnitLevel(unitLevel);
        ChangeScale();
        //SetLookRotation();
        _unitSkinSetter.ChangeSkin(_unitLevel.Level);
        Animations = _unitSkinSetter.SkinSettings.Animations;
        _unitSkinSetter.CurrentSkin.GetComponentInChildren<SkinnedMeshRenderer>().material = _material;
        _fighting=_unitSkinSetter.CurrentSkin.GetComponent<Fighting>();
        _animator=_unitSkinSetter.CurrentSkin.GetComponentInChildren<Animator>();
        GetComponent<Unit>().InitializeMove();
    }
}
