using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Strategy
{
    public class BotDefenseStrategy : IBotStrategy
    {
        public BotAction SelectCardStrategy(Player bot, Table table, Card trumpCard)
        {
            if (table.AttackCards.Count > table.DefenseCards.Count)
            {
                Card cardToBeat = table.AttackCards[table.DefenseCards.Count];
                var sameSuitCards = bot.Hand
                    .Where(card => card.Suit == cardToBeat.Suit && card.Rank > cardToBeat.Rank)
                    .OrderBy(card => Convert.ToInt32(card.Rank))
                    .ToList();

                if (sameSuitCards.Any(card => card.CanBeat(cardToBeat, trumpCard.Suit)))
                {
                    return new BotAction(sameSuitCards.First(), true, false);
                }
                var trumpCards = bot.Hand
                    .Where(card => card.Suit == trumpCard.Suit)
                    .OrderBy(card => Convert.ToInt32(card.Rank))
                    .ToList();

                if (trumpCards.Any(card => card.CanBeat(cardToBeat, trumpCard.Suit)))
                {
                    return new BotAction(trumpCards.First(), true, false);
                }
            }
            return new BotAction(null, false, false);
        }
    }
}
