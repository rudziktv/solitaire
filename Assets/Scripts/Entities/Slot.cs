using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Entities
{
    public class Slot : MonoBehaviour
    {
        protected GameManager Manager => GameManager.Instance;
        protected GameRules Rules => Manager.GameRules;
        
        public List<Card> Cards { get; } = new();

        [SerializeField] protected float gap = 0.2f;

        public virtual void LoadCards(Card[] cards)
        {
            for (var i = 0; i < cards.Length; i++)
            {
                var card = cards[i];
                var vector = transform.position - Vector3.up * gap * i - Vector3.forward * (i + 1);
                // var vector = transform.position - Vector3.up * gap * i;
                Cards.Add(card);
                card.transform.position = vector;
                card.GetComponent<SpriteRenderer>().sortingOrder = i + 1;
                card.Revealed = false;
                card.Slot = this;
            }
            RevealLast();
        }

        public virtual bool IsStackable(Card startingCard)
        {
            return false;
        }

        public virtual void ReloadCards()
        {
            for (var i = 0; i < Cards.Count; i++)
            {
                var card = Cards[i];
                var vector = transform.position - Vector3.up * gap * i - Vector3.forward * (i + 1);
                card.transform.position = vector;
                card.GetComponent<SpriteRenderer>().sortingOrder = i + 1;
                card.Slot = this;
            }
            RevealLast();
        }

        protected virtual void RevealLast()
        {
            if (Cards.Count > 0)
                Cards[^1].Revealed = true;
        }

        public virtual Stack OnStackGrab(Card startingCard)
        {
            var startIndex = Cards.IndexOf(startingCard);
            var stack = startingCard.AddComponent<Stack>();
            var list = Cards.ToArray()[startIndex..(Cards.Count - 1)];
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