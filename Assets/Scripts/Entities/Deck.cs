using UnityEngine;

namespace Entities
{
    public class Deck : MonoBehaviour
    {
        public GameManager Manager => GameManager.Instance;
        public GameRules Rules => Manager.GameRules;
    }
}