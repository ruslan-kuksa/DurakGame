using DurakGame.Models;
using DurakGame.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            Game.AddPlayer("Player 1");
            Game.AddPlayer("Bot");
            Game.DealCards();
            UpdateTrumpCardImage();
            DisplayPlayerHand(Game.Players[0], Game.Players[1]);
            UpdateDeckCardCount();
            ((Button)sender).IsEnabled = false;
        }
        private void UpdateDeckCardCount()
        {
            DeckCounter.Text = Game.Deck.Count.ToString();
        }
        private void DisplayPlayerHand(Player player, Player opponent)
        {
            PlayerHandPanel.Children.Clear();
            OpponentHandPanel.Children.Clear();

            foreach (Card card in player.Hand)
            {
                CardControl cardControl = new CardControl
                {
                    Card = card,
                    Width = 125,
                    Height = 182,
                    Margin = new Thickness(2)
                };
                cardControl.MouseLeftButtonDown += CardControl_MouseLeftButtonDown;
                PlayerHandPanel.Children.Add(cardControl);
            }
            foreach (Card card in opponent.Hand)
            {
                EnemyCardControl enemyCardControl = new EnemyCardControl
                {
                    Card = card,
                    Width = 125,
                    Height = 182,
                    Margin = new Thickness(2)
                };
                OpponentHandPanel.Children.Add(enemyCardControl);
            }
        }
        private void CardControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is CardControl cardControl)
            {
                Card card = cardControl.Card;
                Game.Players[0].RemoveCardFromHand(card);
                Game.Table.AddAttackCard(card);
                AddCardToTable(card);
                DisplayPlayerHand(Game.Players[0], Game.Players[1]);
            }
        }
        private void AddCardToTable(Card card)
        {
            CardControl cardControl = new CardControl
            {
                Card = card,
                Width = 125,
                Height = 182,
                Margin = new Thickness(2)
            };

            Canvas.SetLeft(cardControl, 50 * Game.Table.AttackCards.Count - 350);
            Canvas.SetTop(cardControl, 0);

            TablePanel.Children.Add(cardControl);
        }
        private void UpdateTrumpCardImage()
        {
            Suit trumpSuit = Game.TrumpCard.Suit;
            string suitString = trumpSuit.ToString().ToLower();
            string rankString = Game.TrumpCard.Rank.ToString().ToLower();
            string imagePath = $"pack://application:,,,/Resources/{rankString}_of_{suitString}.png";
            TrumpCardImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));

        }
        private void CardControl_CardClicked(object sender, RoutedEventArgs e)
        {
            CardControl? cardControl = sender as CardControl;
            if (cardControl != null)
            {
                var card = cardControl.Card;
                if (card != null && Game.Table.AttackCards.Count < 6)
                { 
                    ThrowCard(card, Game.Players[0], Game.Players[1]);
                }
            }
        }
        private void ThrowCard(Card card, Player player, Player opponent)
        {
            Game.Table.AddAttackCard(card);
            player.Hand.Remove(card);
            DisplayPlayerHand(player, opponent);
            DisplayTable();
        }
        private void DisplayTable()
        {
            TablePanel.Children.Clear();
            double cardWidth = 125;
            double cardHeight = 182;
            double xOffset = cardWidth;
            for (int i = 0; i < Game.Table.AttackCards.Count; i++)
            {
                Card attackCard = Game.Table.AttackCards[i];
                CardControl attackCardControl = new CardControl
                {
                    Card = attackCard,
                    Width = cardWidth,
                    Height = cardHeight
                };
                Canvas.SetLeft(attackCardControl, i * xOffset);
                Canvas.SetTop(attackCardControl, 0);
                TablePanel.Children.Add(attackCardControl);
            }
        }
    }
}
