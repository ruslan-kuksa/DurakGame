using DurakGame.Models;
using DurakGame.Strategy;
using DurakGame.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace DurakGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainGamePage : Page
    {
        private GameManager Game;
        public MainGamePage()
        {
            InitializeComponent();
            var newImageSource = new BitmapImage(new Uri(App.BackgroundImagePath.ToString(), UriKind.RelativeOrAbsolute));
            var newImageBrush = new ImageBrush(newImageSource);
            BackGrid.Background = newImageBrush;
            for (int i = 0; i < 36; i++)
            {
                AddCardToDeck(i);
            }
            Game = new GameManager();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            Game.AddPlayer(new HumanPlayer("Player", new HumanAttackStrategy(), new HumanDefenseStrategy()));
            Game.AddPlayer(new BotPlayer("Bot", new BotAttackStrategy(), new BotDefenseStrategy()));
            Game.StartGame();
            UpdateTrumpCardImage();
            DisplayPlayerHand(Game.Players[0]);
            DisplayOpponentHand(Game.Players[1]);
            UpdateDeckCardCount();
            Player firstPlayer = Game.FindLowestTrumpCard();
            if (firstPlayer != null)
            {
                FirstPlayerLabel.Content = $"{firstPlayer.Name} ходить першим";
                if (firstPlayer is BotPlayer)
                {
                    BotPlay();
                }
            }
            else
            {
                FirstPlayerLabel.Content = "Немає гравців з козирними картами";
            }
            ((Button)sender).IsEnabled = false;
        }
        private void UpdateDeckCardCount()
        {
            int deckCount = Game.Deck.Count;
            if (!Game.Deck.IsTrumpCardTaken)
            {
                deckCount++;
            }
            DeckCounter.Content = deckCount.ToString();
            if (deckCount == 0)
            {
                DeckImage.Visibility = Visibility.Hidden;
                TrumpCardImage.Visibility = Visibility.Hidden;
                DeckCounter.Visibility = Visibility.Hidden;
            }
        }
        private void DisplayOpponentHand(Player opponent)
        {
            OpponentHandPanel.Children.Clear();
            double CardMargin = 2;
            if (opponent.Hand.Count >= 30)
                CardMargin = -100;
            else if (opponent.Hand.Count >= 24)
                CardMargin = -75;
            else if (opponent.Hand.Count >= 12)
                CardMargin = -50;
            foreach (Card card in opponent.Hand)
            {
                EnemyCardControl enemyCardControl = new EnemyCardControl
                {
                    Card = card,
                    Width = 125,
                    Height = 182,
                    Margin = new Thickness(CardMargin, 0, 0, 0)
                };

                OpponentHandPanel.Children.Add(enemyCardControl);
            }
        }

        private void DisplayPlayerHand(Player player)
        {
            PlayerHandPanel.Children.Clear();
            double CardMargin = 2;
            if (player.Hand.Count >= 30)
                CardMargin = -100;
            else if (player.Hand.Count >= 24)
                CardMargin = -75;
            else if (player.Hand.Count >= 12)
                CardMargin = -50;

            foreach (Card card in player.Hand)
            {
                CardControl cardControl = new CardControl
                {
                    Card = card,
                    Width = 125,
                    Height = 182,
                    Margin = new Thickness(CardMargin, 0, 0, 0)
                };

                cardControl.MouseLeftButtonDown += CardControl_MouseLeftButtonDown;
                PlayerHandPanel.Children.Add(cardControl);
            }
        }
        private void CardControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Game.ActivePlayer is HumanPlayer && sender is CardControl cardControl)
            {
                Card card = cardControl.Card;

                if (Game.Players.Count > 0 && Game.Players[0] is HumanPlayer player && Game.Players[1] is BotPlayer bot)
                {
                    if (player.PlayCard(card, Game.Table, Game.TrumpCard, out string errorMessage))
                    {
                        AddCardToTable(card, Game.Table.DefenseCards.Contains(card));
                        DisplayPlayerHand(player);
                        DisplayOpponentHand(bot);
                        ErrorMessage.Text = "";
                        Game.NextTurn();
                        CheckAndDisplayWinner();
                        if (Game.ActivePlayer is BotPlayer)
                        {
                            BotPlay();
                        }
                    }
                    else
                    {
                        ErrorMessage.Text = errorMessage;
                    }
                }
            }
            else
            {
                ErrorMessage.Text = "Зараз хід бота, зачекайте своєї черги";
            }
        }

        private void AddCardToTable(Card card, bool isDefending)
        {
            CardControl cardControl = new CardControl
            {
                Card = card,
                Width = 125,
                Height = 182,
                Margin = new Thickness(2)
            };

            int position = isDefending ? Game.Table.DefenseCards.Count - 1 : Game.Table.AttackCards.Count - 1;
            Canvas.SetLeft(cardControl, 135 * (position + 1) - 350);
            Canvas.SetTop(cardControl, isDefending ? 0 : -50);

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

        private void DisplayTable()
        {
            TablePanel.Children.Clear();
        }
        private void TakeButton_Click(object sender, RoutedEventArgs e)
        {
            Player player = Game.Players[0];
            List<Card> cardsOnTable = Game.Table.AttackCards.Concat(Game.Table.DefenseCards).ToList();
            Game.TakeCards(player, cardsOnTable);
            DisplayPlayerHand(Game.Players[0]);
            DisplayOpponentHand(Game.Players[1]);
            DisplayTable();
            UpdateDeckCardCount();
            Game.DealCards();
            if (Game.ActivePlayer is BotPlayer)
            {
                BotPlay();
            }
        }

        private void BeatButton_Click(object sender, RoutedEventArgs e)
        {
            Game.EndTurn();
            DisplayPlayerHand(Game.Players[0]);
            DisplayOpponentHand(Game.Players[1]);
            DisplayTable();
            UpdateDeckCardCount();
            Game.DealCards();
            if (Game.ActivePlayer is BotPlayer)
            {
                BotPlay();
            }
        }
        private async void BotPlay()
        {
            await Task.Delay(2000);
            if (Game.ActivePlayer is BotPlayer bot)
            {
                BotAction? action = bot.SelectCardToPlay(Game.Table, Game.TrumpCard);

                if (action.HasValue)
                {
                    if (action.Value.IsPassing)
                    {
                        Game.Table.Clear();
                        Game.EndTurn();
                        DisplayPlayerHand(Game.Players[0]);
                        DisplayOpponentHand(Game.Players[1]);
                        DisplayTable();
                        UpdateDeckCardCount();
                        Game.DealCards();
                        if (Game.ActivePlayer is BotPlayer)
                        {
                            BotPlay();
                        }
                        return;
                    }

                    Card cardToPlay = action.Value.Card;

                    if (cardToPlay != null)
                    {
                        bot.RemoveCardFromHand(cardToPlay);

                        if (action.Value.IsDefending)
                        {
                            BeatButton.Visibility = Visibility.Visible;
                            BeatButton.IsEnabled = true;
                            TakeButton.Visibility = Visibility.Hidden;
                            Game.Table.AddDefenseCard(cardToPlay);
                        }
                        else
                        {
                            BeatButton.Visibility = Visibility.Hidden;
                            TakeButton.Visibility = Visibility.Visible;
                            TakeButton.IsEnabled = true;
                            Game.Table.AddAttackCard(cardToPlay);
                        }

                        AddCardToTable(cardToPlay, action.Value.IsDefending);
                        DisplayPlayerHand(Game.Players[0]);
                        DisplayOpponentHand(Game.Players[1]);
                    }
                    else
                    {
                        List<Card> cardsOnTable = Game.Table.AttackCards.Concat(Game.Table.DefenseCards).ToList();
                        Game.TakeCards(bot, cardsOnTable);
                        DisplayPlayerHand(Game.Players[0]);
                        DisplayOpponentHand(Game.Players[1]);
                        DisplayTable();
                        UpdateDeckCardCount();
                        Game.DealCards();
                        return;
                    }
                    Game.NextTurn();
                    CheckAndDisplayWinner();
                }
            }
        }
        private void AddCardToDeck(int i)
        {
            var cardImage = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/card_back.png")),
                Width = 125,
                Height = 182,
            };
            DeckImage.Children.Add(cardImage);
            Canvas.SetTop(cardImage, i * 0.1);
            Canvas.SetLeft(cardImage, 5 + i * 0.5);
        }
        private void CheckAndDisplayWinner()
        {
            Player winner = Game.CheckWinner();
            if (winner != null)
            {
                MessageBox.Show($"{winner.Name} is the winner!");
                MessageBoxResult result = MessageBox.Show("Do you want to play again?", "Play Again", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ResetGame();
                        break;
                    case MessageBoxResult.No:
                        NavigationService.Navigate(new MenuPage());
                        break;
                }
            }
        }
        private void ResetGame()
        {
            PlayerHandPanel.Children.Clear();
            OpponentHandPanel.Children.Clear();
            TablePanel.Children.Clear();
            DeckImage.Children.Clear();
            ErrorMessage.Text = "";
            FirstPlayerLabel.Content = "";
            DeckCounter.Content = "0";
            Game = new GameManager();
            for (int i = 0; i < 36; i++)
            {
                AddCardToDeck(i);
            }
            StartGameButton.IsEnabled = true;
            DeckImage.Visibility = Visibility.Visible;
            TrumpCardImage.Visibility = Visibility.Visible;
            DeckCounter.Visibility = Visibility.Visible;
        }
    }
}
