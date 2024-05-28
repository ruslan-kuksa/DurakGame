using DurakGame.Messages;
using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Hints
{
    public class DefenseHintHandler : HintHandler
    {
        public override string Handle(Player player, Table table, Card trumpCard)
        {
            if (table.AttackCards.Count > table.DefenseCards.Count)
            {
                Card cardToBeat = table.AttackCards.LastOrDefault();
                if (cardToBeat == null)
                    return GameNotification.NoDefenseCardsMessage;

                List<Card> availableCards = player.Hand
                    .Where(card => card.CanBeat(cardToBeat, trumpCard.Suit))
                    .OrderBy(card => card.Rank)
                    .ToList();

                if (availableCards.Any())
                {
                    Card bestCard = availableCards.First();
                    return string.Format(GameNotification.RecommendDefenseMessage, bestCard.Rank, bestCard.Suit);
                }
            }

            return base.Handle(player, table, trumpCard);
        }
    }
}
