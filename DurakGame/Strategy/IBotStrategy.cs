using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Strategy
{
    public interface IBotStrategy
    {
        BotAction SelectCard(Player player, Table table, Card trumpCard);
    }
}
