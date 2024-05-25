using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Validation
{
    public interface IValidationHandler
    {
        IValidationHandler SetNext(IValidationHandler handler);
        bool Handle(Player player, Card card, Table table, Card trumpCard, out string errorMessage);
    }
}
