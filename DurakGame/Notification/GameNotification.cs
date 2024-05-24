using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakGame.Messages
{
    public static class GameNotification
    {
        public const string NoTrumpCardsMessage = "Немає гравців з козирними картами";
        public const string GameStartedMessage = "Гра розпочалася";
        public const string BotTurnMessage = "Зараз хід бота, зачекайте своєї черги";
        public const string PlayerTookCardsMessage = "Гравець взяв карти зі столу";
        public const string PlayerEndedTurnMessage = "Гравець закінчив свій хід";
        public const string CardReturnedMessage = "Карта повернута до руки";
        public const string CannotAddAttackCardMessage = "Цю карту не можливо підкинути";
        public const string CannotBeatAttackCardMessage = "Ця карта не може побити атакуючу карту";
    }
}
