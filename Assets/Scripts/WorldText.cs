using TMPro;
using UnityEngine;
public class WorldText : MonoBehaviour
{
    [SerializeField] private TMP_Text _textMesh;
    public void UpdateText(string text)
    {
        _textMesh.text = text;
    }
}
