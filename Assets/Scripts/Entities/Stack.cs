using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using JetBrains.Annotations;
using UnityEngine;
using Utils;

namespace Entities
{
    public class Stack : MonoBehaviour
    {
        private GameManager Manager => GameManager.Instance;
        private GameRules Rules => Manager.GameRules;
        private GameSounds Sounds => GameSounds.Instance;

        public Slot OldSlot { get; set; }
        public List<Card> Cards { get; set; } = new();
        public bool Mute { get; set; } = false;
        public Action OnSuccess { get; set; }
        public Action<Slot> Undo { get; set; }

        private AudioSource _audioSource;
        private SpriteRenderer _spriteRenderer;
        private Vector3 _beforeDragPosition;
        private Vector3 _dragOffset;

        private int _beforeDragOrder;
        private int _beforeDragLayerMask;
        
        public bool Cancel { get; set; } = false;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
            // _beforeDragLayerMask = gameObject.layer;
            // gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        protected virtual void OnMouseDown()
        {
            _beforeDragOrder = _spriteRenderer.sortingOrder;
            _beforeDragPosition = transform.position;
            _spriteRenderer.sortingOrder = 999;

            for (int i = 1; i < Cards.Count; i++)
            {
                var card = Cards[i];
                card.GetComponent<SpriteRenderer>().sortingOrder = 999 + i;
            }
            
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _dragOffset = transform.position - mousePos;
            PickUpSound();
        }

        protected virtual void OnMouseDrag()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

        protected virtual void OnMouseUp()
        {
            if (CardAutoMove()) return;
            if (Cancel) return;

            // var currentLayer = gameObject.layer;
            var slot = DroppableSlot();
            if (slot != null)
            {
                OnDropSuccess(slot);
                return;
            }
            
            OnDropFail();
        }

        protected virtual bool CardAutoMove()
        {
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
            var firstCard = Cards.First();

            Action<Card> listener = null;
            listener = (card) =>
            {
                // Debug.Log($"PutDownSound {card}");
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
            
            Destroy(this);
        }

        public virtual void OnDropFail()
        {
            DropSound();
            if (Cancel) return;
            Cards.First().Slot.ReloadCards();
            Destroy(this);
        }
    }
}