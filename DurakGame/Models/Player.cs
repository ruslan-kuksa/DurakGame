using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DurakGame.Models
{
    public class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; }

        public Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
        }

        public void AddCardToHand(Card card)
        {
            Hand.Add(card);
        }

        public void RemoveCardFromHand(Card card)
        {
            Hand.Remove(card);
        }
        public bool HasCardsInHand()
        {
            return Hand.Count > 0;
        }
        public virtual bool IsHuman()
        {
            return false;
        }
    }
}
