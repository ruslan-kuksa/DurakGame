using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Memento
{
    public class GameMemento
    {
        public List<Card> PlayerHand { get; private set; }
        public List<Card> TableAttackCards { get; private set; }
        public List<Card> TableDefenseCards { get; private set; }
        public Player ActivePlayer { get; private set; }

        public GameMemento(List<Card> playerHand, List<Card> tableAttackCards, List<Card> tableDefenseCards, Player activePlayer)
        {
            PlayerHand = new List<Card>(playerHand);
            TableAttackCards = new List<Card>(tableAttackCards);
            TableDefenseCards = new List<Card>(tableDefenseCards);
            ActivePlayer = activePlayer;
        }
    }
}
