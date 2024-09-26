using System.Linq;
using Cards;
using Entities;
using UnityEngine;
using Utils;

namespace Klondike
{
    public class KlondikeFinalSlot : Slot
    {
        protected override void Start()
        {
            base.Start();
            Gap = 0f;
            RevealedGap = 0f;
        }

        public override bool CanStackBeDropped(Stack stack)
        {
            if (stack.Cards.Count != 1) return false;
            var stackFirst = stack.Cards.First();
            if (Cards.Count == 0) return Rules.ValueToRank(stackFirst.Value) == Rank.Ace;
            var slotLast = Cards.Last();
            return slotLast.Value + 1 == stackFirst.Value && stackFirst.Suit == slotLast.Suit;
        }
    }
}