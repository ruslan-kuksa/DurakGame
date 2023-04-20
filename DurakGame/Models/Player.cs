using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DurakGame.Models
{
    public class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; }

        public Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
        }

        public void AddCardToHand(Card card)
        {
            Hand.Add(card);
        }

        public void RemoveCardFromHand(Card card)
        {
            Hand.Remove(card);
        }
        public bool HasCardsInHand()
        {
            return Hand.Count > 0;
        }
        public virtual bool IsHuman()
        {
            return false;
        }
        /*public abstract Card MakeMove(List<Card> tableCards);*/

        protected bool IsValidAttackCard(Card card, List<Card> tableCards)
        {
            if (tableCards.Count == 0)
            {
                return true;
            }
            return tableCards.Any(tableCard => tableCard.Rank == card.Rank);
        }

        protected bool IsValidDefenseCard(Card card, List<Card> tableCards)
        {
            Card lastAttackCard = tableCards.Last();
            bool isHigherRank = card.Rank > lastAttackCard.Rank;
            bool isSameSuit = card.Suit == lastAttackCard.Suit;
            bool isAce = card.Rank == Rank.Ace;
            return (isHigherRank && isSameSuit) || (isAce && !tableCards.Any(tc => tc.Rank == Rank.Ace));
        }
    }
}
