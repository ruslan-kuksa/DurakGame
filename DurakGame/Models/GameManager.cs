using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DurakGame.Models
{
    public class GameManager
    {
        public List<Player> Players { get; private set; }
        public Player ActivePlayer { get; private set; }
        public Deck Deck { get; private set; }
        public Card TrumpCard { get; private set; }
        public Table Table { get; private set; }

        public GameManager()
        {
            Players = new List<Player>();
            Deck = new Deck();
            Table = new Table();
        }

        public void AddPlayer(string playerName)
        {
            Players.Add(new Player(playerName));
        }
        public void DealCards()
        {
            foreach (Player player in Players)
            {
                for (int i = 0; i < 6; i++)
                {
                    player.AddCardToHand(Deck.DrawCard());
                }
            }
            TrumpCard = Deck.DrawCard();
            ActivePlayer = FindLowestTrumpCard();
        }
        public Player FindLowestTrumpCard()
        {
            Player startingPlayer = null;
            int lowestTrumpValue = int.MaxValue;

            foreach (Player player in Players)
            {
                foreach (Card card in player.Hand)
                {
                    int cardRankValue = Convert.ToInt32(card.Rank);
                    if (card.Suit == TrumpCard.Suit && cardRankValue < lowestTrumpValue)
                    {
                        startingPlayer = player;
                        lowestTrumpValue = cardRankValue;
                    }
                }
            }

            return startingPlayer;
        }
        private void SwitchActivePlayer()
        {
            int activePlayerIndex = Players.IndexOf(ActivePlayer);
            int nextPlayerIndex = (activePlayerIndex + 1) % Players.Count;
            ActivePlayer = Players[nextPlayerIndex];
        }
        public void NextTurn()
        {
            SwitchActivePlayer();
        }
    }
}
