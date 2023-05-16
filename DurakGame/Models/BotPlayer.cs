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
        public bool IsPassing;

        public BotAction(Card card, bool isDefending, bool isAttacking, bool isPassing = false)
        {
            Card = card;
            IsDefending = isDefending;
            IsAttacking = isAttacking;
            IsPassing = isPassing;
        }
    }

    public class BotPlayer : Player
    {
        public BotPlayer(string name) : base(name)
        {

        }
        public BotAction? SelectCardToAttack(Table table, Card trumpCard)
        {
            Card chosenCard = null;

            if (table.IsEmpty())
            {
                chosenCard = Hand
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
            }
            else
            {
                List<Rank> existingRanks = table.AttackCards.Concat(table.DefenseCards).Select(card => card.Rank).Distinct().ToList();
                chosenCard = Hand
                    .Where(card => existingRanks.Contains(card.Rank) && table.CanAddAttackCard(card))
                    .OrderBy(card => Convert.ToInt32(card.Rank))
                    .FirstOrDefault();
            }
            if (chosenCard == null)
            {
                return new BotAction(null, false, false, true);
            }
            return new BotAction(chosenCard, false, true);
        }
        public BotAction? SelectCardToDefend(Table table, Card trumpCard)
        {
            if (table.IsEmpty())
            {
                return SelectCardToAttack(table, trumpCard);
            }
            else
            {
                if (table.AttackCards.Count > table.DefenseCards.Count)
                {
                    Card cardToBeat = table.AttackCards[table.DefenseCards.Count];
                    var sameSuitCards = Hand
                        .Where(card => card.Suit == cardToBeat.Suit && card.Rank > cardToBeat.Rank)
                        .OrderBy(card => Convert.ToInt32(card.Rank))
                        .ToList();

                    if (sameSuitCards.Any(card => card.CanBeat(cardToBeat, trumpCard.Suit)))
                    {
                        return new BotAction(sameSuitCards.First(), true, false);
                    }
                    var trumpCards = Hand
                        .Where(card => card.Suit == trumpCard.Suit)
                        .OrderBy(card => Convert.ToInt32(card.Rank))
                        .ToList();

                    if (trumpCards.Any(card => card.CanBeat(cardToBeat, trumpCard.Suit)))
                    {
                        return new BotAction(trumpCards.First(), true, false);
                    }
                }
            }
            return new BotAction(null, false, false);
        }
        public BotAction? SelectCardToPlay(Table table, Card trumpCard)
        {
            if (table.AllAttackCardsDefended())
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