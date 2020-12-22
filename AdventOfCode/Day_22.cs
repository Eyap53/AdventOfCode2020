namespace AdventOfCode
{
    using AdventOfCode.Day_22_Core;
    using AoCHelper;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text.RegularExpressions;

    public class Day_22 : BaseDay
    {
        private Deck _deckPlayer1;
        private Deck _deckPlayer2;

        private readonly int maxRoundIndex = 10000;

        public Day_22()
        {
            ParseInput();
        }

        public override string Solve_1()
        {
            Deck mirrorDeck1 = new Deck(_deckPlayer1);
            Deck mirrorDeck2 = new Deck(_deckPlayer2);
            int combatResult = Combat(mirrorDeck1, mirrorDeck2);
            // Console.WriteLine("Part 1 winner : " + combatResult.ToString());
            return (combatResult == 1 ? mirrorDeck1.CountPoint() : mirrorDeck2.CountPoint()).ToString();
        }

        public override string Solve_2()
        {
            Deck mirrorDeck1 = new Deck(_deckPlayer1);
            Deck mirrorDeck2 = new Deck(_deckPlayer2);
            int combatResult = RecursiveCombat(mirrorDeck1, mirrorDeck2);
            // Console.WriteLine("Part 2 winner : " + combatResult.ToString());
            return (combatResult == 1 ? mirrorDeck1.CountPoint() : mirrorDeck2.CountPoint()).ToString();
        }

        private void ParseInput()
        {
            string[] input = File.ReadAllLines(InputFilePath);
            Queue<Card> cardsPlayerX = new Queue<Card>();

            List<string> buffer = new List<string>();
            foreach (string line in input)
            {
                if (line == "Player 1:" || line == "")
                {
                    // Do Nothing
                }
                else if (line == "Player 2:")
                {
                    // Finish first Deck
                    _deckPlayer1 = new Deck(cardsPlayerX);
                    cardsPlayerX = new Queue<Card>();
                }
                else
                {
                    cardsPlayerX.Enqueue(new Card(int.Parse(line)));
                }
            }
            // Finish second Deck
            _deckPlayer2 = new Deck(cardsPlayerX);
        }

        /// <summary>
        /// Play a game of combat.
        /// </summary>
        /// <param name="deckPlayer1">The deck of the first player.</param>
        /// <param name="deckPlayer2">The deck of the second player.</param>
        /// <returns>True if Player 1 is winner, false if it is Player2.</returns>
        private int Combat(Deck deckPlayer1, Deck deckPlayer2)
        {
            if (deckPlayer1.IsEmpty())
            {
                throw new ArgumentException("deckPlayer1 is empty !");
            }
            else if (deckPlayer2.IsEmpty())
            {
                throw new ArgumentException("deckPlayer2 is empty !");
            }

            int roundIndex = 0;
            do
            {
                Card card1 = deckPlayer1.Draw();
                Card card2 = deckPlayer2.Draw();
                switch (card1.CompareTo(card2))
                {
                    case 1:
                        deckPlayer1.PlaceAtBottom(card1);
                        deckPlayer1.PlaceAtBottom(card2);
                        break;
                    case -1:
                        deckPlayer2.PlaceAtBottom(card2);
                        deckPlayer2.PlaceAtBottom(card1);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            } while (roundIndex <= maxRoundIndex && !deckPlayer1.IsEmpty() && !deckPlayer2.IsEmpty());
            if (deckPlayer2.IsEmpty())
            {
                return 1;
            }
            else if (deckPlayer1.IsEmpty())
            {
                return -1;
            }
            else
            {
                Console.WriteLine("Warning: Combat ended in a draw.");
                return 0;
            }
        }

        /// <summary>
        /// Play a game of recursive combat.
        /// </summary>
        /// <param name="deckPlayer1"></param>
        /// <param name="deckPlayer2"></param>
        /// <returns></returns>
        private int RecursiveCombat(Deck deckPlayer1, Deck deckPlayer2)
        {
            if (deckPlayer1.IsEmpty())
            {
                throw new ArgumentException("deckPlayer1 is empty !");
            }
            else if (deckPlayer2.IsEmpty())
            {
                throw new ArgumentException("deckPlayer2 is empty !");
            }

            int roundIndex = 0;
            List<int> player1Hashs = new List<int>();
            List<int> player2Hashs = new List<int>();

            // var caching
            int currentDeck1Hash;
            int currentDeck2Hash;

            do
            {
                roundIndex++;

                // Recursion prevention
                currentDeck1Hash = deckPlayer1.GetHashCode();
                currentDeck2Hash = deckPlayer2.GetHashCode();

                if (player1Hashs.Contains(currentDeck1Hash) && player2Hashs.Contains(currentDeck2Hash))
                {
                    return 1; // Player 1 victory
                }
                else
                {
                    // Adding current hashs
                    player1Hashs.Add(currentDeck1Hash);
                    player2Hashs.Add(currentDeck2Hash);
                }

                // Normal draw
                Card card1 = deckPlayer1.Draw();
                Card card2 = deckPlayer2.Draw();
                int battleResult;

                if (deckPlayer1.CardRemaining() >= card1.value && deckPlayer2.CardRemaining() >= card2.value)
                {
                    // Play another game recursive combat
                    Deck mirrorDeck1 = new Deck(deckPlayer1, card1.value);
                    Deck mirrorDeck2 = new Deck(deckPlayer2, card2.value);
                    battleResult = RecursiveCombat(mirrorDeck1, mirrorDeck2);
                }
                else
                {
                    battleResult = card1.CompareTo(card2);
                }
                switch (battleResult)
                {
                    case 1:
                        deckPlayer1.PlaceAtBottom(card1);
                        deckPlayer1.PlaceAtBottom(card2);
                        break;
                    case -1:
                        deckPlayer2.PlaceAtBottom(card2);
                        deckPlayer2.PlaceAtBottom(card1);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            } while (roundIndex <= maxRoundIndex && !deckPlayer1.IsEmpty() && !deckPlayer2.IsEmpty());

            if (deckPlayer2.IsEmpty())
            {
                return 1;
            }
            else if (deckPlayer1.IsEmpty())
            {
                return -1;
            }
            else
            {
                Console.WriteLine("Warning: Combat ended in a draw.");
                return 0;
            }
        }
    }
}
