using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Models
{
    public class HumanPlayer : Player
    {
        public HumanPlayer(string name) : base(name)
        {

        }
        public bool PlayAttackCard(Card card, Table table, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!table.CanAddAttackCard(card))
            {
                errorMessage = "Цю карту не можливо підкинути";
                return false;
            }

            RemoveCardFromHand(card);
            table.AddAttackCard(card);
            return true;
        }
        public bool PlayDefenseCard(Card card, Table table, Card trumpCard, out string errorMessage)
        {
            errorMessage = string.Empty;
            Card lastAttackCard = table.AttackCards.LastOrDefault();
            if (lastAttackCard == null || !card.CanBeat(lastAttackCard, trumpCard.Suit))
            {
                errorMessage = "Ця карта не може побити атакуючу карту";
                return false;
            }

            RemoveCardFromHand(card);
            table.AddDefenseCard(card);
            return true;
        }
        public bool PlayCard(Card card, Table table, Card trumpCard, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!Hand.Contains(card))
            {
                return false;
            }

            bool isAttacking = table.DefenseCards.Count == table.AttackCards.Count;
            if (isAttacking)
            {
                return PlayAttackCard(card, table, out errorMessage);
            }
            else
            {
                return PlayDefenseCard(card, table, trumpCard, out errorMessage);
            }
        }
    }
}
