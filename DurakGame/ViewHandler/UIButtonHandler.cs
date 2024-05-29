using DurakGame.Memento;
using DurakGame.Messages;
using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DurakGame.ViewHandler
{
    public class UIButtonHandler
    {
        private readonly MainGamePage _mainGamePage;

        public UIButtonHandler(MainGamePage mainGamePage)
        {
            _mainGamePage = mainGamePage;
        }

        public void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            _mainGamePage.UIPlayerManager.InitializePlayers();
            _mainGamePage.Game.StartGame();
            _mainGamePage.UIManager.UpdateTrumpCardImage();
            _mainGamePage.UIManager.UpdateUI();
            _mainGamePage.UIManager.DisplayFirstPlayer();
            ((Button)sender).IsEnabled = false;
            _mainGamePage.GameStateTextBlock.Text = GameNotification.GameStartedMessage;
        }

        public void TakeButton_Click(object sender, RoutedEventArgs e)
        {
            Player player = _mainGamePage.Game.Players[0];
            List<Card> cardsOnTable = _mainGamePage.Game.Table.AttackCards.Concat(_mainGamePage.Game.Table.DefenseCards).ToList();
            _mainGamePage.Game.TakeCards(player, cardsOnTable);
            _mainGamePage.UIManager.UpdateUI();
            _mainGamePage.Game.DealCards();
            _mainGamePage.UIBotManager.ActivePlayerBot();
            _mainGamePage.GameStateTextBlock.Text = GameNotification.PlayerTookCardsMessage;
        }

        public void BeatButton_Click(object sender, RoutedEventArgs e)
        {
            _mainGamePage.Game.EndTurn();
            _mainGamePage.UIManager.UpdateUI();
            _mainGamePage.Game.DealCards();
            _mainGamePage.UIBotManager.ActivePlayerBot();
            _mainGamePage.GameStateTextBlock.Text = GameNotification.PlayerEndedTurnMessage;
        }

        public void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            GameMemento memento = _mainGamePage.Caretaker.Undo();
            _mainGamePage.Game.RestoreState(memento);
            _mainGamePage.UIManager.DisplayPlayerHand();
            _mainGamePage.UIManager.DisplayOpponentHand();
            _mainGamePage.UIManager.DisplayTable();
            _mainGamePage.GameStateTextBlock.Text = GameNotification.CardReturnedMessage;
        }

        public void HintButton_Click(object sender, RoutedEventArgs e)
        {
            string hint = _mainGamePage.Game.GetHint();
            MessageBox.Show(hint, "Підсказка");
        }
    }
}
