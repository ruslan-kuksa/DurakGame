using DurakGame.Messages;
using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Hints
{
    public abstract class HintHandler : IHintHandler
    {
        private IHintHandler _nextHandler;

        public IHintHandler SetNext(IHintHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual string Handle(Player player, Table table, Card trumpCard)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Handle(player, table, trumpCard);
            }
            else
            {
                return GameNotification.NoAvailableHintsMessage;
            }
        }
    }
}
