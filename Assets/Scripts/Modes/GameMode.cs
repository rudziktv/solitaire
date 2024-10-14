using UnityEngine;

namespace Modes
{
    public class GameMode<T> : IGameMode where T : GameRules
    {
        public GameModeData Data { get; }

        public delegate void InitializeGameModeArgs(T rules, GameObject rulesGameObject);
        
        private GameObject _gameObject;
        private readonly InitializeGameModeArgs _onInitializeGameMode;

        public GameMode(GameModeData data, InitializeGameModeArgs onInitializeGameMode = null)
        {
            Data = data;
            _onInitializeGameMode = onInitializeGameMode;
        }

        private void CreateGameObject()
        {
            _gameObject = new GameObject($"{Data.Name} Rules")
            {
                transform =
                {
                    parent = GameManager.Instance.transform
                }
            };
        }


        public GameRules InitializeGameRules()
        {
            CreateGameObject();
            var rules = _gameObject.AddComponent<T>();
            _onInitializeGameMode?.Invoke(rules, _gameObject);
            return rules;
        }
    }
}