using DurakGame.Messages;
using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Validation
{
    public class AttackCardHandler : BaseHandler
    {
        public override bool Handle(Player player, Card card, Table table, Card trumpCard, out string errorMessage)
        {
            if (!table.CanAddAttackCard(card))
            {
                errorMessage = GameNotification.CannotAddAttackCardMessage;
                return false;
            }
            else
            {
                return base.Handle(player, card, table, trumpCard, out errorMessage);
            }
        }
    }
}
