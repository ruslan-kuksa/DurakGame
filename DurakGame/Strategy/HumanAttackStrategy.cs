using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Strategy
{
    public class HumanAttackStrategy : IHumanStrategy
    {
        public bool PlayCard(Player player, Card card, Table table, Card trumpCard, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!table.CanAddAttackCard(card))
            {
                errorMessage = "Цю карту не можливо підкинути";
                return false;
            }

            player.RemoveCardFromHand(card);
            table.AddAttackCard(card);
            return true;
        }
    }
}
