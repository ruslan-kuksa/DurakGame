using DurakGame.Models;
using DurakGame.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
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
            Game.StartGame();
            UpdateTrumpCardImage();
            DisplayPlayerHand(Game.Players[0]);
            DisplayOpponentHand(Game.Players[1]);
            UpdateDeckCardCount();
            if (Game.TrumpCard == null)
            {
                MessageBox.Show("TrumpCard ще не встановлено.");
            }
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
            DeckCounter.Content = Game.Deck.Count.ToString();
        }
        private void DisplayOpponentHand(Player opponent)
        {
            OpponentHandPanel.Children.Clear();

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

        private void DisplayPlayerHand(Player player)
        {
            PlayerHandPanel.Children.Clear();

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
        }

        private void CardControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Game.ActivePlayer is HumanPlayer && sender is CardControl cardControl)
            {
                Card card = cardControl.Card;

                if (Game.Players.Count > 0 && Game.Players[0] is HumanPlayer player && Game.Players[1] is BotPlayer bot)
                {
                    bool isAttacking = Game.Table.DefenseCards.Count == Game.Table.AttackCards.Count;

                    if (isAttacking)
                    {
                        if (Game.Table.CanAddAttackCard(card))
                        {
                            player.RemoveCardFromHand(card);
                            Game.Table.AddAttackCard(card);
                            AddCardToTable(card, false);
                            DisplayPlayerHand(player);
                            DisplayOpponentHand(bot);
                            ErrorMessage.Text = "";
                        }
                        else
                        {
                            ErrorMessage.Text = "Ця карта не може бути підкинута.";
                        }
                    }
                    else
                    {
                        Card lastAttackCard = Game.Table.AttackCards.LastOrDefault();
                        if (lastAttackCard != null && card.CanBeat(lastAttackCard, Game.TrumpCard.Suit))
                        {
                            player.RemoveCardFromHand(card);
                            Game.Table.AddDefenseCard(card);
                            AddCardToTable(card, true);
                            DisplayPlayerHand(player);
                            DisplayOpponentHand(bot);
                            ErrorMessage.Text = "";
                        }
                        else
                        {
                            ErrorMessage.Text = "Ця карта не може побити атакуючу карту.";
                        }
                    }

                    if (string.IsNullOrEmpty(ErrorMessage.Text))
                    {
                        Game.NextTurn();
                        if (Game.ActivePlayer is BotPlayer)
                        {
                            BotPlay();
                        }
                    }
                }
            }
            else
            {
                ErrorMessage.Text = "Зараз хід бота, зачекайте своєї черги.";
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
                DisplayPlayerHand(player);
                DisplayOpponentHand(opponent);
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
            for (int i = 0; i < Game.Table.DefenseCards.Count; i++)
            {
                Card defenseCard = Game.Table.DefenseCards[i];
                CardControl defenseCardControl = new CardControl
                {
                    Card = defenseCard,
                    Width = cardWidth,
                    Height = cardHeight
                };
                Canvas.SetLeft(defenseCardControl, i * xOffset);
                Canvas.SetTop(defenseCardControl, cardHeight / 2);
                TablePanel.Children.Add(defenseCardControl);
            }
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
        private void BotPlay()
        {
            if (Game.ActivePlayer is BotPlayer bot)
            {
                BotAction? action = bot.SelectCardToPlay(Game.Table, Game.TrumpCard);

                if (action.HasValue)
                {
                    Card cardToPlay = action.Value.Card;

                    if (cardToPlay != null)
                    {
                        bot.RemoveCardFromHand(cardToPlay);

                        if (action.Value.IsDefending)
                        {
                            Game.Table.AddDefenseCard(cardToPlay);
                        }
                        else
                        {
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
                }
            }
        }
    }
}
