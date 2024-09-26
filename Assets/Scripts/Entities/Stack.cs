using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using UnityEngine;
using Utils;

namespace Entities
{
    public class Stack : MonoBehaviour
    {
        private GameSounds Sounds => GameSounds.Instance;
        
        public List<Card> Cards { get; set; } = new();
        public Action OnSuccess { get; set; }

        private AudioSource _audioSource;
        private SpriteRenderer _spriteRenderer;
        private Vector3 _beforeDragPosition;
        private Vector3 _dragOffset;

        private int _beforeDragOrder;

        private float _mouseDownTime;
        
        public bool Cancel { get; set; } = false;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            _mouseDownTime += Time.deltaTime;
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
            // Debug.Log($"OnMouseDown STACK");
            
            // yield return new WaitForSeconds(Parameters.CARD_SOUND_PRESS_THRESHOLD);
            // _audioSource.clip = Sounds.PickUpCardSound;
            // _audioSource.Play();
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
            // if (_mouseDownTime >= Parameters.CARD_SOUND_PRESS_THRESHOLD)
            // {
            //     _audioSource.clip = Sounds.PutDownCardSound;
            //     _audioSource.Play();
            // }
            
            if (Cancel) return;
            Ray ray = new()
            {
                origin = transform.position + Vector3.up * 1f,
                direction = transform.forward
            };
            // Debug.DrawRay(ray.origin, ray.direction, Color.green, 20000f);
            // if (Physics.Raycast(ray, out var hit))
            // {
            //     Debug.Log(hit.collider.name);
            // }
            var hit2D = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit2D.collider == null)
            {
                OnDropFail();
                return;
            }
            
            Debug.Log($"{hit2D.collider.name}");
            
            var card = hit2D.collider.GetComponent<Card>();
            var slot = card == null ? hit2D.collider.GetComponent<Slot>() : card.Slot;
            
            Debug.Log($"{card} {slot}");

            if (card == null && slot == null)
            {
                OnDropFail();
                return;
            }

            if (slot.CanStackBeDropped(this))
            {
                OnDropSuccess(slot);
                return;
            }
            
            OnDropFail();
        }

        public virtual void OnDropSuccess(Slot slot)
        {
            slot.OnStackDrop(this);
            OnSuccess.Invoke();
            Destroy(this);
        }

        public virtual void OnDropFail()
        {
            if (Cancel) return;
            Cards.First().Slot.ReloadCards();
            // _spriteRenderer.sortingOrder = _beforeDragOrder;
            // transform.position = _beforeDragPosition;
            Destroy(this);
        }
    }
}