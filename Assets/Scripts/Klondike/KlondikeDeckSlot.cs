using System.Linq;
using Entities;
using UnityEngine;

namespace Klondike
{
    public class KlondikeDeckSlot : Slot
    {
        public override bool IsStackable(Card startingCard)
        {
            // return base.IsStackable(startingCard);
            return startingCard == Cards.Last();
        }
    }
}