using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

namespace Entities
{
    public class Slot : MonoBehaviour
    {
        protected GameManager Manager => GameManager.Instance;
        protected GameRules Rules => Manager.GameRules;
        
        public List<Card> Cards { get; } = new();
        
        [SerializeField] private float gap = Parameters.CARD_GAP;
        [SerializeField] private float revealedGap = Parameters.REVEALED_CARD_GAP;

        public float RevealedGap
        {
            get => revealedGap;
            set => revealedGap = value;
        }

        public float Gap
        {
            get => gap;
            set => gap = value;
        }

        protected virtual void Start() { }

        public virtual void AddCards(params Card[] cards)
        {
            Cards.AddRange(cards);
            ReloadCards(true);
        }

        public virtual bool IsStackable(Card startingCard)
        {
            return true;
        }

        public virtual void ReloadCards(bool muted = false)
        {
            var v = transform.position - Vector3.forward;
            for (var i = 0; i < Cards.Count; i++)
            {
                var card = Cards[i];
                card.Slot = this;
                var cardGap = card.Revealed ? RevealedGap : Gap;
                // var vector = transform.position - Vector3.up * cardGap * i - Vector3.forward * (i + 1);
                // Debug.Log($"Gap: {cardGap} - Vector: {vector} // {card.name}");
                card.transform.position = v;
                card.GetComponent<SpriteRenderer>().sortingOrder = i + 1;
                v -= Vector3.up * cardGap;
                v -= Vector3.forward;
                // Debug.Log($"{card.name} reloaded : order {card.GetComponent<SpriteRenderer>().sortingOrder}");
            }
            RevealLast(muted);
        }

        protected virtual void RevealLast(bool muted = false)
        {
            if (Cards.Count > 0)
            {
                Cards[^1].FlipCard(true, muted);
            }
        }

        public virtual Stack OnStackGrab(Card startingCard)
        {
            var startIndex = Cards.IndexOf(startingCard);
            var stack = startingCard.AddComponent<Stack>();
            stack.Cards.AddRange(Cards.Where((_, i) => i >= startIndex));

            stack.OnSuccess = () =>
            {
                Cards.RemoveRange(startIndex, Cards.Count - startIndex);
                ReloadCards();
            };
            return stack;
        }

        public virtual bool CanStackBeDropped(Stack stack)
        {
            return false;
        }

        public virtual void OnStackDrop(Stack stack)
        {
            Cards.AddRange(stack.Cards);
            ReloadCards();
        }
    }
}