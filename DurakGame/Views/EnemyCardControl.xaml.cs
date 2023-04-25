using DurakGame.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DurakGame.Views
{
    public partial class EnemyCardControl : UserControl
    {
        public static readonly DependencyProperty CardProperty = DependencyProperty.Register(
        "Card", typeof(Card), typeof(EnemyCardControl), new PropertyMetadata(null, OnCardChanged));

        public static readonly DependencyProperty CardImageProperty = DependencyProperty.Register(
        "CardImage", typeof(ImageSource), typeof(EnemyCardControl), new PropertyMetadata(null));

        public ImageSource CardImage
        {
            get { return (ImageSource)GetValue(CardImageProperty); }
            set { SetValue(CardImageProperty, value); }
        }

        public Card Card
        {
            get { return (Card)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }

        public EnemyCardControl()
        {
            InitializeComponent();
            CardImage = new BitmapImage(new Uri("/Resources/card_back.png", UriKind.Relative));
        }

        private void EnemyCardControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ToggleCardVisibility();
        }

        private static void OnCardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EnemyCardControl enemyCardControl = (EnemyCardControl)d;
            enemyCardControl.SetCardBackImage();
        }

        private void SetCardBackImage()
        {
            string cardBackImagePath = "/Resources/card_back.png";
            BitmapImage bitmapImage = new BitmapImage(new Uri(cardBackImagePath, UriKind.Relative));
            CardImageControl.Source = bitmapImage;
        }

        private void ToggleCardVisibility()
        {
            string cardBackImagePath = "/Resources/card_back.png";
            string cardFrontImagePath = $"/Resources/{Card.Rank.ToString().ToLowerInvariant()}_of_{Card.Suit.ToString().ToLowerInvariant()}.png";

            if (((BitmapImage)CardImage).UriSource.OriginalString == cardBackImagePath)
            {
                CardImageControl.Source = new BitmapImage(new Uri(cardFrontImagePath, UriKind.Relative));
            }
            else
            {
                CardImageControl.Source = new BitmapImage(new Uri(cardBackImagePath, UriKind.Relative));
            }
        }
    }
}
