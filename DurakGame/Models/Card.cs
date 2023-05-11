using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Models
{
    public class Card
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }
        public Card(Suit suit, Rank rank) 
        { 
            Suit = suit;
            Rank = rank;
        }
        public bool CanBeat(Card other, Suit trumpSuit)
        {
            if (Suit == other.Suit)
            {
                return Rank > other.Rank;
            }
            else
            {
                return Suit == trumpSuit && Rank > other.Rank;
            }
        }
    }
}
