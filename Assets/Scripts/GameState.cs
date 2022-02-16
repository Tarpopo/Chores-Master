using DefaultNamespace;
using UnityEngine;
public class GameState : Singleton<GameState>
{
    private GameStates _currentState;
    private GameStates _previousState;
    public bool IsPause => _currentState == GameStates.Pause;
    public bool IsWin => _currentState == GameStates.Win;
    public bool IsLose => _currentState == GameStates.Lose;
    public bool IsWinOrLose => _currentState == GameStates.Lose || _currentState == GameStates.Win;
    public bool IsActive => _currentState == GameStates.Active;
    public void SetGameState(GameStates gameState)
    {
        _previousState = _currentState;
        SetPause(gameState == GameStates.Pause);
        _currentState = gameState;
    }
    public void ReturnToPreviousState() => SetGameState(_previousState);
    public void ChangePauseState()=> SetGameState(_currentState != GameStates.Pause ? GameStates.Pause : GameStates.Active);
    public void SetPauseState(bool isPause)=>SetGameState(isPause?GameStates.Pause:GameStates.Active);
    private void SetPause(bool isPause)=>Time.timeScale = isPause ? 0 : 1;
}
public enum GameStates
{
    Active,
    Pause,
    Win,
    Lose
}
