using DurakGame.Messages;
using DurakGame.Models;
using DurakGame.Strategy;
using DurakGame.Validation;
using DurakGame.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DurakGame.ViewHandler
{
    public class UIPlayerManager
    {
        private readonly MainGamePage _mainGamePage;

        public UIPlayerManager(MainGamePage mainGamePage)
        {
            _mainGamePage = mainGamePage;
        }

        public void InitializePlayers()
        {
            BaseValidator attackValidator = new BaseValidator();
            BaseValidator defenseValidator = new BaseValidator();
            HumanAttackStrategy humanAttackStrategy = new HumanAttackStrategy(attackValidator);
            HumanDefenseStrategy humanDefenseStrategy = new HumanDefenseStrategy(defenseValidator);

            _mainGamePage.Game.AddPlayer(new HumanPlayer("Player", humanAttackStrategy, humanDefenseStrategy));
            _mainGamePage.Game.AddPlayer(new BotPlayer("Bot", new BotAttackStrategy(), new BotDefenseStrategy()));
        }

        public void CardControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_mainGamePage.Game.ActivePlayer is HumanPlayer player && sender is CardControl cardControl)
            {
                HandleHumanPlayerCardClick(player, cardControl.Card);
            }
            else
            {
                _mainGamePage.ErrorMessage.Text = GameNotification.BotTurnMessage;
            }
        }

        private void HandleHumanPlayerCardClick(HumanPlayer player, Card card)
        {
            _mainGamePage.Caretaker.Save(_mainGamePage.Game.SaveState());

            if (player.PlayCard(card, _mainGamePage.Game.Table, _mainGamePage.Game.TrumpCard, out string errorMessage))
            {
                ProcessCardPlay(card);
            }
            else
            {
                _mainGamePage.ErrorMessage.Text = errorMessage;
            }
        }

        private void ProcessCardPlay(Card card)
        {
            _mainGamePage.UIManager.AddCardToTable(card, _mainGamePage.Game.Table.DefenseCards.Contains(card));
            _mainGamePage.UIManager.DisplayPlayerHand();
            _mainGamePage.UIManager.DisplayOpponentHand();
            _mainGamePage.ErrorMessage.Text = string.Empty;
            _mainGamePage.UIManager.ShowUndoButton();
            _mainGamePage.Game.NextTurn();
            _mainGamePage.UIManager.CheckAndDisplayWinner();
            _mainGamePage.UIBotManager.ActivePlayerBot();
        }
    }
}
