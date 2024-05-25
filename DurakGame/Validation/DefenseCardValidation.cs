using DurakGame.Messages;
using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Validation
{
    public class DefenseCardValidation : IValidationStrategy
    {
        public void Validate(Player player, Card card, Table table, Card trumpCard)
        {
            Card lastAttackCard = table.AttackCards.LastOrDefault();
            if (lastAttackCard == null || !card.CanBeat(lastAttackCard, trumpCard.Suit))
            {
                throw new GameValidationException(GameNotification.CannotBeatAttackCardMessage);
            }
        }
    }
}
