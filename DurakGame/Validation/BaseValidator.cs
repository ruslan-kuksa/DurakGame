using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Validation
{
    public class BaseValidator
    {
        private IValidationStrategy? _strategy;

        public void SetValidationStrategy(IValidationStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Validate(Player player, Card card, Table table, Card trumpCard)
        {
            _strategy?.Validate(player, card, table, trumpCard);
        }
    }
}
