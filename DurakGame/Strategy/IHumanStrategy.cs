using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Strategy
{
    public interface IHumanStrategy
    {
        bool PlayCardStrategy(Player player, Card card, Table table, Card trumpCard, out string errorMessage);
    }
}
