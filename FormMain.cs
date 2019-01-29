// Frida Jonassen
// 08/11/2018

using System;
using System.Windows.Forms;

namespace Assignment4
{
    public partial class FormMain : Form
    {
        // Instance variables
        private const int maxNumOfIngredients = 20;
        private const int maxNumOfRecipes = 50;

        Recipe currRecipe = new Recipe(maxNumOfIngredients);
        RecipeManager recipeManager = new RecipeManager(maxNumOfRecipes);

        /// <summary>
        /// Create and initialize the main window.
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            InitializeGUI();
        }

        /// <summary>
        /// Initialize the GUI components of the main window.
        /// </summary>
        private void InitializeGUI()
        {
            cboxCategory.DataSource = Enum.GetValues(typeof(FoodCategory));
            SyncRecipeToGUI();
        }

        /// <summary>
        /// Make the GUI reflect the state of the curr_recipe object.
        /// </summary>
        private void SyncRecipeToGUI()
        {
            cboxCategory.SelectedIndex = cboxCategory.FindString(currRecipe.Category.ToString());
            txtName.Text = currRecipe.Name;
            tboxDescription.Text = currRecipe.Description;
        }

        /// <summary>
        /// Update the GUI.
        /// </summary>
        private void UpdateGUI()
        {
            string[] recipeListStrings = recipeManager.RecipeListToString();
            lboxRecipes.Items.Clear();
            lboxRecipes.Items.AddRange(recipeListStrings);
        }

        /// <summary>
        /// Reset the properties of the GUI to their initial state.
        /// </summary>
        private void ResetGUI()
        {
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            lboxRecipes.Enabled = true;
            btnSaveChanges.Visible = false;
            btnAddRecipe.Visible = true;

            currRecipe = new Recipe(maxNumOfIngredients);
            SyncRecipeToGUI();
            UpdateGUI();
        }

        /// <summary>
        /// Validates user input. Prints helpful message when input is missing.
        /// </summary>
        private bool CheckInput()
        {
            bool ok = false;
            if (string.IsNullOrEmpty(txtName.Text))
                MessageBox.Show("No name specifed!");
            else if (string.IsNullOrEmpty(tboxDescription.Text))
                MessageBox.Show("No description specifed!");
            else if (currRecipe.GetCurrentNumOfIngredients() <= 0)
                MessageBox.Show("No ingredients specifed!");
            else
                ok = true;

            return ok;
        }

        /// <summary>
        /// Make curr_recipe reflect the state of GUI.
        /// </summary>
        private bool SyncGUIToRecipe()
        {
            bool ok = CheckInput();

            if (ok)
            {
                currRecipe.Name = txtName.Text;
                currRecipe.Description = tboxDescription.Text;
                currRecipe.Category = (FoodCategory)cboxCategory.SelectedValue;
            }

            return ok;
        }

        /// <summary>
        /// Opens up a window that let's the user edit the ingredient list for
        /// the current recipe. Changes are only saved if the user clicks ok.
        /// </summary>
        private void btnAddIngredients_Click(object sender, EventArgs e)
        {
            FormIngredients dlg = new FormIngredients(currRecipe);
            DialogResult dlgResult = dlg.ShowDialog();
            string[] old_ingredients = (string[]) currRecipe.Ingredients.Clone();

            if (dlgResult == DialogResult.OK)
                if (currRecipe.GetCurrentNumOfIngredients() <= 0)
                    MessageBox.Show("No ingriedients specified!");
            else
                currRecipe.Ingredients = old_ingredients;
        }

        /// <summary>
        /// Adds current recipe to recipe list.
        /// </summary>
        private void btnAddRecipe_Click(object sender, EventArgs e)
        {
            if (!SyncGUIToRecipe())
                return;

            bool ok = recipeManager.AddRecipe(currRecipe);
            if (!ok)
                MessageBox.Show("You have reached the maximum number of recipes!");

            ResetGUI();
        }

        /// <summary>
        /// Enters information from the selected recipe to the GUI. Also enables
        /// editing and delete buttons.
        /// </summary>
        private void lboxRecipes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lboxRecipes.SelectedItem == null)
                return;

            Recipe selected = recipeManager.GetRecipeAt(lboxRecipes.SelectedIndex);
            currRecipe = new Recipe(maxNumOfIngredients) {
                Name = selected.Name,
                Description = selected.Description,
                Category = selected.Category,
                Ingredients = selected.Ingredients.Clone() as string[]};

            SyncRecipeToGUI();
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
        }

        /// <summary>
        /// Delete the selected recipe from the recipe list.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool ok = recipeManager.DeleteRecipeAt(lboxRecipes.SelectedIndex);
            if (!ok)
                MessageBox.Show("Error! The index of the recipe selected for deletion is out of range.");
            ResetGUI();
        }

        /// <summary>
        /// Edit the selected recipe. Modify GUI elements so user is aware he is
        /// editing and not adding the recipe.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            SyncRecipeToGUI();
            btnSaveChanges.Visible = true;
            btnAddRecipe.Visible = false;
            lboxRecipes.Enabled = false;
            btnEdit.Enabled = false;
        }

        /// <summary>
        /// Save changes to the edited recipe.
        /// </summary>
        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (!SyncGUIToRecipe())
                return;

            bool ok = recipeManager.ChangeRecipeAt(lboxRecipes.SelectedIndex, currRecipe);
            if (!ok)
                MessageBox.Show("Error! The index of the edited recipe is out of range.");

            ResetGUI();
        }
    }
}
