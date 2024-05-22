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
        public List<Card> Hand { get; private set; }
        public Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
        }

        public void SetHand(List<Card> hand)
        {
            Hand = hand;
        }

        public void AddCardToHand(Card card)
        {
            Hand.Add(card);
        }

        public void RemoveCardFromHand(Card card)
        {
            Hand.Remove(card);
        }
    }
}
