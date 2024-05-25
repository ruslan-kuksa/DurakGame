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
    public class HumanAttackStrategy : IHumanStrategy
    {
        private readonly BaseValidator _validator;

        public HumanAttackStrategy(BaseValidator validator)
        {
            _validator = validator;
        }

        public bool PlayCardStrategy(Player player, Card card, Table table, Card trumpCard, out string errorMessage)
        {
            try
            {
                _validator.SetValidationStrategy(new AttackCardValidation());
                _validator.Validate(player, card, table, trumpCard);
                player.RemoveCardFromHand(card);
                table.AddAttackCard(card);
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
