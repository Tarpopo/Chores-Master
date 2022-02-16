using System.Collections.Generic;
using UnityEngine;
public class HealthUI : MonoBehaviour
{
    [SerializeField] private List<HealthCell> _cells;
    private Stack<HealthCell> _activeCells = new Stack<HealthCell>(11);
    public void SetHealthCell(int health)
    {
        ClearCells();
        for (int i = 0; i < health; i++)
        {
            _cells[i].gameObject.SetActive(true);
            _cells[i].SetActiveCell(true);
            _activeCells.Push(_cells[i]);
        }
    }
    private void ClearCells()
    {
        _activeCells.Clear();
        foreach (var cell in _cells)
        {
            cell.gameObject.SetActive(false);
        }
    }
    public void ReduceOneCell()
    {
        if (_activeCells.Count == 0) return;
        _activeCells.Pop().SetActiveCell(false);
    }
}
