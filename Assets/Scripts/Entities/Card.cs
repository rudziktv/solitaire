using System;
using System.Collections;
using Cards;
using Controllers;
using UnityEngine;

namespace Entities
{
    public class Card : MonoBehaviour
    {
        private static readonly int FlipCardAnim = Animator.StringToHash("Flip Card");
        GameManager Manager => GameManager.Instance;
        GameRules Rules => GameManager.Instance.GameRules;
        GameSounds Sounds => GameSounds.Instance;
        GameAnimations Animations => GameAnimations.Instance;
        public int Value { get; private set; }
        public Suit Suit { get; private set; }
        public Slot Slot { get; set; }

        public bool Revealed
        {
            get => _revealed;
            set
            {
                if (_revealed != value)
                {
                    _animator.SetTrigger(FlipCardAnim);
                }
                _revealed = value;
            }
        }

        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Sprite _sprite;
        private bool _revealed;

        private Vector3 _beforeDragPosition;
        private Vector3 _dragOffset;
        private int _beforeDragOrder;

        private AudioSource _audioSource;
        
        public event Action<Card> OnMovementEnd;

        // private Stack _stack;

        public void SwitchSuit()
        {
            if (!_sprite) _sprite = Resources.Load<Sprite>(BuildCardSpritePath());
            _spriteRenderer.sprite = Revealed ? _sprite : Manager.BackCardSprite;
        }

        public void FlipCard(bool revealed, bool muted = false)
        {
            if (Revealed != revealed && !muted)
                _audioSource.PlayOneShot(Sounds.FlipCardSound);
            Revealed = revealed;
        }

        public virtual void Initialize(Suit suit, int value)
        {
            Suit = suit;
            Value = value;
            name = $"{Rules.ValueToRank(value)} of {suit}";
        }

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
        }

        public void MoveTo(MovementConfig config)
        {
            StopCoroutine(nameof(MovementCoroutine));
            StartCoroutine(nameof(MovementCoroutine), config);
        }

        private IEnumerator MovementCoroutine(MovementConfig config)
        {
            var time = 0f;
            var origin = transform.position;
            var previousOrder = _spriteRenderer.sortingOrder;
            if (config.OnTop)
                _spriteRenderer.sortingOrder = 999;
            while (transform.position != config.Target)
            {
                // Debug.Log($"move-coroutine {name}");
                transform.position = Animations.AnimatedMove(origin, config.Target, time);
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            _spriteRenderer.sortingOrder = config.FinalOrder ?? previousOrder;
            transform.position = config.Target;
            OnMovementEnd?.Invoke(this);
        }

        private void Start()
        {
            _sprite = Resources.Load<Sprite>(BuildCardSpritePath());
            _spriteRenderer.sprite = Revealed ? _sprite : Manager.BackCardSprite;
        }

        private string BuildCardSpritePath()
        {
            var rank = GameManager.Instance.GameRules.ValueToStringRank(Value);
            return $"Sprites/Cards/card{Suit.ToString()}{rank}";
        }

        private void OnMouseDown()
        {
            // Create stack if suitable
            if (!Revealed || !Slot.IsStackable(this) || Manager.DisabledInteractions) return;
            Slot.OnStackGrab(this);
            // _stack = Slot.OnStackGrab(this);
        }
    }
}