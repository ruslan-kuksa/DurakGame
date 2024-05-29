using DurakGame.Constants;
using DurakGame.Models;
using DurakGame.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using DurakGame.Messages;
using System.Windows.Media.Imaging;

namespace DurakGame.ViewHandler
{
    public class UIManager
    {
        private readonly MainGamePage _mainGamePage;

        public UIManager(MainGamePage mainGamePage)
        {
            _mainGamePage = mainGamePage;
        }

        public void UpdateUI()
        {
            DisplayPlayerHand();
            DisplayOpponentHand();
            UpdateDeckCardCount();
            DisplayTable();
        }
        public void DisplayFirstPlayer()
        {
            Player firstPlayer = _mainGamePage.Game.FindLowestTrumpCard();
            if (firstPlayer != null)
            {
                _mainGamePage.FirstPlayerLabel.Content = string.Format(GameNotification.FirstPlayerMessage, firstPlayer.Name);
                _mainGamePage.UIBotManager.ActivePlayerBot();
            }
            else
            {
                _mainGamePage.FirstPlayerLabel.Content = GameNotification.NoTrumpCardsMessage;
            }
        }

        public void UpdateTrumpCardImage()
        {
            Suit trumpSuit = _mainGamePage.Game.TrumpCard.Suit;
            string suitString = trumpSuit.ToString().ToLower();
            string rankString = _mainGamePage.Game.TrumpCard.Rank.ToString().ToLower();
            string imagePath = $"pack://application:,,,/Resources/{rankString}_of_{suitString}.png";
            _mainGamePage.TrumpCardImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
        }
        public double CalculateCardMargin(int cardCount)
        {
            if (cardCount >= 30) return GameConstants.LargeCardMargin;
            if (cardCount >= 24) return GameConstants.MediumCardMargin;
            if (cardCount >= 12) return GameConstants.SmallCardMargin;
            return GameConstants.DefaultCardMargin;
        }

        public void DisplayHand(Player player, Panel handPanel, bool isPlayerHand)
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
                    playerCardControl.MouseLeftButtonDown += _mainGamePage.CardControl_MouseLeftButtonDown;
                }
                else if (cardControl is EnemyCardControl enemyCardControl)
                {
                    enemyCardControl.Card = card;
                }

                handPanel.Children.Add(cardControl);
            }
        }

        public void DisplayPlayerHand()
        {
            DisplayHand(_mainGamePage.Game.Players[0], _mainGamePage.PlayerHandPanel, true);
        }

        public void DisplayOpponentHand()
        {
            DisplayHand(_mainGamePage.Game.Players[1], _mainGamePage.OpponentHandPanel, false);
        }

        private void UpdateDeckCardCount()
        {
            int deckCount = _mainGamePage.Game.Deck.Count;
            if (!_mainGamePage.Game.Deck.IsTrumpCardTaken)
            {
                deckCount++;
            }
            _mainGamePage.DeckCounter.Content = deckCount.ToString();
            _mainGamePage.UIDeckManager.SetDeckVisibility(deckCount);
        }

        public void DisplayTable()
        {
            _mainGamePage.TablePanel.Children.Clear();
            foreach (Card card in _mainGamePage.Game.Table.AttackCards)
            {
                AddCardToTable(card, false);
            }
            foreach (Card card in _mainGamePage.Game.Table.DefenseCards)
            {
                AddCardToTable(card, true);
            }
        }

        public void AddCardToTable(Card card, bool isDefending)
        {
            CardControl cardControl = new CardControl
            {
                Card = card,
                Width = GameConstants.CardWidth,
                Height = GameConstants.CardHeight,
                Margin = new Thickness(GameConstants.DefaultCardMargin)
            };

            int position = isDefending ? _mainGamePage.Game.Table.DefenseCards.Count - 1 : _mainGamePage.Game.Table.AttackCards.Count - 1;
            Canvas.SetLeft(cardControl, GameConstants.CardLeftMultiplier * (position + 1) + GameConstants.CardLeftOffset);
            Canvas.SetTop(cardControl, isDefending ? 0 : GameConstants.CardTopOffset);

            _mainGamePage.TablePanel.Children.Add(cardControl);
        }

        public void CheckAndDisplayWinner()
        {
            Player winner = _mainGamePage.Game.CheckWinner();
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
                        _mainGamePage.NavigationService.Navigate(new MenuPage());
                        break;
                }
            }
        }

        public void ResetGame()
        {
            ClearGameUI();
            _mainGamePage.UIDeckManager.InitializeDeck();
            _mainGamePage.InitializeGameManager();
            _mainGamePage.StartGameButton.IsEnabled = true;
            _mainGamePage.UIDeckManager.SetDeckVisibility(GameConstants.MaxDeckCount);
        }

        private void ClearGameUI()
        {
            _mainGamePage.PlayerHandPanel.Children.Clear();
            _mainGamePage.OpponentHandPanel.Children.Clear();
            _mainGamePage.TablePanel.Children.Clear();
            _mainGamePage.DeckImage.Children.Clear();
            _mainGamePage.ErrorMessage.Text = string.Empty;
            _mainGamePage.FirstPlayerLabel.Content = string.Empty;
            _mainGamePage.DeckCounter.Content = "0";
        }

        public void ShowUndoButton()
        {
            _mainGamePage.UndoButton.Visibility = Visibility.Visible;
            Task.Delay(2000).ContinueWith(t => _mainGamePage.Dispatcher.Invoke(() =>
            {
                _mainGamePage.UndoButton.Visibility = Visibility.Hidden;
            }));
        }

    }
}
