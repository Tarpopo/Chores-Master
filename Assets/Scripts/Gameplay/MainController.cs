using SquareDino;
using UnityEngine;
using SquareDino.Scripts.MyAds;
using SquareDino.Scripts.MyAnalytics;

public class MainController : MonoBehaviour
{
    [SerializeField] private SceneUI sceneUI;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private ParticleSystem fireworksParticleSystem;
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        sceneUI.GameWindow.RestartButton.OnClick+=()=>RestartGame(true);
        sceneUI.VictoryWindow.ContinueButton.OnClick += NextLevel;
        sceneUI.LosingWindow.RestartButton.OnClick += ()=>RestartGame(false);
        levelManager.OnLevelLoaded += LevelManager_OnLevelLoaded;
        levelManager.OnLevelCompleted += LevelManager_OnLevelCompleted;
        levelManager.OnLevelNotPassed += LevelManager_OnLevelNotPassed;
        StartGame();
    }
    private void StartGame()
    {
        levelManager.LoadLevel();
    }
    private void NextLevel()
    {
        MoneyHandler.SaveMoneyData();
        MyAdsManager.InterstitialShow(InterstitialClosed);
    }
    private void RestartGame(bool isAnalytics)
    {
        levelManager.CurrentLevel.OnProgressUpdated -= CurrentLevel_OnProgressUpdated;
        sceneUI.LosingWindow.Enable(false);
        SoundManager.Instance.PlaySound(SoundTypes.Drums);
        if (isAnalytics) MyAnalyticsManager.LevelRestart();
        MyAdsManager.InterstitialShow(InterstitialClosed);
    }
    private void InterstitialClosed()
    {    
        levelManager.LoadLevel();
    }
    private void LevelManager_OnLevelLoaded()
    {
        // fireworksParticleSystem.Stop();
        // fireworksParticleSystem.gameObject.SetActive(false);

        levelManager.CurrentLevel.OnProgressUpdated += CurrentLevel_OnProgressUpdated;

        sceneUI.VictoryWindow.Enable(false);
        sceneUI.GameWindow.Enable(true);
    }
    private void CurrentLevel_OnProgressUpdated(LevelProgress levelProgress)
    {
        sceneUI.GameWindow.UpdateProgressBar(levelProgress);
    }
    private void LevelManager_OnLevelCompleted()
    {
        levelManager.CurrentLevel.OnProgressUpdated -= CurrentLevel_OnProgressUpdated;

        // fireworksParticleSystem.gameObject.SetActive(true);
        // if (!fireworksParticleSystem.isPlaying) fireworksParticleSystem.Play();

        sceneUI.GameWindow.Enable(false);
        sceneUI.VictoryWindow.Enable(true);

        MyVibration.Haptic(MyHapticTypes.Selection);
    }
    private void LevelManager_OnLevelNotPassed()
    {
        levelManager.CurrentLevel.OnProgressUpdated -= CurrentLevel_OnProgressUpdated;

        sceneUI.GameWindow.Enable(false);
        sceneUI.LosingWindow.Enable(true);

        MyVibration.Haptic(MyHapticTypes.Selection);
    }
}