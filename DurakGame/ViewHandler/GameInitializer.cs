using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.ViewHandler
{
    public class GameInitializer
    {
        private readonly MainGamePage _mainGamePage;

        public GameInitializer(MainGamePage mainGamePage)
        {
            _mainGamePage = mainGamePage;
        }


        public void GameInitialize()
        {
            _mainGamePage.UIBackgroundManager.SetBackgroundImage();
            _mainGamePage.UIDeckManager.InitializeDeck();
            _mainGamePage.InitializeGameManager();
        }
    }
}
