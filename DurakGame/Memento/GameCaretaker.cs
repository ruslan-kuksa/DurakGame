using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Memento
{
    public class GameCaretaker
    {
        private Stack<GameMemento> _mementos = new Stack<GameMemento>();

        public void Save(GameMemento memento)
        {
            _mementos.Push(memento);
        }

        public GameMemento Undo()
        {
            if (_mementos.Count > 0)
            {
                return _mementos.Pop();
            }
            return null;
        }
    }
}
