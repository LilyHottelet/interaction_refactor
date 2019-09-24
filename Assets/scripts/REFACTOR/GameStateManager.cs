using UnityStandardAssets.Characters.FirstPerson;

public enum GameState
{
    Default,
    Paused
}

public class GameStateManager : MonoBehaviourSingleton<GameStateManager>
{
    public GameState currentGameState { get; private set; }
    public FirstPersonController player;


    public void SwitchToState(GameState state)
    {
        currentGameState = state;
    }

    public void SetGamePlaying(bool play)
    {
        player.enabled = play;
        currentGameState = GameState.Paused;
    }
}
