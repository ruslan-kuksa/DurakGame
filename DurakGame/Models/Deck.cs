﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Models
{
    public class Deck
    {
        private List<Card> cards { get; set; }
        public Deck()
        {
            cards = new List<Card>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    cards.Add(new Card (suit, rank ));
                }
            }
            Shuffle();
        }
        public void Shuffle()
        {
            Random random = new Random();
            for (int i = 0; i < cards.Count; i++)
            {
                int j = random.Next(cards.Count);
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }
        public Card DrawCard()
        {
            if (cards.Count == 0) 
                return null;
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
        public int Count
        {
            get { return cards.Count; }
        }
    }
}
