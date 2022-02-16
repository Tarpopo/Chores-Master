using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Lines : MonoBehaviour
{
   [SerializeField] private List<Line> _lines;
   public int ActiveLineIndex { get; private set;}
   public int LineCount => _lines.Count;
   public Line GetLine(int index) => _lines[index];
   public IEnumerable<int> GetThreeCenterIndex(int lineIndex) => _lines.GetThreeCenterIndex(lineIndex);
   public Vector3 GetClosestLinePosition(Vector3 position)
   { 
      return _lines.Select(item => item.GetSpawnPoint(false)).GetClosest(position);
   }
   public Line GetClosestLine(Vector3 position)
   {
      var distance = Mathf.Infinity;
      var Closeline = _lines[0];
      foreach (var line in _lines)
      {
         var dist = Vector3.Distance(position, line.GetSpawnPoint(false));
         if (dist > distance) continue;
         distance = dist;
         Closeline = line;
      }
      ActiveLineIndex = _lines.IndexOf(Closeline);
      return Closeline;
   }
   public void ScaleLine()
   {
      //StartCoroutine(CorroutinesKid);
   }
}
