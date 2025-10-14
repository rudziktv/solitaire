using System.Linq;
using Cards;
using Entities;
using UnityEngine;
using Utils;

namespace Klondike
{
    public class KlondikeSlot : Slot
    {
        public override bool IsStackable<T>(T startingCard)
        {
            var startIndex = Cards.IndexOf(startingCard);
            if (Cards.Count - startIndex <= 1) return true;
            for (int i = startIndex; i < Cards.Count - 1; i++)
            {
                var card = Cards[i];
                var nextCard = Cards[i + 1];
                if (CardUtils.SuitToColor(nextCard.Suit) == CardUtils.SuitToColor(card.Suit) || card.Value != nextCard.Value + 1) return false;
            }
            return true;
        }

        public override bool CanStackBeDropped(Stack stack)
        {
            var stackFirst = stack.Cards.First();
            if (Cards.Count <= 0) return Rules.ValueToRank(stackFirst.Value) == Rank.King;
            var slotLast = Cards.Last();
            return slotLast.Value == stackFirst.Value + 1 && CardUtils.SuitToColor(stackFirst.Suit) != CardUtils.SuitToColor(slotLast.Suit);
        }
    }
}