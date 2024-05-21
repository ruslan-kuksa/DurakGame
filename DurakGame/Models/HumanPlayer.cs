using DurakGame.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Models
{
    public class HumanPlayer : Player
    {
        private readonly IHumanStrategy _attackStrategy;
        private readonly IHumanStrategy _defenseStrategy;

        public HumanPlayer(string name, IHumanStrategy attackStrategy, IHumanStrategy defenseStrategy)
            : base(name)
        {
            _attackStrategy = attackStrategy;
            _defenseStrategy = defenseStrategy;
        }

        public bool PlayCard(Card card, Table table, Card trumpCard, out string errorMessage)
        {
            bool isAttacking = table.DefenseCards.Count == table.AttackCards.Count;
            if (isAttacking)
            {
                return _attackStrategy.PlayCard(this, card, table, trumpCard, out errorMessage);
            }
            else
            {
                return _defenseStrategy.PlayCard(this, card, table, trumpCard, out errorMessage);
            }
        }
    }
}
