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
                Card chosenCard = Hand.Where(card => card.Suit != trumpCard.Suit).OrderBy(card => Convert.ToInt32(card.Rank)).FirstOrDefault();
                if (chosenCard == null)
                {
                    chosenCard = Hand.OrderBy(card => Convert.ToInt32(card.Rank)).FirstOrDefault();
                }

                return new BotAction(chosenCard, false);
            }
            else
            {
                foreach (Card cardOnTable in table.AttackCards.Concat(table.DefenseCards))
                {
                    Card suitableCard = Hand.FirstOrDefault(card => card.CanBeat(cardOnTable, trumpCard.Suit));
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




