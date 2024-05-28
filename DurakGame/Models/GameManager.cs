using DurakGame.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using DurakGame.Memento;
using DurakGame.Hints;
using DurakGame.Messages;

namespace DurakGame.Models
{
    public class GameManager
    {
        private int hintUsageCount = 0;
        private const int maxHintUsage = 2;
        public List<Player> Players { get; private set; }
        public Player ActivePlayer { get; private set; }
        public Deck Deck { get; private set; }
        public Card TrumpCard { get; private set; }
        public Table Table { get; private set; }

        private IHintHandler hintHandler;

        public event Action GameChanged;

        public GameManager()
        {
            Players = new List<Player>();
            Deck = new Deck();
            Table = new Table();
            hintHandler = new AttackHintHandler();
            hintHandler.SetNext(new DefenseHintHandler());
        }
        public string GetHint()
        {
            if (hintUsageCount >= maxHintUsage)
            {
                return GameNotification.HintUsageExceededMessage;
            }
            hintUsageCount++;
            return hintHandler.Handle(ActivePlayer, Table, TrumpCard);
        }
        private void OnGameChanged()
        {
            GameChanged?.Invoke();
        }
        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }
        public void StartGame()
        {
            foreach (Player player in Players)
            {
                for (int i = 0; i < 6; i++)
                {
                    player.AddCardToHand(Deck.DrawCard());
                }
            }
            if (Deck.Count > 0)
            {
                TrumpCard = Deck.DrawCard();
            }
            ActivePlayer = FindLowestTrumpCard();
            OnGameChanged();
        }

        public void DealCards()
        {
            foreach (Player player in Players)
            {
                while (player.Hand.Count < 6 && Deck.Count > 1) 
                {
                    player.AddCardToHand(Deck.DrawCard());
                }
            }
            if (Deck.Count == 1)
            {
                foreach (Player player in Players)
                {
                    if (player.Hand.Count < 6)
                    {
                        player.AddCardToHand(TrumpCard);
                        Deck.DrawCard();
                        Deck.IsTrumpCardTaken = true;
                        break;
                    }
                }
            }
            OnGameChanged();
        }
        public Player FindLowestTrumpCard()
        {
            Player startingPlayer = null;
            int lowestTrumpValue = int.MaxValue;

            foreach (Player player in Players)
            {
                foreach (Card card in player.Hand)
                {
                    int cardRankValue = Convert.ToInt32(card.Rank);
                    if (card.Suit == TrumpCard.Suit && cardRankValue < lowestTrumpValue)
                    {
                        startingPlayer = player;
                        lowestTrumpValue = cardRankValue;
                    }
                }
            }
            return startingPlayer ?? Players[0];
        }
        public void SwitchActivePlayer()
        {
            int activePlayerIndex = Players.IndexOf(ActivePlayer);
            int nextPlayerIndex = (activePlayerIndex + 1) % Players.Count;
            ActivePlayer = Players[nextPlayerIndex];

        }
        public void NextTurn()
        {
            SwitchActivePlayer();
        }
        public void EndTurn()
        {
            Table.Clear();
            DealCards();
            SwitchActivePlayer();
            OnGameChanged();
        }
        public void TakeCards(Player player, List<Card> cardsOnTable)
        {
            foreach (Card card in cardsOnTable)
            {
                player.AddCardToHand(card);
            }
            Table.Clear();
            DealCards();
            SwitchActivePlayer();
        }
        public Player CheckWinner()
        {
            if (Deck.Count == 0)
            {
                foreach (Player player in Players)
                {
                    if (player.Hand.Count == 0)
                    {
                        return player;
                    }
                }
            }
            return null;
        }

        public GameMemento SaveState()
        {
            return new GameMemento(
                new List<Card>(Players[0].Hand),
                new List<Card>(Table.AttackCards),
                new List<Card>(Table.DefenseCards),
                ActivePlayer
            );
        }

        public void RestoreState(GameMemento memento)
        {
            Players[0].SetHand(new List<Card>(memento.PlayerHand));
            Table.SetAttackCards(new List<Card>(memento.TableAttackCards));
            Table.SetDefenseCards(new List<Card>(memento.TableDefenseCards));
            ActivePlayer = memento.ActivePlayer;
            OnGameChanged();
        }
    }
}

