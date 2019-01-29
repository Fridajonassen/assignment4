// Frida Jonassen   
// 08/11/2018

using System;

namespace Assignment4
{
    public class Recipe
    {
        // Instance variables
        private string[] ingredientArray;
        private string name;
        private FoodCategory category;
        private string description;

        // Properties
        public FoodCategory Category
        {
            get {return category;}
            set {category = value;}
        }

        public string Description
        {
            get {return description;}
            set {description = value;}
        }

        public string[] Ingredients
        {
            get {return ingredientArray;}
            set {ingredientArray = value;}
        }

        public int MaxNumOfIngredients
        {
            get {return ingredientArray.Length;}
        }

        public string Name
        {
            get {return name;}
            set {name = value;}
        }

        /// <summary>
        /// Initialize the object.
        /// </summary>
        /// <param name=maxNumOfIngredients>Maximum number of ingredients allowed.</param>
        public Recipe(int maxNumOfIngredients)
        {
            ingredientArray = new string[maxNumOfIngredients];
            DefaultValues();
        }

        /// <summary>
        /// Set default values for recipe.
        /// </summary>
        private void DefaultValues()
        {
            for (int i = 0; i < ingredientArray.Length; i++)
                ingredientArray[i] = string.Empty;

            name = string.Empty;
            category = FoodCategory.Meat;
            description = string.Empty;
        }

        /// <summary>
        /// Add's ingredient to recipe.
        /// </summary>
        /// <param name=ingredient>Ingredient to add.</param>
        /// <returns>Boolean representing if ingredient was added.</returns>
        public bool AddIngredient(string ingredient)
        {
            bool ok = true;
            int empty_index = FindVacantPosition();

            if (CheckIndex(empty_index))
                ingredientArray[empty_index] = ingredient;
            else
                ok = false;

            return ok;
        }

        /// <summary>
        /// Change ingredient in recipe.
        /// </summary>
        /// <param name=index>Index of ingredient to change.</param>
        /// <param name=value>Value to change ingredient to.</param>
        /// <returns>Boolean representing if ingredient was changed.</returns>
        public bool ChangeIngredientAt(int index, string value)
        {
            bool ok = true;

            if (CheckIndex(index))
                ingredientArray[index] = value;
            else
                ok = false;
            return ok;
        }

        /// <summary>
        /// Check if index exists.
        /// </summary>
        /// <param name=index>Index to check.</param>
        /// <returns>Boolean representing if available.</returns>
        private bool CheckIndex(int index)
        {
            if ((index >= 0) && (index < ingredientArray.Length))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Delete ingredient at index. Also left-shift the ingredient array to
        /// fill the empty slot, we do this so we don't get get empty values
        /// between ingredients because then we can simplyfy ingredient
        /// selection a great deal in the GUI part of the program.
        /// </summary>
        /// <param name=index>Index of ingredient to delete.</param>
        /// <returns>Boolean representing if the ingredient was deleted.</returns>
        public bool DeleteIngredientAt(int index)
        {
            bool ok = true;

            if (CheckIndex(index))
            {
                for (int i = index; i < ingredientArray.Length; ++i)
                    if (i == (ingredientArray.Length - 1))
                        ingredientArray[i] = String.Empty;
                    else
                        ingredientArray[i] = ingredientArray[i + 1];
            }
            else
                ok = false;

            return ok;
        }

        /// <summary>
        /// Find an index in the ingredient array that's empty.
        /// </summary>
        /// <returns>Empty index or -1 representing no empty indexes.</returns>
        private int FindVacantPosition()
        {
            for (int i = 0; i < ingredientArray.Length; i++) {
                if (ingredientArray[i] == string.Empty)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Get the number of ingredients available.
        /// </summary>
        /// <returns>Current number of ingredients.</returns>
        public int GetCurrentNumOfIngredients()
        {
            int count = 0;

            for (int i = 0; i < ingredientArray.Length; ++i) {
                if (!string.IsNullOrEmpty(ingredientArray[i]))
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Create a formatted string with information about the recipe.
        /// </summary>
        /// <returns>The formatted string.</returns>
        public override string ToString()
        {
            int chars = Math.Min(description.Length, 15);
            string descriptionText = description.Substring(0, chars);

            if (string.IsNullOrEmpty(descriptionText))
                descriptionText = "Description is missing";

            string textOut = string.Format("{0, -20} {1, 4}       {2, -12}   {3, -15}",
                                           name, GetCurrentNumOfIngredients(),
                                           category.ToString(), descriptionText);
            return textOut;
        }
    }
}
