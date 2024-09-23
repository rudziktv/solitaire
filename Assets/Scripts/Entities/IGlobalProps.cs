namespace Entities
{
    public interface IGlobalProps
    {
        public GameManager Manager => GameManager.Instance;
        public GameRules Rules => Manager.GameRules;
    }
}