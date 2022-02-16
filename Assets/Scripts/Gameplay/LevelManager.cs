using System.Collections.Generic;
using Sirenix.OdinInspector;
using SquareDino.Scripts.MyAnalytics;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    public event System.Action OnLevelCompleted;
    public event System.Action OnLevelNotPassed;
    public event System.Action OnLevelLoaded;
    [Header("Settings")]
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [SerializeField] private LevelContainer levelContainer;
    [SerializeField] private Transform levelParent;
    [SerializeField, Sirenix.OdinInspector.ReadOnly] private int currentLevelNumber;
    //private readonly RandomNoRepeate randomLevelNumber = new RandomNoRepeate();
    public Level CurrentLevel { get; private set; }
    public List<Level> Levels
    {
        get { return levelContainer.Levels; }
    }
    // public void ResetLevel()
    // {
    //     LoadLevel(Statistics.CurrentLevelNumber);
    // }
    public void LoadLevel()
    {
        UnloadLevel();
        currentLevelNumber = Statistics.CurrentLevelNumber;

        if (levelContainer.IsDebug)
        {
            CurrentLevel = Instantiate(levelContainer.DebugLevel, levelParent);
        }
        else
        {
            CurrentLevel = Instantiate(levelContainer.Levels[currentLevelNumber], levelParent);
        }
        CurrentLevel.transform.position = levelParent.position;
        CurrentLevel.OnLevelLoaded += LevelLoaded;
    }
    
    // public void LoadCustomLevel(int id)
    // {
    //     UnloadLevel();
    //
    //     CurrentLevel = Instantiate(levelContainer.Levels[id], levelParent);
    //     CurrentLevel.transform.position = levelParent.position;
    //     CurrentLevel.OnLevelLoaded += LevelLoaded;
    // }
    [Button]
    public void LoadLevel(int id)
    {
        UnloadLevel();
        Statistics.CurrentLevelNumber=id;
        
        CurrentLevel = Instantiate(levelContainer.Levels[id], levelParent);
        CurrentLevel.transform.position = levelParent.position;
        CurrentLevel.OnLevelLoaded += LevelLoaded;
    }
    public void UnloadLevel(bool editor = false)
    {
        if (CurrentLevel != null)
        {
            CurrentLevel.OnLevelCompleted -= CurrentLevel_OnLevelCompleted;
            if (editor)
                DestroyImmediate(CurrentLevel.gameObject);
            else
                Destroy(CurrentLevel.gameObject);
        }
    }

    private void Start()
    {
        MyAnalyticsManager.SetUniqueLevelsCount(levelContainer.Levels.Count);
        MyAnalyticsManager.SetLevelsRandomizer(false);
    }

    /// <summary>
    /// Производит расчёт номера следующего уровня
    /// </summary>
    private void IncreaseLevelNumber()
    {
        Statistics.PlayerLevel++;
        //
        // if (Statistics.AllLevelsCompleted)
        // {
        //     print("clearLevels");
        //     currentLevelNumber = randomLevelNumber.GetAvailable();
        //     // Statistics.PlayerLevel = 0;
        //     // Statistics.AllLevelsCompleted = false;
        // }
        // else
        // {
        //     if (currentLevelNumber <= levelContainer.Levels.Count - 2)
        //     {
        //         currentLevelNumber++;
        //     }
        //     else
        //     {
        //         currentLevelNumber = randomLevelNumber.GetAvailable();
        //         Statistics.AllLevelsCompleted = true;
        //     }
        // }
        // print(currentLevelNumber);
        currentLevelNumber = levelContainer.Levels.GetNextIndex(currentLevelNumber);
        if (currentLevelNumber == 0 && Statistics.TutorialIsCompleted) currentLevelNumber++;
        print(currentLevelNumber);
        Statistics.CurrentLevelNumber = currentLevelNumber;
    }
    private void LevelLoaded()
    {
        CurrentLevel.OnLevelLoaded -= LevelLoaded;
        CurrentLevel.OnLevelCompleted += CurrentLevel_OnLevelCompleted;
        CurrentLevel.OnLevelLosing += CurrentLevel_OnLevelLosing;
        if (Statistics.CurrentLevelNumber > 0) MyAnalyticsManager.LevelStart(CurrentLevel.name);
        OnLevelLoaded?.Invoke();
    }
    public void CurrentLevel_OnLevelLosing()
    {
        SoundManager.Instance.PlaySound(SoundTypes.Lose);
        CurrentLevel.OnLevelCompleted -= CurrentLevel_OnLevelCompleted;
        CurrentLevel.OnLevelLosing -= CurrentLevel_OnLevelLosing;
        OnLevelNotPassed?.Invoke();
    }
    public void CurrentLevel_OnLevelCompleted()
    {
        SoundManager.Instance.PlaySound(SoundTypes.Win);
        MyAnalyticsManager.LevelWin();
        CurrentLevel.OnLevelCompleted -= CurrentLevel_OnLevelCompleted;
        CurrentLevel.OnLevelLosing -= CurrentLevel_OnLevelLosing;
        IncreaseLevelNumber();
        OnLevelCompleted?.Invoke();
    }
}