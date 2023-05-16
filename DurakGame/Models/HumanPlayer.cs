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
        public List<Card> GetAllCardsOfRank(Rank rank)
        {
            return Hand.Where(card => card.Rank == rank).ToList();
        }
        public bool PlayCard(Card card, Table table, Card trumpCard, out string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            if (Hand.Contains(card))
            {
                bool isAttacking = table.DefenseCards.Count == table.AttackCards.Count;

                if (isAttacking)
                {
                    if (table.CanAddAttackCard(card))
                    {
                        RemoveCardFromHand(card);
                        table.AddAttackCard(card);
                        return true;
                    }
                    else
                    {
                        ErrorMessage = "Цю карту не можливо підкинути";
                    }
                }
                else
                {
                    Card lastAttackCard = table.AttackCards.LastOrDefault();
                    if (lastAttackCard != null && card.CanBeat(lastAttackCard, trumpCard.Suit))
                    {
                        RemoveCardFromHand(card);
                        table.AddDefenseCard(card);
                        return true;
                    }
                    else
                    {
                        ErrorMessage = "Ця карта не може побити атакуючу карту";
                    }
                }
            }
            return false;
        }
    }
}
