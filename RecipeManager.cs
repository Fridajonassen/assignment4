// Frida Jonassen
// 08/11/2018

namespace Assignment4
{
    public class RecipeManager
    {
        // Instance variables
        private Recipe[] recipeList;

        // Properties

        /// <summary>
        /// Initialize the object.
        /// </summary>
        /// <param name=maxNumOfElements>Maximum number of recipes allowed.</param>
        public RecipeManager(int maxNumOfElements)
        {
            recipeList = new Recipe[maxNumOfElements];
        }

        /// <summary>
        /// Add a new recipe.
        /// </summary>
        /// <param name=recipe>Recipe to add.</param>
        /// <returns>Boolean representing if the recipe was added.</returns>
        public bool AddRecipe(Recipe recipe)
        {
            bool ok = true;
            int empty_index = FindVacantPosition();

            if (CheckIndex(empty_index))
                recipeList[empty_index] = recipe;
            else
                ok = false;

            return ok;
        }

        /// <summary>
        /// Change a recipe by replacing it with another.
        /// </summary>
        /// <param name=index>Index of recipe to replace.</param>
        /// <param name=recipe>Changed recipe.</param>
        /// <returns>Boolean representing if the recipe was changed.</returns>
        public bool ChangeRecipeAt(int index, Recipe recipe)
        {
            bool ok = true;

            if (CheckIndex(index))
                recipeList[index] = recipe;
            else
                ok = false;

            return ok;
        }

        /// <summary>
        /// Check if index exists.
        /// </summary>
        /// <param name=index>Index to check.</param>
        /// <returns>Boolean representing if index exists.</returns>
        private bool CheckIndex(int index)
        {
            if ((index >= 0) && (index < recipeList.Length))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Get the number of recipes currently available.
        /// </summary>
        /// <returns>Current number of recipes.</returns>
        public int CurrentNumOfItems()
        {
            int count = 0;

            for (int i = 0; i < recipeList.Length; ++i) {
                if (recipeList[i] != null)
                    count++;
            }

            return count;

        }

        /// <summary>
        /// Delete recipe at index. Also left-shift the recipe array to
        /// fill the empty slot, we do this so we don't get get empty values
        /// between recipes because then we can simplyfy recipe
        /// selection a great deal in the GUI part of the program.
        /// </summary>
        /// <param name=index>Index of recipe to delete.</param>
        /// <returns>Boolean representing if the recipe was deleted.</returns>
        public bool DeleteRecipeAt(int index)
        {
            bool ok = true;

            if (CheckIndex(index))
            {
                for (int i = index; i < recipeList.Length; ++i)
                    if (i == (recipeList.Length - 1))
                        recipeList[i] = null;
                    else
                        recipeList[i] = recipeList[i + 1];
            }
            else
                ok = false;

            return ok;
        }

        /// <summary>
        /// Find an index in the recipe array that's empty.
        /// </summary>
        /// <returns>Empty index or -1 representing no empty indexes.</returns>
        private int FindVacantPosition()
        {
            for (int i = 0; i < recipeList.Length; i++) {
                if (recipeList[i] == null)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Get recipe at specifed index.
        /// </summary>
        /// <param name=index>Index of recipe to fetch.</param>
        /// <returns>Recipe found at index, or null if nothing was found.</returns>
        public Recipe GetRecipeAt(int index)
        {
            if (CheckIndex(index))
                return recipeList[index];
            else
                return null;
        }

        /// <summary>
        /// Create a formatted string with information about all available recipes.
        /// </summary>
        /// <returns>The formatted string.</returns>
        public string[] RecipeListToString()
        {
            string[] recipeListString = new string[CurrentNumOfItems()];
            int count = 0;

            for (int i = 0; i < recipeList.Length; ++i) {
                if (recipeList[i] != null)
                {
                    recipeListString[count] = recipeList[i].ToString();
                    count++;
                }
            }

            return recipeListString;
        }
    }
}
