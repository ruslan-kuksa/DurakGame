using DurakGame.Messages;
using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Validation
{
    public class AttackCardValidation : IValidationStrategy
    {
        public void Validate(Player player, Card card, Table table, Card trumpCard)
        {
            if (!table.CanAddAttackCard(card))
            {
                throw new GameValidationException(GameNotification.CannotAddAttackCardMessage);
            }
        }
    }
}
