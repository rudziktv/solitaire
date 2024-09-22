using Cards;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    [SerializeField] protected bool cardsRevealed = false;
    [SerializeField] protected GameObject cardPrefab;
    // [SerializeField] protected GameObject slotPrefab;

    public bool CardsRevealed => cardsRevealed;

    public virtual void GameStart() { }

    public virtual Rank ValueToRank(int value)
    {
        return (Rank)value;
    }

    public virtual string ValueToStringRank(int value)
    {
        var rank = ValueToRank(value);
        return rank is Rank.Ace or Rank.King or Rank.Queen or Rank.Jack ? ((char)rank).ToString() : ((int)rank).ToString();
    }
    
    public virtual void OnStackMove() { }
}