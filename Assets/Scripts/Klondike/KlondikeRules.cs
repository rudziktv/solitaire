using Cards;
using Entities;
using UnityEngine;
using Utils;

namespace Klondike
{
    public class KlondikeRules : GameRules
    {
        [SerializeField] private GameObject slotPrefab;
        
        private KlondikeSlot[] _slots;
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
    }
}