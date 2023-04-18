using DurakGame.Models;
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
        private GameManager Game;
        public MainWindow()
        {
            InitializeComponent();
            Game = new GameManager();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            // Добавить игроков и раздать карты
            Game.AddPlayer("Player 1");
            Game.DealCards();

            DisplayPlayerHand(Game.Players[0]);
        }
        private void DisplayPlayerHand(Player player)
        {
            PlayerHandPanel.Children.Clear();

            foreach (Card card in player.Hand)
            {
                Button cardButton = new Button
                {
                    Content = $"{card.Rank} of {card.Suit}",
                    Width = 100,
                    Height = 150,
                    Margin = new Thickness(5)
                };

                PlayerHandPanel.Children.Add(cardButton);
            }
        }
    }
}
