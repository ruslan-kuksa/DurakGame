using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Models
{
    public struct BotAction
    {
        public Card Card;
        public bool IsDefending;

        public BotAction(Card card, bool isDefending)
        {
            Card = card;
            IsDefending = isDefending;
        }
    }

    public class BotPlayer : Player
    {
        public BotPlayer(string name) : base(name)
        {

        }
        public BotAction SelectCardToPlay(Table table, Card trumpCard)
        {
            if (table.IsEmpty())
            {
                return new BotAction(Hand.OrderBy(card => Convert.ToInt32(card.Rank)).FirstOrDefault(), false);
            }
            else
            {
                foreach (Card cardOnTable in table.AttackCards.Concat(table.DefenseCards))
                {
                    Card suitableCard = Hand.FirstOrDefault(card => card.Rank == cardOnTable.Rank || (card.Suit == trumpCard.Suit && card.Rank > cardOnTable.Rank));
                    if (suitableCard != null)
                    {
                        return new BotAction(suitableCard, table.IsAttackingCard(cardOnTable));
                    }
                }
            }
            return new BotAction(null, false);
        }
    }
}



