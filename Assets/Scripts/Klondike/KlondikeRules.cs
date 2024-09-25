using Cards;
using Entities;
using UnityEngine;
using Utils;

namespace Klondike
{
    public class KlondikeRules : GameRules
    {
        // [SerializeField] private GameObject slotPrefab;

        private KlondikeFinalSlot[] _finalSlots;
        private KlondikeSlot[] _slots;
        private KlondikeDeck _deck;
        private Card[] _cards;
        
        public override void GameStart()
        {
            base.GameStart();
            
            // Create cards for board
            _cards = new Card[52];
            for (var s = (Suit)0; s < (Suit)4; s++)
            {
                for (var r = 0; r < 13; r++)
                {
                    var card = Instantiate(cardPrefab).GetComponent<Card>();
                    _cards[(int)s * 13 + r] = card;
                    card.Initialize(s, r + 1);
                }
            }

            MathC.Sum(1);
            
            Shuffle.StandardShuffle(_cards);
            
            // Create 7 slots in middle
            _slots = new KlondikeSlot[7];
            Vector3 pos = new(-6f, 0f, 6f);
            for (int i = 0; i < 7; i++)
            {
                var slot = Instantiate(slotPrefab, transform, true);
                slot.name = $"Slot {i + 1}";
                slot.transform.position = pos;
                slot.GetComponent<SpriteRenderer>().sortingOrder = -2;
                
                var slotScript = slot.AddComponent<KlondikeSlot>();
                _slots[i] = slotScript;
                
                var s = MathC.Sum(i);
                slotScript.LoadCards(_cards[s..(s + i + 1)]);
                // Debug.Log($"{slot.name}: {s}, {s + i + 1}");
                
                pos.x += 2f;
            }
            
            // Create deck
            var deckObject = Instantiate(slotPrefab, transform, true);
            var deckPos = new Vector3(-6f, 3f, -10f);
            deckObject.transform.position = deckPos;
            _deck = deckObject.AddComponent<KlondikeDeck>();
            _deck.LoadCards(_cards[MathC.Sum(7)..^0]);
            
            // Create final slots
            _finalSlots = new KlondikeFinalSlot[4];
            var finalSlotPos = transform.position;
            finalSlotPos.z = 7f;
            finalSlotPos.y += 3f;
            for (int i = 0; i < 4; i++)
            {
                var finalSlot = Instantiate(slotPrefab, transform, true);
                finalSlot.name = $"Slot {i + 1}";
                finalSlot.transform.position = finalSlotPos;
                finalSlotPos.x += 2f;
                _finalSlots[i] = finalSlot.AddComponent<KlondikeFinalSlot>();
            }
        }

        public override void RemoveAllBoard()
        {
            base.RemoveAllBoard();

            if (_finalSlots != null)
            {
                foreach (var finalSlot in _finalSlots)
                {
                    Destroy(finalSlot.gameObject);
                }
            }

            if (_slots != null)
            {
                foreach (var slot in _slots)
                {
                    Destroy(slot.gameObject);
                }
            }
            
            if (_finalSlots != null)
                Destroy(_deck.gameObject);
            
            if (_cards != null)
            {
                foreach (var card in _cards)
                {
                    Destroy(card.gameObject);
                }
            }
        }

        public override Rank ValueToRank(int value)
        {
            return value switch
            {
                1 => Rank.Ace,
                11 => Rank.Jack,
                12 => Rank.Queen,
                13 => Rank.King,
                _ => (Rank)value
            };
        }

        public override void OnCardDoubleClick(Card card, Stack stack)
        {
            base.OnCardDoubleClick(card, stack);

            foreach (var finalSlot in _finalSlots)
            {
                if (finalSlot.CanStackBeDropped(stack))
                {
                    stack.Cancel = true;
                    // Debug.Log("Stack can be dropped");
                    stack.OnDropSuccess(finalSlot);
                    // finalSlot.ReloadCards();
                    // finalSlot.OnStackDrop(stack);
                    // stack.OnSuccess.Invoke();
                    return;
                }
            }
            
            stack.OnDropFail();
        }
    }
}