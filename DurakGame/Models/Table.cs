using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Models
{
    public class Table
    {
        private List<Card> cardsOnTable;

        public Table()
        {
            cardsOnTable = new List<Card>();
        }

        public void AddCardToTable(Card card)
        {
            cardsOnTable.Add(card);
        }

        public void ClearTable()
        {
            cardsOnTable.Clear();
        }

        public bool HasCardsOnTable()
        {
            return cardsOnTable.Count > 0;
        }
    }
}
