using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Strategy
{
    public class BotAttackStrategy : IBotStrategy
    {
        public BotAction SelectCardStrategy(Player bot, Table table, Card trumpCard)
        {
            Card chosenCard;
            if (table.IsEmpty())
            {
                chosenCard = ChooseCardForEmptyTable(bot, trumpCard);
            }
            else
            {
                chosenCard = ChooseCardForNonEmptyTable(bot, table);
            }

            if (chosenCard == null)
            {
                return new BotAction(null, false, false, true);
            }
            return new BotAction(chosenCard, false, true);
        }

        private Card ChooseCardForEmptyTable(Player bot, Card trumpCard)
        {
            Card nonTrumpCard = bot.Hand
                .Where(card => card.Suit != trumpCard.Suit)
                .OrderBy(card => Convert.ToInt32(card.Rank))
                .FirstOrDefault();

            if (nonTrumpCard != null)
            {
                return nonTrumpCard;
            }

            return bot.Hand
                .Where(card => card.Suit == trumpCard.Suit)
                .OrderBy(card => Convert.ToInt32(card.Rank))
                .FirstOrDefault();
        }

        private Card ChooseCardForNonEmptyTable(Player bot, Table table)
        {
            List<Rank> existingRanks = table.AttackCards.Concat(table.DefenseCards)
                .Select(card => card.Rank)
                .Distinct()
                .ToList();

            return bot.Hand
                .Where(card => existingRanks.Contains(card.Rank) && table.CanAddAttackCard(card))
                .OrderBy(card => Convert.ToInt32(card.Rank))
                .FirstOrDefault();
        }
    }
}
