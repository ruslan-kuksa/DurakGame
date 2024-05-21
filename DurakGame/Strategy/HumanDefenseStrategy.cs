using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Strategy
{
    public class HumanDefenseStrategy : IHumanStrategy
    {
        public bool PlayCard(Player player, Card card, Table table, Card trumpCard, out string errorMessage)
        {
            errorMessage = string.Empty;
            Card lastAttackCard = table.AttackCards.LastOrDefault();
            if (lastAttackCard == null || !card.CanBeat(lastAttackCard, trumpCard.Suit))
            {
                errorMessage = "Ця карта не може побити атакуючу карту";
                return false;
            }

            player.RemoveCardFromHand(card);
            table.AddDefenseCard(card);
            return true;
        }
    }
}
