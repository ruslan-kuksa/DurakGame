using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Validation
{
    public abstract class BaseHandler : IValidationHandler
    {
        private IValidationHandler _nextHandler;

        public IValidationHandler SetNext(IValidationHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual bool Handle(Player player, Card card, Table table, Card trumpCard, out string errorMessage)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Handle(player, card, table, trumpCard, out errorMessage);
            }
            else
            {
                errorMessage = null;
                return true;
            }
        }
    }
}
