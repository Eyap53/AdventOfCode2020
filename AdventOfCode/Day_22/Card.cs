namespace AdventOfCode.Day_22_Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Card : IComparable<Card>
    {
        public int value;

        public Card(int value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public int CompareTo(Card other)
        {
            if (this.value > other.value)
            {
                return 1;
            }
            else if (this.value < other.value)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
