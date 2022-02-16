using UnityEngine;
using TMPro;
using System.Collections;

namespace UI
{
    public class LosingWindow : CanvasGroupBased
    {
        [Space]
        [SerializeField] private MyButton restartButton;
        [SerializeField] private TextMeshProUGUI fadedText;

        public MyButton RestartButton => restartButton;
    }
}