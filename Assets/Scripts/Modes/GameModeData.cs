namespace Modes
{
    public class GameModeData
    {
        public string Name { get; }
        public string Description { get; }
        // public Texture2D Backface { get; }

        public GameModeData(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}