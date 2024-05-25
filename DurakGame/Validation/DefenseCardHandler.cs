using DurakGame.Messages;
using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Validation
{
    public class DefenseCardHandler : BaseHandler
    {
        public override bool Handle(Player player, Card card, Table table, Card trumpCard, out string errorMessage)
        {
            Card lastAttackCard = table.AttackCards.LastOrDefault();

            if (lastAttackCard == null || !card.CanBeat(lastAttackCard, trumpCard.Suit))
            {
                errorMessage = GameNotification.CannotBeatAttackCardMessage;
                return false;
            }
            else
            {
                return base.Handle(player, card, table, trumpCard, out errorMessage);
            }
        }
    }
}
