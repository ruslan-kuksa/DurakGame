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
            Game.AddPlayer(new HumanPlayer("Player"));
            Game.AddPlayer(new BotPlayer("Bot"));
            Game.DealCards();
            UpdateTrumpCardImage();
            DisplayPlayerHand(Game.Players[0], Game.Players[1]);
            UpdateDeckCardCount();
            Player firstPlayer = Game.FindLowestTrumpCard();
            if (firstPlayer != null)
            {
                FirstPlayerLabel.Content = $"{firstPlayer.Name} ходить першим";
            }
            else
            {
                FirstPlayerLabel.Content = "Немає гравців з козирними картами";
            }
            ((Button)sender).IsEnabled = false;
            UpdateButtons();
        }
        private void UpdateDeckCardCount()
        {
            DeckCounter.Content = Game.Deck.Count.ToString();
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

                if (Game.Players.Count > 0 && Game.Players[0] is HumanPlayer player)
                {
                    if (Game.Table.IsEmpty())
                    {
                        player.RemoveCardFromHand(card);
                        Game.Table.AddAttackCard(card);
                        AddCardToTable(card);
                        DisplayPlayerHand(player, Game.Players[1]);
                    }
                    else if (Game.Table.ContainsCardWithRank(card.Rank))
                    {
                        player.RemoveCardFromHand(card);
                        Game.Table.AddAttackCard(card);
                        AddCardToTable(card);
                        DisplayPlayerHand(player, Game.Players[1]);
                    }
                    else
                    {
                        MessageBox.Show("Ви можете підкинути лише карту того ж значення, що і на столі.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
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
            var playerAsHuman = player as HumanPlayer;
            bool canThrowCard = Game.Table.IsEmpty() || playerAsHuman.GetAllCardsOfRank(card.Rank).Count > 0;

            if (canThrowCard)
            {
                Game.Table.AddAttackCard(card);
                player.Hand.Remove(card);
                DisplayPlayerHand(player, opponent);
                DisplayTable();
            }
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
        private void TakeButton_Click(object sender, RoutedEventArgs e)
        {
            Player player = Game.Players[0];
            List<Card> cardsOnTable = Game.Table.AttackCards.Concat(Game.Table.DefenseCards).ToList();
            Game.TakeCards(player, cardsOnTable);
            DisplayPlayerHand(Game.Players[0], Game.Players[1]);
            DisplayTable();
            UpdateDeckCardCount();
        }

        private void BeatButton_Click(object sender, RoutedEventArgs e)
        {
            Game.EndTurn();
            DisplayPlayerHand(Game.Players[0], Game.Players[1]);
            DisplayTable();
            UpdateDeckCardCount();
        }
        private void UpdateButtons()
        {
            bool isPlayerTurn = Game.ActivePlayer.Name == "Player";
            BeatButton.IsEnabled = isPlayerTurn && Game.Table.AttackCards.Count > 0 && Game.Table.DefenseCards.Count > 0;
            TakeButton.IsEnabled = isPlayerTurn && Game.Table.AttackCards.Count > 0;
        }
    }
}
