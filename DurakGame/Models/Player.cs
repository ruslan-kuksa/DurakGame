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
        private string Name { get; set; }
        private List<Card> Hand { get; set; }
        public Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
        }
    }
}
