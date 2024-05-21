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
        public BotAction SelectCard(Player bot, Table table, Card trumpCard)
        {
            Card chosenCard = null;

            if (table.IsEmpty())
            {
                chosenCard = bot.Hand
                    .Where(card => card.Suit != trumpCard.Suit)
                    .OrderBy(card => Convert.ToInt32(card.Rank))
                    .FirstOrDefault();

                if (chosenCard == null)
                {
                    chosenCard = bot.Hand
                        .Where(card => card.Suit == trumpCard.Suit)
                        .OrderBy(card => Convert.ToInt32(card.Rank))
                        .FirstOrDefault();
                }
            }
            else
            {
                List<Rank> existingRanks = table.AttackCards.Concat(table.DefenseCards).Select(card => card.Rank).Distinct().ToList();
                chosenCard = bot.Hand
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
    }
}
