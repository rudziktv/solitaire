using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Utils;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Entities
{
    public class Stack : MonoBehaviour, IPointerUpHandler
    {
        private GameRules Rules => Manager.GameRules;
        private GameSounds Sounds => GameSounds.Instance;
        private GameManager Manager => GameManager.Instance;
        
        public bool Cancel { get; set; }
        public bool Mute { get; set; } = false;
        public Action OnSuccess { get; set; }
        public Action<Slot> Undo { get; set; }
        public List<Card> Cards { get; set; } = new();

        private int _beforeDragLayerMask;
        private Vector3 _dragOffset;
        private Vector3 _beforeDragPosition;
        private AudioSource _audioSource;
        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
        }

        protected virtual bool CardAutoMove()
        {
            // Debug.Log($"CardAutoMove {name}");
            var oldPos = _beforeDragPosition;
            oldPos.z = transform.position.z;
            if (!((oldPos - transform.position).magnitude < Parameters.CARD_DISTANCE_SOUND_THRESHOLD)) return false;
            Rules.OnCardClick(this);
            return true;
        }

        [CanBeNull]
        protected virtual Slot DroppableSlot()
        {
            Vector3[] rays = new Vector3[9];
            
            var size = _spriteRenderer.size;
            var halfWidth = size.x / 2f;
            var halfHeight = size.y / 2f;

            
            var vector = transform.position;
            vector.x -= halfWidth;
            vector.y -= halfHeight;
            vector.z += 0.5f;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rays[j + 3 * i] = vector;
                    vector.x += halfWidth;
                }
                vector.x = transform.position.x - halfWidth;
                vector.y += halfHeight;
            }
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

            foreach (var ray in rays)
            {
                var hit = Physics2D.Raycast(ray, transform.forward);
                if (hit.collider == null)
                    continue;
                
                var card = hit.collider.GetComponent<Card>();
                var slot = card == null ? hit.collider.GetComponent<Slot>() : card.Slot;
                
                if (slot == null)
                    continue;
                
                if (!slot.CanStackBeDropped(this)) continue;
                gameObject.layer = LayerMask.NameToLayer("Default");
                return slot;
            }
            gameObject.layer = LayerMask.NameToLayer("Default");
            return null;
        }

        public void PickUpSound()
        {
            if (Mute) return;
            _audioSource.PlayOneShot(Sounds.PickUpCardSound);
        }

        public void PutDownSound()
        {
            if (Mute) return;

            var firstCard = Cards.First();

            Action<Card> listener = null;
            listener = (card) =>
            {
                var source = card.GetComponent<AudioSource>();
                source.PlayOneShot(Sounds.PutDownCardSound);
                firstCard.OnMovementEnd -= listener;
            };
            firstCard.OnMovementEnd += listener;
        }

        public void DropSound()
        {
            if (Mute) return;
            _audioSource.PlayOneShot(Sounds.MoveNotAllowedSound);
            var firstCard = Cards.First();

            Action<Card> listener = null;
            listener = (card) =>
            {
                var source = card.GetComponent<AudioSource>();
                source.PlayOneShot(Sounds.DropDownCardSound);
                firstCard.OnMovementEnd -= listener;
            };
            firstCard.OnMovementEnd += listener;
        }

        public virtual void OnDropSuccess(Slot slot)
        {
            PutDownSound();
            slot.OnStackDrop(this);
            OnSuccess.Invoke();
            
            if (Undo != null)
                Manager.AddMove(() => Undo.Invoke(slot));
            Manager.GameRules.OnStackMove(this);
            
            Destroy(this);
        }

        public virtual void OnDropFail()
        {
            DropSound();
            if (Cancel) return;
            Cards.First().Slot.ReloadCards();
            Destroy(this);
        }

        // replacing OnPointerDown, OnMouseDown
        private void Start()
        {
            // Debug.Log($"Start Stack: {name}");
            _beforeDragPosition = transform.position;
            _spriteRenderer.sortingOrder = 999;

            for (int i = 1; i < Cards.Count; i++)
            {
                var card = Cards[i];
                card.GetComponent<SpriteRenderer>().sortingOrder = 999 + i;
            }
            
            var mousePos = Camera.main.ScreenToWorldPoint(PointerUtils.GetPointerPosition());
            _dragOffset = transform.position - mousePos;
            PickUpSound();
        }

        // replacing OnMouseUp
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            // Debug.Log($"OnPointerUp Stack: {name}");
            
            if (CardAutoMove()) return;
            if (Cancel) return;
            
            var slot = DroppableSlot();
            if (slot != null)
            {
                OnDropSuccess(slot);
                return;
            }
            
            OnDropFail();
        }

        // replacing OnDrag, OnMouseDrag
        public virtual void Update()
        {
            // Debug.Log($"Update Stack: {name}, {Mouse.current.position.ReadValue()}");
            var mousePos = Camera.main.ScreenToWorldPoint(PointerUtils.GetPointerPosition());
            transform.position = mousePos + _dragOffset - Vector3.forward * 30;
            
            var v = transform.position;
            for (int i = 1; i < Cards.Count; i++)
            {
                var card = Cards[i];
                v -= Vector3.up * Parameters.REVEALED_CARD_GAP;
                v -= Vector3.forward;
                card.transform.position = v;
            }
        }

        public virtual void AutoMove()
        {
            _beforeDragPosition = transform.position;
            
            if (CardAutoMove()) return;
            if (Cancel) return;
            
            OnDropFail();
        }
    }
}