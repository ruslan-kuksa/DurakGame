using DurakGame.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DurakGame.Models
{
    public class GameManager
    {
        public List<Player> Players { get; private set; }
        public Player ActivePlayer { get; private set; }
        public Deck Deck { get; private set; }
        public Card TrumpCard { get; private set; }
        public Table Table { get; private set; }
        public List<Card> DiscardPile { get; private set; }

        public GameManager()
        {
            Players = new List<Player>();
            Deck = new Deck();
            Table = new Table();
            DiscardPile = new List<Card>();
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
        }

        public void DealCards()
        {
            foreach (Player player in Players)
            {
                while (player.Hand.Count < 6 && Deck.Count > 0)
                {
                    player.AddCardToHand(Deck.DrawCard());
                }
            }
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
            DiscardPile.AddRange(Table.GetAllCards());
            Table.Clear();
            DealCards();
            SwitchActivePlayer();
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
    }
}
