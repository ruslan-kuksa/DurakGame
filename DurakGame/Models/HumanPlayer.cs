using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Models
{
    public class HumanPlayer : Player
    {
        public HumanPlayer(string name) : base(name)
        {

        }
        public override bool IsHuman()
        {
            return true;
        }
        public bool CanAttackWithCards(List<Card> cards)
        {
            if (cards.Count == 0)
            {
                return false;
            }

            Rank rank = cards[0].Rank;
            return cards.All(card => card.Rank == rank);
        }
        public List<Card> GetAllCardsOfRank(Rank rank)
        {
            return Hand.Where(card => card.Rank == rank).ToList();
        }
    }
}
