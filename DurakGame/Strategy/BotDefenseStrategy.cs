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
            if (table.AttackCards.Count <= table.DefenseCards.Count)
            {
                return new BotAction(null, false, false);
            }

            Card cardToBeat = table.AttackCards[table.DefenseCards.Count];
            Card chosenCard = ChooseSameSuitCard(bot, cardToBeat, trumpCard);

            if (chosenCard != null)
            {
                return new BotAction(chosenCard, true, false);
            }

            chosenCard = ChooseTrumpCard(bot, cardToBeat, trumpCard);

            if (chosenCard != null)
            {
                return new BotAction(chosenCard, true, false);
            }

            return new BotAction(null, false, false);
        }

        private Card ChooseSameSuitCard(Player bot, Card cardToBeat, Card trumpCard)
        {
            return bot.Hand
                .Where(card => card.Suit == cardToBeat.Suit && card.Rank > cardToBeat.Rank)
                .OrderBy(card => Convert.ToInt32(card.Rank))
                .FirstOrDefault(card => card.CanBeat(cardToBeat, trumpCard.Suit));
        }

        private Card ChooseTrumpCard(Player bot, Card cardToBeat, Card trumpCard)
        {
            return bot.Hand
                .Where(card => card.Suit == trumpCard.Suit)
                .OrderBy(card => Convert.ToInt32(card.Rank))
                .FirstOrDefault(card => card.CanBeat(cardToBeat, trumpCard.Suit));
        }
    }
}
