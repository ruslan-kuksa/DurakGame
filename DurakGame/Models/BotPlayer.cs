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
        public bool IsAttacking;

        public BotAction(Card card, bool isDefending, bool isAttacking)
        {
            Card = card;
            IsDefending = isDefending;
            IsAttacking = isAttacking;
        }
    }

    public class BotPlayer : Player
    {
        public BotPlayer(string name) : base(name)
        {

        }
        public BotAction? SelectCardToAttack(Table table, Card trumpCard)
        {
            Card chosenCard = Hand
                .Where(card => card.Suit != trumpCard.Suit)
                .OrderBy(card => Convert.ToInt32(card.Rank))
                .FirstOrDefault();

            if (chosenCard == null)
            {
                chosenCard = Hand
                    .Where(card => card.Suit == trumpCard.Suit)
                    .OrderBy(card => Convert.ToInt32(card.Rank))
                    .FirstOrDefault();
            }

            return chosenCard != null ? new BotAction(chosenCard, false, true) : null;
        }

        public BotAction? SelectCardToDefend(Table table, Card trumpCard)
        {
            if (table.IsEmpty())
            {
                return SelectCardToAttack(table, trumpCard);
            }
            else
            {
                foreach (Card cardOnTable in table.AttackCards)
                {
                    var canBeatCards = Hand
                        .Where(card => card.Suit == cardOnTable.Suit && card.CanBeat(cardOnTable, trumpCard.Suit))
                        .OrderBy(card => Convert.ToInt32(card.Rank))
                        .ToList();

                    if (canBeatCards.Any())
                    {
                        return new BotAction(canBeatCards.First(), true, false);
                    }
                }

                foreach (Card cardOnTable in table.AttackCards)
                {
                    var trumpCards = Hand
                        .Where(card => card.Suit == trumpCard.Suit && card.CanBeat(cardOnTable, trumpCard.Suit))
                        .OrderBy(card => Convert.ToInt32(card.Rank))
                        .ToList();

                    if (trumpCards.Any())
                    {
                        return new BotAction(trumpCards.First(), true, false);
                    }
                }
            }
            List<Card> cardsOnTable = table.GetAllCards();
            foreach (Card card in cardsOnTable)
            {
                this.AddCardToHand(card);
            }
            table.Clear();
            return new BotAction(null, false, false);
        }

        public BotAction? SelectCardToPlay(Table table, Card trumpCard)
        {
            if (table.IsEmpty())
            {
                return SelectCardToAttack(table, trumpCard);
            }
            else
            {
                return SelectCardToDefend(table, trumpCard);
            }
        }
    }
}




