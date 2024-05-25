using DurakGame.Models;
using DurakGame.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Strategy
{
    public class HumanDefenseStrategy : IHumanStrategy
    {
        private readonly BaseValidator _validator;

        public HumanDefenseStrategy(BaseValidator validator)
        {
            _validator = validator;
        }

        public bool PlayCardStrategy(Player player, Card card, Table table, Card trumpCard, out string errorMessage)
        {
            try
            {
                _validator.SetValidationStrategy(new DefenseCardValidation());
                _validator.Validate(player, card, table, trumpCard);
                player.RemoveCardFromHand(card);
                table.AddDefenseCard(card);
                errorMessage = string.Empty;
                return true;
            }
            catch (GameValidationException ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
