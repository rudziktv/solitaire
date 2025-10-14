using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Entities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Klondike
{
    public class KlondikeDeck : Deck, IPointerClickHandler
    {
        public int DealSize { get; set; } = 3;
        
        private static GameSounds Sounds => GameSounds.Instance;

        public int TotalCardsCount => Cards.Count + _slot.Cards.Count;
        private List<Card> Cards { get; } = new();
        
        private KlondikeDeckSlot _slot;
        private AudioSource _audioSource;
        
        private void Start()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            name = "Deck";
            var slotGameObject = Instantiate(Rules.SlotPrefab, Rules.transform, true);
            slotGameObject.name = "Deck Slot";
            var vector = transform.position;
            vector.x += 2f;
            slotGameObject.transform.position = vector;
            _slot = slotGameObject.AddComponent<KlondikeDeckSlot>();
            _slot.DealSize = DealSize;
            _slot.GapVector = Vector3.right;
            _slot.Gap = 0;
            _slot.RevealedGap = 0.3f;
        }

        public void LoadCards(Card[] cards)
        {
            Cards.AddRange(cards);
            RefreshCards();
        }

        private void DealFromDeck()
        {
            // NEW with DealSize
            var dealCards = new List<Card>();
            int i;
            for (i = Cards.Count - 1; i >= 0 && (Cards.Count - 1) - i < DealSize; i--)
            {
                // Debug.Log($"Deal For loop {i}");
                dealCards.Add(Cards[i]);
                Cards[i].Revealed = true;
            }
            _slot.AddCards(dealCards.ToArray());
            _audioSource.PlayOneShot(Sounds.FlipCardSound);
            Cards.RemoveRange(i + 1, dealCards.Count);
            Manager.AddMove(() =>
            {
                if (_slot.Cards.Count > 0)
                {
                    foreach (var previousDeal in dealCards)
                    {
                        _slot.Cards.Remove(previousDeal);
                    }
                    _slot.ReloadCards(true);
                    dealCards.Reverse();
                    Cards.AddRange(dealCards);
                }
                RefreshCards();
                _audioSource.PlayOneShot(Sounds.DeckCardUndoSound);
            });
        }

        private void RestartCards()
        {
            if (_slot.Cards.Count == 0) return;
            Cards.AddRange(_slot.Cards);
            Cards.Reverse();
            _slot.Cards.Clear();
            RefreshCards();
            _audioSource.PlayOneShot(Sounds.ResetDeckSound);
        }

        public void RefreshCards()
        {
            var vector = transform.position;
            vector.z += 0.05f;
            for (int i = 0; i < Cards.Count; i++)
            {
                var card = Cards[i];
                card.Revealed = false;
                card.MoveTo(new(vector, i + 1, true));
                card.Slot = null;
            }
        }

        private void OnDestroy()
        {
            Destroy(_slot.gameObject);
        }

        // replacing OnMouseUpAsButton
        public void OnPointerClick(PointerEventData eventData)
        {
            // Debug.Log($"OnPointerClick KlondikeDeck: {name}");
            
            if (Manager.DisableInteractions) return;
            if (Cards.Count > 0)
            {
                DealFromDeck();
            }
            else
            {
                RestartCards();
                Manager.AddMove(() =>
                {
                    Cards.Reverse();
                    _slot.AddCards(Cards.ToArray());
                    foreach (var card in _slot.Cards)
                    {
                        card.FlipCard(true, true);
                    }
                    Cards.Clear();
                    _audioSource.PlayOneShot(Sounds.ResetDeckUndoSound);
                });
            }
        }
    }
}