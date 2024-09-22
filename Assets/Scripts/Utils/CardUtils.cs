using Cards;

namespace Utils
{
    public static class CardUtils
    {
        public static CardColor SuitToColor(Suit suit)
        {
            return suit switch
            {
                Suit.Clubs => CardColor.Black,
                Suit.Spades => CardColor.Black,
                _ => CardColor.Red
            };
        }
    }
}