using DurakGame.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Models
{
    public struct BotAction
    {
        public Card Card;
        public bool IsDefending;
        public bool IsAttacking;
        public bool IsPassing;

        public BotAction(Card card, bool isDefending, bool isAttacking, bool isPassing = false)
        {
            Card = card;
            IsDefending = isDefending;
            IsAttacking = isAttacking;
            IsPassing = isPassing;
        }
    }

    public class BotPlayer : Player
    {
        private readonly IBotStrategy _attackStrategy;
        private readonly IBotStrategy _defenseStrategy;

        public BotPlayer(string name, IBotStrategy attackStrategy, IBotStrategy defenseStrategy) : base(name)
        {
            _attackStrategy = attackStrategy;
            _defenseStrategy = defenseStrategy;
        }

        public BotAction SelectCardToPlay(Table table, Card trumpCard)
        {
            bool isAttacking = table.DefenseCards.Count == table.AttackCards.Count;
            if (isAttacking)
            {
                return _attackStrategy.SelectCardStrategy(this, table, trumpCard);
            }
            else
            {
                return _defenseStrategy.SelectCardStrategy(this, table, trumpCard);
            }
        }
    }
}