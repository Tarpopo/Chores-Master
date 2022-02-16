using UnityEngine;
using UnityEngine.EventSystems;
public class Tutorial : MonoBehaviour,IPointerClickHandler
{
    [Header("Analytics")] 
    [SerializeField] private int _tutorialId;
    [SerializeField] private bool _isStartedTutorial;
    [SerializeField] private bool _isEndTutorial;
    [Header("Tutorial")]
    [SerializeField] private GameObject _nextTutorial;
    [SerializeField] private float _nextTutorialDealy;
    private void Start()
    {
        if (_isStartedTutorial)AmplitudeHelper.Inctance.TutorialStarted();
        //AmplitudeHelper.Inctance.TutorialNStarted(_tutorialId);
    }
    private void PlayWithDelay()
    {
        _nextTutorial.SetActive(true);
        GameState.Instance.SetGameState(GameStates.Pause);
    }
    private void DeactivateTutorial()
    {
        GameState.Instance.SetGameState(GameStates.Active);
        //AmplitudeHelper.Inctance.TutorialNCompleted(_tutorialId);
        if (_isEndTutorial)
        {
            AmplitudeHelper.Inctance.TutorialCompleted();
            Statistics.TutorialIsCompleted = true;
        }
        gameObject.SetActive(false);
        if(_nextTutorial!=null)Invoke(nameof(PlayWithDelay),_nextTutorialDealy);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        DeactivateTutorial();
    }
}
