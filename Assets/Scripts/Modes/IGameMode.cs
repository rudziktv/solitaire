namespace Modes
{
    public interface IGameMode
    {
        public GameModeData Data { get; }
        public GameRules InitializeGameRules();
    }
}