using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace DurakGame.ViewHandler
{
    public class UIBackgroundManager
    {
        private readonly MainGamePage _mainGamePage;

        public UIBackgroundManager(MainGamePage mainGamePage)
        {
            _mainGamePage = mainGamePage;
        }

        public void SetBackgroundImage()
        {
            BitmapImage newImageSource = new BitmapImage(new Uri(App.BackgroundImagePath.ToString(), UriKind.RelativeOrAbsolute));
            ImageBrush newImageBrush = new ImageBrush(newImageSource);
            _mainGamePage.BackGrid.Background = newImageBrush;
        }
    }
}
