using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace Klondike
{
    public class KlondikeDeck : Deck
    {
        GameSounds Sounds => GameSounds.Instance;
        private Slot _slot;
        private List<Card> Cards { get; } = new();

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
            _audioSource.PlayOneShot(Sounds.FlipCardSound);
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
                // card.transform.position = vector;
                card.MoveTo(new(vector, i + 1, true));
                // card.GetComponent<SpriteRenderer>().sortingOrder = ;
                card.Slot = null;
            }
        }

        private void OnMouseUpAsButton()
        {
            if (Manager.DisabledInteractions) return;
            if (Cards.Count > 0)
            {
                DealCard();
                Manager.AddMove(() =>
                {
                    if (_slot.Cards.Count > 0)
                    {
                        var card = _slot.Cards[^1];
                        _slot.Cards.Remove(card);
                        Cards.Add(card);
                    }
                    RefreshCards();
                });
            }
            else
            {
                RestartCards();
                Manager.AddMove(() =>
                {
                    _slot.AddCards(Cards.ToArray());
                    foreach (var card in _slot.Cards)
                    {
                        card.FlipCard(true);
                    }
                    Cards.Clear();
                });
            }
        }

        private void OnDestroy()
        {
            Destroy(_slot.gameObject);
        }
    }
}