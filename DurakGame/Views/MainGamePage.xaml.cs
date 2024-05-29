using DurakGame.Memento;
using DurakGame.Messages;
using DurakGame.Models;
using DurakGame.Strategy;
using DurakGame.Validation;
using DurakGame.Views;
using DurakGame.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
using static System.Collections.Specialized.BitVector32;

namespace DurakGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainGamePage : Page
    {
        private GameManager Game;
        private GameCaretaker Caretaker = new GameCaretaker();

        public MainGamePage()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            SetBackgroundImage();
            InitializeDeck();
            InitializeGameManager();
        }

        private void SetBackgroundImage()
        {
            BitmapImage newImageSource = new BitmapImage(new Uri(App.BackgroundImagePath.ToString(), UriKind.RelativeOrAbsolute));
            ImageBrush newImageBrush = new ImageBrush(newImageSource);
            BackGrid.Background = newImageBrush;
        }

        private void InitializeDeck()
        {
            for (int i = 0; i < 36; i++)
            {
                AddCardToDeck(i);
            }
        }

        private void InitializeGameManager()
        {
            Game = new GameManager();
            Game.GameChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            DisplayPlayerHand();
            DisplayOpponentHand();
            UpdateDeckCardCount();
            DisplayTable();
        }
        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            InitializePlayers();
            Game.StartGame();
            UpdateTrumpCardImage();
            UpdateUI();
            DisplayFirstPlayer();
            ((Button)sender).IsEnabled = false;
            GameStateTextBlock.Text = GameNotification.GameStartedMessage;
        }
        private void InitializePlayers()
        {
            BaseValidator attackValidator = new BaseValidator();
            BaseValidator defenseValidator = new BaseValidator();
            HumanAttackStrategy humanAttackStrategy = new HumanAttackStrategy(attackValidator);
            HumanDefenseStrategy humanDefenseStrategy = new HumanDefenseStrategy(defenseValidator);

            Game.AddPlayer(new HumanPlayer("Player", humanAttackStrategy, humanDefenseStrategy));
            Game.AddPlayer(new BotPlayer("Bot", new BotAttackStrategy(), new BotDefenseStrategy()));
        }
        private void DisplayFirstPlayer()
        {
            Player firstPlayer = Game.FindLowestTrumpCard();
            if (firstPlayer != null)
            {
                FirstPlayerLabel.Content = string.Format(GameNotification.FirstPlayerMessage, firstPlayer.Name);
                ActivePlayerBot();
            }
            else
            {
                FirstPlayerLabel.Content = GameNotification.NoTrumpCardsMessage;
            }
        }
        private void UpdateDeckCardCount()
        {
            int deckCount = Game.Deck.Count;
            if (!Game.Deck.IsTrumpCardTaken)
            {
                deckCount++;
            }
            DeckCounter.Content = deckCount.ToString();
            SetDeckVisibility(deckCount);
        }

        private void SetDeckVisibility(int deckCount)
        {
            var visibility = deckCount == 0 ? Visibility.Hidden : Visibility.Visible;
            DeckImage.Visibility = visibility;
            TrumpCardImage.Visibility = visibility;
            DeckCounter.Visibility = visibility;
        }
        private double CalculateCardMargin(int cardCount)
        {
            if (cardCount >= 30) return GameConstants.LargeCardMargin;
            if (cardCount >= 24) return GameConstants.MediumCardMargin;
            if (cardCount >= 12) return GameConstants.SmallCardMargin;
            return GameConstants.DefaultCardMargin;
        }
        private void DisplayHand(Player player, Panel handPanel, bool isPlayerHand)
        {
            handPanel.Children.Clear();
            double cardMargin = CalculateCardMargin(player.Hand.Count);

            foreach (Card card in player.Hand)
            {
                UserControl cardControl = isPlayerHand ? new CardControl() : new EnemyCardControl();
                cardControl.Width = GameConstants.CardWidth;
                cardControl.Height = GameConstants.CardHeight;
                cardControl.Margin = new Thickness(cardMargin, 0, 0, 0);

                if (cardControl is CardControl playerCardControl)
                {
                    playerCardControl.Card = card;
                    playerCardControl.MouseLeftButtonDown += CardControl_MouseLeftButtonDown;
                }
                else if (cardControl is EnemyCardControl enemyCardControl)
                {
                    enemyCardControl.Card = card;
                }

                handPanel.Children.Add(cardControl);
            }
        }

        private void DisplayOpponentHand()
        {
            DisplayHand(Game.Players[1], OpponentHandPanel, false);
        }

        private void DisplayPlayerHand()
        {
            DisplayHand(Game.Players[0], PlayerHandPanel, true);
        }

        private void CardControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Game.ActivePlayer is HumanPlayer player && sender is CardControl cardControl)
            {
                HandleHumanPlayerCardClick(player, cardControl.Card);
            }
            else
            {
                ErrorMessage.Text = GameNotification.BotTurnMessage;
            }
        }

        private void HandleHumanPlayerCardClick(HumanPlayer player, Card card)
        {
            Caretaker.Save(Game.SaveState());

            if (player.PlayCard(card, Game.Table, Game.TrumpCard, out string errorMessage))
            {
                ProcessCardPlay(card);
            }
            else
            {
                ErrorMessage.Text = errorMessage;
            }
        }
        private void ActivePlayerBot()
        {
            if (Game.ActivePlayer is BotPlayer)
            {
                BotPlay();
            }
        }
        private void ProcessCardPlay(Card card)
        {
            AddCardToTable(card, Game.Table.DefenseCards.Contains(card));
            DisplayPlayerHand();
            DisplayOpponentHand();
            ErrorMessage.Text = string.Empty;
            ShowUndoButton();
            Game.NextTurn();
            CheckAndDisplayWinner();
            ActivePlayerBot();
        }

        private void AddCardToTable(Card card, bool isDefending)
        {
            CardControl cardControl = new CardControl
            {
                Card = card,
                Width = GameConstants.CardWidth,
                Height = GameConstants.CardHeight,
                Margin = new Thickness(GameConstants.DefaultCardMargin)
            };

            int position = isDefending ? Game.Table.DefenseCards.Count - 1 : Game.Table.AttackCards.Count - 1;
            Canvas.SetLeft(cardControl, GameConstants.CardLeftMultiplier * (position + 1) + GameConstants.CardLeftOffset);
            Canvas.SetTop(cardControl, isDefending ? 0 : GameConstants.CardTopOffset);

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
            foreach (Card card in Game.Table.AttackCards)
            {
                AddCardToTable(card, false);
            }
            foreach (Card card in Game.Table.DefenseCards)
            {
                AddCardToTable(card, true);
            }
        }

        private void TakeButton_Click(object sender, RoutedEventArgs e)
        {
            Player player = Game.Players[0];
            List<Card> cardsOnTable = Game.Table.AttackCards.Concat(Game.Table.DefenseCards).ToList();
            Game.TakeCards(player, cardsOnTable);
            UpdateUI();
            Game.DealCards();
            ActivePlayerBot();
            GameStateTextBlock.Text = GameNotification.PlayerTookCardsMessage;
        }

        private void BeatButton_Click(object sender, RoutedEventArgs e)
        {
            Game.EndTurn();
            UpdateUI();
            Game.DealCards();
            ActivePlayerBot();
            GameStateTextBlock.Text = GameNotification.PlayerEndedTurnMessage;
        }

        private async void BotPlay()
        {
            await Task.Delay(GameConstants.ActionDelay);

            if (!(Game.ActivePlayer is BotPlayer bot))
            {
                return;
            }

            BotAction? action = bot.SelectCardToPlay(Game.Table, Game.TrumpCard);
            if (!action.HasValue)
            {
                TakeBotCards(bot);
                return;
            }

            if (action.Value.IsPassing)
            {
                HandleBotPassing();
                return;
            }

            HandleBotCardPlayOrTake(bot, action.Value);
            Game.NextTurn();
            CheckAndDisplayWinner();
        }

        private void HandleBotPassing()
        {
            Game.Table.Clear();
            Game.EndTurn();
            UpdateUI();
            Game.DealCards();
            ActivePlayerBot();
        }

        private void HandleBotCardPlayOrTake(BotPlayer bot, BotAction action)
        {
            if (action.Card != null)
            {
                PlayBotCard(bot, action.Card, action.IsDefending);
            }
            else
            {
                TakeBotCards(bot);
            }
        }

        private void PlayBotCard(BotPlayer bot, Card cardToPlay, bool isDefending)
        {
            bot.RemoveCardFromHand(cardToPlay);

            if (isDefending)
            {
                EnableButton(BeatButton, TakeButton, true);
                Game.Table.AddDefenseCard(cardToPlay);
            }
            else
            {
                EnableButton(TakeButton, BeatButton, true);
                Game.Table.AddAttackCard(cardToPlay);
            }

            AddCardToTable(cardToPlay, isDefending);
            DisplayPlayerHand();
            DisplayOpponentHand();
        }

        private void TakeBotCards(BotPlayer bot)
        {
            List<Card> cardsOnTable = Game.Table.AttackCards.Concat(Game.Table.DefenseCards).ToList();
            Game.TakeCards(bot, cardsOnTable);
            UpdateUI();
            Game.EndTurn();
            Game.DealCards();
        }

        private void EnableButton(Button toEnable, Button toDisable, bool isEnabled)
        {
            toEnable.Visibility = Visibility.Visible;
            toEnable.IsEnabled = isEnabled;
            toDisable.Visibility = Visibility.Hidden;
        }

        private void AddCardToDeck(int i)
        {
            Image cardImage = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/card_back.png")),
                Width = GameConstants.CardWidth,
                Height = GameConstants.CardHeight,
            };
            DeckImage.Children.Add(cardImage);
            Canvas.SetTop(cardImage, i * GameConstants.DeckCardOffset);
            Canvas.SetLeft(cardImage, 5 + i * GameConstants.DeckCardLeftOffset);
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
            ClearGameUI();
            InitializeDeck();
            InitializeGameManager();
            StartGameButton.IsEnabled = true;
            SetDeckVisibility(GameConstants.MaxDeckCount);
        }

        private void ClearGameUI()
        {
            PlayerHandPanel.Children.Clear();
            OpponentHandPanel.Children.Clear();
            TablePanel.Children.Clear();
            DeckImage.Children.Clear();
            ErrorMessage.Text = string.Empty;
            FirstPlayerLabel.Content = string.Empty;
            DeckCounter.Content = "0";
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            GameMemento memento = Caretaker.Undo();
            Game.RestoreState(memento);
            DisplayPlayerHand();
            DisplayOpponentHand();
            DisplayTable();
            GameStateTextBlock.Text = GameNotification.CardReturnedMessage;
        }

        private void ShowUndoButton()
        {
            UndoButton.Visibility = Visibility.Visible;
            Task.Delay(2000).ContinueWith(t => Dispatcher.Invoke(() =>
            {
                UndoButton.Visibility = Visibility.Hidden;
            }));
        }
        private void HintButton_Click(object sender, RoutedEventArgs e)
        {
            string hint = Game.GetHint();
            MessageBox.Show(hint, "Підсказка");
        }
    }
}
