using DurakGame.Constants;
using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace DurakGame.ViewHandler
{
    public class UIBotManager
    {
        private readonly MainGamePage _mainGamePage;

        public UIBotManager(MainGamePage mainGamePage)
        {
            _mainGamePage = mainGamePage;
        }

        public async void BotPlay()
        {
            await Task.Delay(GameConstants.ActionDelay);

            if (!(_mainGamePage.Game.ActivePlayer is BotPlayer bot))
            {
                return;
            }

            BotAction? action = bot.SelectCardToPlay(_mainGamePage.Game.Table, _mainGamePage.Game.TrumpCard);
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
            _mainGamePage.Game.NextTurn();
            _mainGamePage.UIManager.CheckAndDisplayWinner();
        }

        private void HandleBotPassing()
        {
            _mainGamePage.Game.Table.Clear();
            _mainGamePage.Game.EndTurn();
            _mainGamePage.UIManager.UpdateUI();
            _mainGamePage.Game.DealCards();
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
                EnableButton(_mainGamePage.BeatButton, _mainGamePage.TakeButton, true);
                _mainGamePage.Game.Table.AddDefenseCard(cardToPlay);
            }
            else
            {
                EnableButton(_mainGamePage.TakeButton, _mainGamePage.BeatButton, true);
                _mainGamePage.Game.Table.AddAttackCard(cardToPlay);
            }

            _mainGamePage.UIManager.AddCardToTable(cardToPlay, isDefending);
            _mainGamePage.UIManager.DisplayPlayerHand();
            _mainGamePage.UIManager.DisplayOpponentHand();
        }

        private void TakeBotCards(BotPlayer bot)
        {
            List<Card> cardsOnTable = _mainGamePage.Game.Table.AttackCards.Concat(_mainGamePage.Game.Table.DefenseCards).ToList();
            _mainGamePage.Game.TakeCards(bot, cardsOnTable);
            _mainGamePage.UIManager.UpdateUI();
            _mainGamePage.Game.EndTurn();
            _mainGamePage.Game.DealCards();
        }

        private void EnableButton(Button toEnable, Button toDisable, bool isEnabled)
        {
            toEnable.Visibility = Visibility.Visible;
            toEnable.IsEnabled = isEnabled;
            toDisable.Visibility = Visibility.Hidden;
        }

        public void ActivePlayerBot()
        {
            if (_mainGamePage.Game.ActivePlayer is BotPlayer)
            {
                BotPlay();
            }
        }
    }
}
