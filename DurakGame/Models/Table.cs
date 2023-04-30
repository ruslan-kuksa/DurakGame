using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Models
{
    public class Table
    {
        public List<Card> AttackCards { get; private set; }
        public List<Card> DefenseCards { get; private set; }

        public Table()
        {
            AttackCards = new List<Card>();
            DefenseCards = new List<Card>();
        }

        public void AddAttackCard(Card card)
        {
            AttackCards.Add(card);
        }

        public void AddDefenseCard(Card card)
        {
            DefenseCards.Add(card);
        }

        public void Clear()
        {
            AttackCards.Clear();
            DefenseCards.Clear();
        }

        public bool IsEmpty()
        {
            return AttackCards.Count == 0 && DefenseCards.Count == 0;
        }
        public List<Card> GetAllCards()
        {
            return AttackCards.Concat(DefenseCards).ToList();
        }
    }
}

