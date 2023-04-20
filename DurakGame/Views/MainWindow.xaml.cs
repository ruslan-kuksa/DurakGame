using DurakGame.Models;
using DurakGame.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace DurakGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Card _selectedCard;
        private GameManager Game;
        public MainWindow()
        {
            InitializeComponent();
            Game = new GameManager();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            Game.AddPlayer("Player 1");
            Game.DealCards();
            UpdateTrumpCardImage();
            DisplayPlayerHand(Game.Players[0]);
            ((Button)sender).IsEnabled = false;
        }
        private void DisplayPlayerHand(Player player)
        {
            PlayerHandPanel.Children.Clear();

            foreach (Card card in player.Hand)
            {
                CardControl cardControl = new CardControl
                {
                    Card = card,
                    Width = 125,
                    Height = 182,
                    Margin = new Thickness(2)
                };
                PlayerHandPanel.Children.Add(cardControl);
            }
        }
        private void UpdateTrumpCardImage()
        {
            Suit trumpSuit = Game.TrumpCard.Suit;
            string suitString = trumpSuit.ToString().ToLower();
            string rankString = Game.TrumpCard.Rank.ToString().ToLower();
            string imagePath = $"pack://application:,,,/Resources/{rankString}_of_{suitString}.png";
            TrumpCardImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));

        }
        /*private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            Button cardButton = sender as Button;
            _selectedCard = cardButton.Tag as Card;
        }
        private Card GetSelectedCardFromUI()
        {
            return _selectedCard;
        }*/
    }
}
