using UnityEngine;
[RequireComponent(typeof(OutLIne))]
public class Line : MonoBehaviour
{
    [SerializeField] private Transform _spawnPointUp;
    [SerializeField] private Transform _spawnPointDown;
    //[SerializeField] private Color _lineActiveColor;
    private OutLIne _outline;
    private void Awake()
    {
        _outline = GetComponent<OutLIne>();
    }
    public void SetActiveOutLineColor(bool isActive)
    {
        _outline.SetActiveOutLine(isActive);
    }
    public void SetOutLineColor(Color color)
    {
        _outline.SetOutlineColor(color);
    }
    public Vector3 GetSpawnPoint(bool isEnemy)
    {
        return isEnemy ? _spawnPointUp.position : _spawnPointDown.position;
    }
    public Transform GetTargetTransform(bool isEnemy)
    {
        return isEnemy ? _spawnPointDown : _spawnPointUp;
    }
}
