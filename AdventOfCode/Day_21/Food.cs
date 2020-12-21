namespace AdventOfCode.Day_21_Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Food
    {
        public string[] ingredients;
        public string[] allergens;

        public override string ToString()
        {
            return String.Join(" ", ingredients) + " (contains " + String.Join(" ", allergens) + ")";
        }

        public bool ContainsAllergen(string allergen)
        {
            return allergens.Contains(allergen);
        }
    }
}
