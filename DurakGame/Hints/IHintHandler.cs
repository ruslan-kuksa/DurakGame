using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Hints
{
    public interface IHintHandler
    {
        IHintHandler SetNext(IHintHandler handler);

        string Handle(Player player, Table table, Card trumpCard);
    }
}
