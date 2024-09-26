using System;
using System.Collections.Generic;
using Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace Klondike
{
    public class KlondikeDeck : Deck
    {
        private Slot _slot;
        private List<Card> Cards { get; } = new();
        
        private void Start()
        {
            name = "Deck";
            var slotGameObject = Instantiate(Rules.SlotPrefab, Rules.transform, true);
            slotGameObject.name = "Deck Slot";
            var vector = transform.position;
            vector.x += 2f;
            slotGameObject.transform.position = vector;
            _slot = slotGameObject.AddComponent<Slot>();
            _slot.Gap = 0f;
            _slot.RevealedGap = 0f;
        }

        public void LoadCards(Card[] cards)
        {
            Cards.AddRange(cards);
            RefreshCards();
        }

        private void DealCard()
        {
            var card = Cards[^1];
            _slot.AddCards(card);
            Cards.Remove(card);
        }

        private void RestartCards()
        {
            Cards.AddRange(_slot.Cards);
            Cards.Reverse();
            _slot.Cards.Clear();
            RefreshCards();
        }

        public void RefreshCards()
        {
            var vector = transform.position;
            vector.z += 0.05f;
            for (int i = 0; i < Cards.Count; i++)
            {
                var card = Cards[i];
                card.Revealed = false;
                card.transform.position = vector;
                card.GetComponent<SpriteRenderer>().sortingOrder = i + 1;
                card.Slot = null;
            }
        }

        private void OnMouseUpAsButton()
        {
            if (Cards.Count > 0) DealCard();
            else RestartCards();
        }

        private void OnDestroy()
        {
            Destroy(_slot.gameObject);
        }
    }
}