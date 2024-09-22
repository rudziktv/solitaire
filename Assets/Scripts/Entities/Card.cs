using System;
using Cards;
using Unity.VisualScripting;
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

        private Stack _stack;

        public virtual void Initialize(Suit suit, int value)
        {
            Suit = suit;
            Value = value;
            name = $"{Rules.ValueToRank(value)} of {suit}";
        }

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            // var path = BuildCardSpritePath();
            // Debug.Log(BuildCardSpritePath());
        }

        private void Start()
        {
            _sprite = Resources.Load<Sprite>(BuildCardSpritePath());
            // Revealed = Rules.CardsRevealed;
        }

        private string BuildCardSpritePath()
        {
            var rank = GameManager.Instance.GameRules.ValueToStringRank(Value);
            return $"Sprites/Cards/card{Suit.ToString()}{rank}";
        }

        private void OnMouseDown()
        {
            //Save previous position to snap if failed to drag.
            if (!Revealed || !Slot.IsStackable(this)) return;

            _stack = Slot.OnStackGrab(this);

            // _beforeDragPosition = transform.position;
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _dragOffset = transform.position - mousePos;
            _beforeDragOrder = _spriteRenderer.sortingOrder;
            _spriteRenderer.sortingOrder = 999;
        }

        private void OnMouseDrag()
        {
            if (!Revealed || !Slot.IsStackable(this)) return;
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos + _dragOffset - Vector3.forward * 30;
        }

        private void OnMouseUp()
        {
            if (!Revealed || !Slot.IsStackable(this)) return;
            
            // Debug.Log($"{transform.position} {_beforeDragPosition}");
            
            // _stack.OnStackDrop();

            // transform.position = _beforeDragPosition;
            _spriteRenderer.sortingOrder = _beforeDragOrder;
            
            // Destroy(_stack);
            // _stack = null;
        }
    }
}