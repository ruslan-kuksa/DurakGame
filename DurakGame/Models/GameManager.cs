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
        public Deck Deck { get; private set; }

        public GameManager()
        {
            Players = new List<Player>();
            Deck = new Deck();
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
        }
    }
}
