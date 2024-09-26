using Cards;
using UnityEngine;

namespace Entities
{
    public class Card : MonoBehaviour
    {
        GameManager Manager => GameManager.Instance;
        GameRules Rules => GameManager.Instance.GameRules;
        public int Value { get; private set; }
        public Suit Suit { get; private set; }
        public Slot Slot { get; set; }

        public bool Revealed
        {
            get => _revealed;
            set
            {
                _revealed = value;
                if (!_sprite) _sprite = Resources.Load<Sprite>(BuildCardSpritePath());
                _spriteRenderer.sprite = Revealed ? _sprite : Manager.BackCardSprite;
            }
        }

        private SpriteRenderer _spriteRenderer;
        private Sprite _sprite;
        private bool _revealed;

        private Vector3 _beforeDragPosition;
        private Vector3 _dragOffset;
        private int _beforeDragOrder;

        // private Stack _stack;

        public virtual void Initialize(Suit suit, int value)
        {
            Suit = suit;
            Value = value;
            name = $"{Rules.ValueToRank(value)} of {suit}";
        }

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _sprite = Resources.Load<Sprite>(BuildCardSpritePath());
        }

        private string BuildCardSpritePath()
        {
            var rank = GameManager.Instance.GameRules.ValueToStringRank(Value);
            return $"Sprites/Cards/card{Suit.ToString()}{rank}";
        }

        private void OnMouseDown()
        {
            // Create stack if suitable
            if (!Revealed || !Slot.IsStackable(this)) return;
            Slot.OnStackGrab(this);
            // _stack = Slot.OnStackGrab(this);
        }
    }
}