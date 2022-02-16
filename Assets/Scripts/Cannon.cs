using System;
using SquareDino;
using UnityEngine;
using Random = UnityEngine.Random;
public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform _cannonBody;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _clickDelay;
    [SerializeField] private float _rayDistance;
    [SerializeField] private Transform _rayCastPoint;
    [SerializeField] private WorldText _text;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private PlayerUnits _unitsSettings;
    [SerializeField] private Transform _shootTransform;
    [SerializeField] private int _upScaleSpeed;
    [SerializeField] private int _downScaleSpeed;
    [SerializeField] private Vector3 _scale;
    [SerializeField] private Vector3 _nonActiveScale;
    [SerializeField] private Vector3 _moveAnimation;
    [SerializeField] private int _moveSpeedAnimation;
    [SerializeField] private int _moveSpeed;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private Transform _plate;
    [SerializeField] private float _adaptivePlatePosition;
    [SerializeField] private float _normalPlatePostition;
    private Coroutine _moveCoroutine;
    private Coroutine _scaleCoroutine;
    private Timer _shootTimer;
    private Timer _clickTimer;
    private float _progressBarCurrentLenght;
    private UserInput _userInput;
    private UnitSpawner _unitSpawner;
    private OutlineColorSetter _outlineColorSetter;
    private UnitStats _nextUnit;
    private Vector3 _bodyStartPosition;
    private int _activeLineIndex=-1;
    private void Start()
    {
        _progressBar.SetParameters(_reloadTime,_reloadTime);
        _outlineColorSetter = GetComponentInChildren<OutlineColorSetter>();
        _userInput = FindObjectOfType<UserInput>();
        _unitSpawner = GetComponent<UnitSpawner>();
        _shootTimer = new Timer();
        _clickTimer = new Timer();
        _userInput.OnTouchUp +=MoveToLine;
        //_levelManager.OnLevelLoaded += MoveToLine;
        _nextUnit = new UnitStats();
        CalculateNextUnit();
        _bodyStartPosition=_cannonBody.localPosition;
    }
    private void UpdateProgressBar()
    {
        _progressBarCurrentLenght += Time.deltaTime;
        _progressBar.SetProgress(_progressBarCurrentLenght);
    }
    private void Update()
    {
        if(_shootTimer.IsTick) UpdateProgressBar();
        _shootTimer.UpdateTimer();
        _clickTimer.UpdateTimer();
    }
    private void CheckPlatePosition()
    {
        var position = _plate.localPosition;
        position.x = _unitSpawner.LinesCollection.LineCount > 3&&_activeLineIndex==0 ? _adaptivePlatePosition : _normalPlatePostition;
        _plate.localPosition = position;
    }
    private void MoveToLine()
    {
        if (_clickTimer.IsTick) return;
        _clickTimer.StartTimer(_clickDelay, null);
        //if (_moveCoroutine != null) return;
        var line = _unitSpawner.LinesCollection.GetClosestLine(_userInput.GetTouchOnPlane(_cannonBody.position));
        if (_activeLineIndex == _unitSpawner.LinesCollection.ActiveLineIndex)
        {
            Shoot();
            return;
        }
        StopAllCoroutines();
        var position = _cannonBody.position;
        //_outlineColorSetter.TrySetLineColor(line);
        position.x=line.GetSpawnPoint(false).x;
        _moveCoroutine=StartCoroutine(CorroutinesKid.Move(_cannonBody.transform, position, _moveSpeed, ()=>
        {
            _outlineColorSetter.TrySetLineColor(line);
            _moveCoroutine = null;
            CheckPlatePosition();
            Shoot();
        }));
        _activeLineIndex = _unitSpawner.LinesCollection.ActiveLineIndex;
    }
    private void Shoot()
    {
        _cannonBody.localScale=Vector3.one*100;
        if (_shootTimer.IsTick || CheckCloseUnit() || _moveCoroutine != null)
        {
            StartCoroutine(CorroutinesKid.ScaleAnimation(_cannonBody.transform,_nonActiveScale,_upScaleSpeed,_downScaleSpeed,null));
            return;
        }
        ParticleManager.Instance.PlayParticle(ParticleTypes.Shoot, _shootTransform.position, Vector3.zero,
            Vector3.one * 2, null);
        var localPosition = _cannonBody.localPosition;
        localPosition.y = _bodyStartPosition.y;
        _cannonBody.localPosition = localPosition;
        localPosition.y = _moveAnimation.y;
        StartCoroutine(CorroutinesKid.ScaleAnimation(_cannonBody.transform,_scale,_upScaleSpeed,_downScaleSpeed,null));
        StartCoroutine(CorroutinesKid.MoveLocalAndBack(_cannonBody.transform,localPosition,_moveSpeedAnimation, null));
        _progressBarCurrentLenght = 0;
        MyVibration.Haptic(MyHapticTypes.LightImpact);
        SoundManager.Instance.PlaySound(SoundTypes.CannonShoot);
        _shootTimer.StartTimer(_reloadTime,null);
        _nextUnit.LineIndex = _activeLineIndex;
        var unit = _unitSpawner.SpawnUnit(_nextUnit);
        unit.OnCameToEnd += () =>
        {
            _unitSpawner.LinesCollection.GetLine(unit.LineIndex).SetOutLineColor(Color.blue);
        };
        CalculateNextUnit();
    }
    private void CalculateNextUnit()
    {
        _nextUnit.UnitLevel = _unitsSettings.GetRandomLevel(Statistics.CurrentLevelNumber + 1);
        _text.UpdateText(Math.Pow(2, _nextUnit.UnitLevel).ToString());
    }
    private bool CheckCloseUnit()
    {
        return Physics.Raycast(_rayCastPoint.position,_rayCastPoint.forward,_rayDistance);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawLine(_rayCastPoint.position,_rayCastPoint.position+_rayCastPoint.forward*_rayDistance);
    }
}
