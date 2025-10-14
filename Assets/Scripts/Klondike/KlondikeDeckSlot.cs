using System.Linq;
using Entities;
using UnityEngine;

namespace Klondike
{
    public class KlondikeDeckSlot : Slot
    {
        public int DealSize { get; set; } = 1;
        
        public override bool IsStackable<T>(T startingCard)
        {
            return startingCard == Cards.Last();
        }

        protected override void PreReloadCardFunction()
        {
            base.PreReloadCardFunction();
            for (int i = 0; i < Cards.Count; i++)
            {
                Cards[i].FlipCard(Cards.Count - 1 - i < DealSize, true);
            }
        }

        protected override void PostReloadCardFunction(bool muted = false)
        {
            // removal
        }
    }
}