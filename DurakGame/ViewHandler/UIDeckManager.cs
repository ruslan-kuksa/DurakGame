using DurakGame.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

namespace DurakGame.ViewHandler
{
    public class UIDeckManager
    {
        private readonly MainGamePage _mainGamePage;

        public UIDeckManager(MainGamePage mainGamePage)
        {
            _mainGamePage = mainGamePage;
        }

        public void InitializeDeck()
        {
            for (int i = 0; i < 36; i++)
            {
                AddCardToDeck(i);
            }
        }

        public void AddCardToDeck(int i)
        {
            Image cardImage = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/card_back.png")),
                Width = GameConstants.CardWidth,
                Height = GameConstants.CardHeight,
            };
            _mainGamePage.DeckImage.Children.Add(cardImage);
            Canvas.SetTop(cardImage, i * GameConstants.DeckCardOffset);
            Canvas.SetLeft(cardImage, 5 + i * GameConstants.DeckCardLeftOffset);
        }

        public void SetDeckVisibility(int deckCount)
        {
            var visibility = deckCount == 0 ? Visibility.Hidden : Visibility.Visible;
            _mainGamePage.DeckImage.Visibility = visibility;
            _mainGamePage.TrumpCardImage.Visibility = visibility;
            _mainGamePage.DeckCounter.Visibility = visibility;
        }
    }
}
