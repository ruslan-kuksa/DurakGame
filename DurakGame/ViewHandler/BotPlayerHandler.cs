using DurakGame.Constants;
using DurakGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using DurakGame.Memento;

namespace DurakGame.ViewHandler
{
    /*public class BotPlayerHandler
    {

        private async void BotPlay()
        {
            await Task.Delay(GameConstants.ActionDelay);

            if (!(Game.ActivePlayer is BotPlayer bot))
            {
                return;
            }

            BotAction action = bot.SelectCardToPlay(Game.Table, Game.TrumpCard);
            if (action.Equals(default(BotAction)))
            {
                return;
            }

            if (action.IsPassing)
            {
                HandleBotPassing();
                return;
            }

            HandleBotCardPlayOrTake(bot, action);
            Game.NextTurn();
            CheckAndDisplayWinner();
        }

        private void HandleBotPassing()
        {
            Game.Table.Clear();
            Game.EndTurn();
            UpdateUI();
            Game.DealCards();
            if (Game.ActivePlayer is BotPlayer)
            {
                BotPlay();
            }
        }

        private void HandleBotCardPlayOrTake(BotPlayer bot, BotAction action)
        {
            Card cardToPlay = action.Card;
            if (cardToPlay != null)
            {
                PlayBotCard(bot, cardToPlay, action.IsDefending);
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
            var cardsOnTable = Game.Table.AttackCards.Concat(Game.Table.DefenseCards).ToList();
            Game.TakeCards(bot, cardsOnTable);
            UpdateUI();
            Game.DealCards();
        }

        private void EnableButton(Button toEnable, Button toDisable, bool isEnabled)
        {
            toEnable.Visibility = Visibility.Visible;
            toEnable.IsEnabled = isEnabled;
            toDisable.Visibility = Visibility.Hidden;
        }
    }*/
}
