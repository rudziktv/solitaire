using Cards;
using Entities;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    [SerializeField] protected bool cardsRevealed = false;
    protected GameManager Manager => GameManager.Instance;
    public GameObject CardPrefab => Manager.CardPrefab;
    public GameObject SlotPrefab => Manager.SlotPrefab;

    public bool CardsRevealed => cardsRevealed;

    public virtual void GameStart(string args = "")
    {
        RemoveAllBoard();
    }
    
    public virtual void RemoveAllBoard() { }

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
    
    public virtual void OnCardClick(Stack stack) { }
}