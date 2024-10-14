using Klondike;

namespace Modes
{
    public static class GameModes
    {
        public readonly static GameMode<KlondikeRules> KlondikeMode = new(new (
            "Klondike",
            "Classic"
            )
        );
        
        public static IGameMode[] List =
        {
            KlondikeMode
        };
    }
}