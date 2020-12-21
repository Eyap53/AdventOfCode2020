namespace AdventOfCode
{
    using AdventOfCode.Day_21_Core;
    using AoCHelper;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text.RegularExpressions;

    public class Day_21 : BaseDay
    {
        private Food[] _foods;
        HashSet<string> _allAllergens;
        HashSet<string> _allIngredients;
        Dictionary<string, string> allergenForIngredient = new Dictionary<string, string>();
        Dictionary<string, HashSet<string>> correspondanceWIP = new Dictionary<string, HashSet<string>>();

        public Day_21()
        {
            ParseInput();
        }

        public override string Solve_1()
        {
            int nonAllergenic = 0;
            foreach (Food food in _foods)
            {
                foreach (string ingredient in food.ingredients)
                {
                    if (!allergenForIngredient.ContainsKey(ingredient))
                    {
                        nonAllergenic++;
                    }
                }
            }

            return nonAllergenic.ToString();
        }

        public override string Solve_2()
        {
            List<string> ordoredIndredients = allergenForIngredient.Keys.ToList();
            ordoredIndredients.Sort((x1, x2) => allergenForIngredient[x1].CompareTo(allergenForIngredient[x2]));
            return String.Join(",", ordoredIndredients);
        }

        private void ParseInput()
        {
            string[] input = File.ReadAllLines(InputFilePath);
            _foods = new Food[input.Length];
            _allAllergens = new HashSet<string>();
            _allIngredients = new HashSet<string>();

            #region Food Parsing
            {
                string pattern = @"^(?<ingredients>(\w+ ?)+) \(contains (?<allergens>(\w+(, )?)+)\)$";
                Regex regex = new Regex(pattern);

                // Var caching
                Match m;
                Food food;
                string[] allergens;
                string[] ingredients;
                for (int i = 0; i < input.Length; i++)
                {
                    food = new Food();
                    m = regex.Match(input[i]);

                    if (m.Success)
                    {
                        ingredients = m.Groups["ingredients"].Value.Split();
                        allergens = m.Groups["allergens"].Value.Split(", ");
                        food.ingredients = ingredients;
                        _allIngredients.UnionWith(ingredients);
                        food.allergens = allergens;
                        _allAllergens.UnionWith(allergens);

                        _foods[i] = food;
                    }
                    else
                    {
                        throw new ArgumentException("Not matched. Input : '" + input[i] + "'");
                    }
                }
            }
            #endregion

            #region Ingredients recognition
            {
                for (int i = 0; i < _allAllergens.Count; i++)
                {
                    string allergen = _allAllergens.ElementAt(i);
                    foreach (Food food in _foods)
                    {
                        if (food.ContainsAllergen(allergen))
                        {
                            List<string> ingredients = food.ingredients.ToList();
                            ingredients.RemoveAll(x => allergenForIngredient.ContainsKey(x));
                            if (!correspondanceWIP.ContainsKey(allergen))
                            {
                                correspondanceWIP[allergen] = ingredients.ToHashSet();
                            }
                            else
                            {
                                correspondanceWIP[allergen].IntersectWith(ingredients);
                            }
                        }
                    }
                    if (correspondanceWIP[allergen].Count == 1)
                    {
                        string ingredientFound = correspondanceWIP[allergen].First();
                        allergenForIngredient[ingredientFound] = allergen;
                        List<string> ingredientsWithAllergenFound = new List<string>() { ingredientFound };

                        for (int j = 0; j < i; j++)
                        {
                            if (correspondanceWIP[_allAllergens.ElementAt(j)].Count > 1 && correspondanceWIP[_allAllergens.ElementAt(j)].RemoveWhere(x => ingredientsWithAllergenFound.Contains(x)) > 0)
                            {
                                if (correspondanceWIP[_allAllergens.ElementAt(j)].Count == 1)
                                {
                                    ingredientFound = correspondanceWIP[_allAllergens.ElementAt(j)].First();
                                    allergenForIngredient[ingredientFound] = _allAllergens.ElementAt(j);
                                    ingredientsWithAllergenFound.Add(ingredientFound);
                                    j = -1; // Restart Loop
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }

        private string GetFoodListAsString()
        {
            return String.Join("\n", (object[])_foods);
        }
    }
}
