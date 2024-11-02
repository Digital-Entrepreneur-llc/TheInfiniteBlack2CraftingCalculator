using System;
using System.Collections.Generic;

namespace TheInfiniteBlack2CraftingCalculator
{
    public class Recipe
    {
        public string Name { get; private set; }
        public int OutputQuantity { get; private set; }
        public Dictionary<string, int> Ingredients { get; private set; }
        public Dictionary<string, int> FactionRequirements { get; private set; }
        public bool RequiresEliteRank { get; set; }  // New property
        public bool RequiresFactionSelection { get; set; }
        public string SelectedFaction { get; set; }
        public Dictionary<string, int> EliteSkullCosts { get; private set; }  // New property for elite skull costs
        public string DefaultShipMod { get; private set; }  // For Emperor class ships
        private decimal defaultModCost;

        public Recipe(string name, int outputQuantity)
        {
            Name = name;
            OutputQuantity = outputQuantity;
            Ingredients = new Dictionary<string, int>();
            FactionRequirements = new Dictionary<string, int>();
            EliteSkullCosts = new Dictionary<string, int>
            {
                { "Elite 0", 0 },
                { "Elite 1", 100 },
                { "Elite 2", 500 },
                { "Elite 3", 1000 },
                { "Elite 4", 2000 },
                { "Elite 5", 3000 }
            };
            RequiresFactionSelection = false;
            RequiresEliteRank = false;

            // Set default Paladin mod for Emperor class ships
            if (name.Contains("Emperor"))
            {
                DefaultShipMod = "Paladin";
                defaultModCost = 300000m;
            }
        }

        public decimal CalculateModCostAdjustment(string selectedMod, ResourceManager resourceManager)
        {
            if (DefaultShipMod == "Paladin")
            {
                // For Emperor ships, subtract Paladin cost and add new mod cost
                return resourceManager.GetShipModPrice(selectedMod) - defaultModCost;
            }
            return resourceManager.GetShipModPrice(selectedMod);
        }

        public int CalculateCumulativeSkullCost(string eliteRank)
        {
            int totalSkulls = 0;
            int targetRank = int.Parse(eliteRank.Replace("Elite ", ""));

            // Add up all skull costs up to and including the selected rank
            for (int i = 1; i <= targetRank; i++)
            {
                totalSkulls += EliteSkullCosts[$"Elite {i}"];
            }

            return totalSkulls;
        }

        public void AddFactionRequirement(string item, int quantity)
        {
            if (!FactionRequirements.ContainsKey(item))
            {
                FactionRequirements.Add(item, quantity);
            }
        }

        public void AddIngredient(string itemName, int quantity)
        {
            string key = itemName.ToLower();
            if (Ingredients.ContainsKey(key))
            {
                Ingredients[key] += quantity;
            }
            else
            {
                Ingredients.Add(key, quantity);
            }
        }

        public (Dictionary<string, int> materials, decimal creditCost) CalculateRequirements(
            int desiredQuantity, ResourceManager resourceManager)
        {
            Dictionary<string, int> materials = new Dictionary<string, int>();
            decimal totalCredits = 0m;

            foreach (var ingredient in Ingredients)
            {
                int requiredAmount = (int)Math.Ceiling((double)desiredQuantity / OutputQuantity) * ingredient.Value;
                materials.Add(ingredient.Key, requiredAmount);
                totalCredits += requiredAmount * resourceManager.GetResourcePrice(ingredient.Key);
            }

            return (materials, totalCredits);
        }

        public (Dictionary<string, int> materials, decimal creditCost) CalculateRequirementsWithElite(
            int desiredQuantity, ResourceManager resourceManager, string eliteRank)
        {
            // First get base requirements
            var (materials, totalCredits) = CalculateRequirements(desiredQuantity, resourceManager);

            // Add skull costs for elite rank if applicable
            if (eliteRank != "Elite 0")
            {
                int skullCost = CalculateCumulativeSkullCost(eliteRank);
                if (materials.ContainsKey("skull"))
                {
                    materials["skull"] += skullCost;
                }
                else
                {
                    materials.Add("skull", skullCost);
                }
                totalCredits += skullCost * resourceManager.GetResourcePrice("skull");
            }

            return (materials, totalCredits);
        }
    }
}