namespace AdventOfCode.Day_22_Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Deck
    {
        private Queue<Card> _cards;

        public Deck(Queue<Card> cards)
        {
            this._cards = cards;
        }

        /// <summary>
        /// Constructor that copy another deck.
        /// </summary>
        /// <param name="deck">The deck to copy.</param>
        public Deck(Deck deck)
        {
            this._cards = new Queue<Card>(deck._cards);
        }

        /// <summary>
        /// Constructor that copy another deck but with a limited number of cards.
        /// </summary>
        /// <param name="deck">The deck to copy.</param>
        /// <param name="count">The number of card to use.</param>
        public Deck(Deck deck, int count)
        {
            this._cards = new Queue<Card>(deck._cards.Take(count));
        }

        public Card Draw()
        {
            return _cards.Dequeue();
        }

        public void PlaceAtBottom(Card card)
        {
            _cards.Enqueue(card);
        }

        public bool IsEmpty()
        {
            return !_cards.Any();
        }

        public int CardRemaining()
        {
            return _cards.Count;
        }

        public int CountPoint()
        {
            int sum = 0;
            for (int i = 1; i <= _cards.Count; i++)
            {
                sum += _cards.ElementAt(_cards.Count - i).value * i;
            }
            return sum;
        }

        public override int GetHashCode()
        {
            int hash = 19;
            foreach (Card c in _cards)
            {
                hash = hash * 31 + c.GetHashCode();
            }
            return hash;
        }

        public override string ToString()
        {
            return String.Join(", ", _cards);
        }
    }
}
