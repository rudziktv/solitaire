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
        
        [SerializeField] private Vector3 gapVector = Vector3.down;
        [SerializeField] private float gap = Parameters.CARD_GAP;
        [SerializeField] private float revealedGap = Parameters.REVEALED_CARD_GAP;

        public bool slotRevealOverride = false;

        public Vector3 GapVector
        {
            get => gapVector;
            set => gapVector = value;
        }

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
            PreReloadCardFunction();
            for (var i = 0; i < Cards.Count; i++)
            {
                var card = Cards[i];
                card.Slot = this;
                var cardGap = card.Revealed ? RevealedGap : Gap;
                card.MoveTo(new(v, i + 1, true));
                v += GapVector * cardGap;
                v -= Vector3.forward;
            }
            PostReloadCardFunction(muted);
        }
        
        protected virtual void PreReloadCardFunction() { }

        protected virtual void PostReloadCardFunction(bool muted = false)
        {
            if (Cards.Count > 0)
            {
                Cards[^1].FlipCard(true, muted);
            }
        }

        public virtual Stack OnStackGrab(Card startingCard, bool automove = false)
        {
            var startIndex = Cards.IndexOf(startingCard);
            var stack = startingCard.AddComponent<Stack>();
            var selectedCards = Cards.Where((_, i) => i >= startIndex).ToList();
            stack.Cards.AddRange(selectedCards);
            stack.OnSuccess = () =>
            {
                Cards.RemoveRange(startIndex, Cards.Count - startIndex);
                ReloadCards();
            };
            
            
            stack.Undo = newSlot =>
            {
                var start = newSlot.Cards.Count - selectedCards.Count;
                start = Mathf.Clamp(start, 0, newSlot.Cards.Count - 1);
                newSlot.Cards.RemoveRange(start, selectedCards.Count);
                Cards.AddRange(selectedCards);
                
                ReloadCards();
                newSlot.ReloadCards();
            };
            
            if (startIndex > 0)
            {
                var cardBehindStack = Cards[startIndex - 1];
                if (!cardBehindStack.Revealed)
                {
                    var old = stack.Undo;
                    stack.Undo = (newSlot) =>
                    {
                        cardBehindStack.FlipCard(false);
                        old.Invoke(newSlot);
                    };
                }
            }
            
            if (automove)
                stack.AutoMove();
            
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