using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class Stack : MonoBehaviour
    {
        public List<Card> Cards { get; set; } = new();
        public Action OnSuccess { get; set; }
        
        private SpriteRenderer _spriteRenderer;
        private Vector3 _beforeDragPosition;
        private Vector3 _dragOffset;

        private int _beforeDragOrder;
        
        public bool Cancel { get; set; } = false;

        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     Debug.Log($"TriggerEnter {other.name}");
        // }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void OnMouseDown()
        {
            _beforeDragPosition = transform.position;
            _beforeDragOrder = _spriteRenderer.sortingOrder;
            _spriteRenderer.sortingOrder = 999;
            Debug.Log($"OnMouseDown STACK");
        }

        protected virtual void OnMouseUp()
        {
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
            OnSuccess.Invoke();
            slot.OnStackDrop(this);
            Destroy(this);
        }

        public virtual void OnDropFail()
        {
            if (Cancel) return;
            _spriteRenderer.sortingOrder = _beforeDragOrder;
            transform.position = _beforeDragPosition;
            Destroy(this);
        }
    }
}