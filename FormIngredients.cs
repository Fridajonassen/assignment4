// Frida Jonassen
// 08/11/2018

using System;
using System.Windows.Forms;

namespace Assignment4
{
    public partial class FormIngredients : Form
    {
        Recipe recipe;

        /// <summary>
        /// Create and initialize the Ingredient editor window.
        /// </summary>
        /// <param name=c_recipe>Recipe object where ingredients will be saved.</param>
        public FormIngredients(Recipe c_recipe)
        {
            InitializeComponent();

            recipe = c_recipe;
            if (string.IsNullOrEmpty(recipe.Name))
                this.Text = "Add ingredients";
            else
                this.Text = recipe.Name + " Add ingredients";

            InitializeGUI();
        }

        /// <summary>
        /// Initialize the GUI components of the Ingredients window.
        /// </summary>
        private void InitializeGUI()
        {
            if (recipe.Ingredients == null)
                recipe.Ingredients = new string[recipe.MaxNumOfIngredients];

            UpdateGUI();
        }

        /// <summary>
        /// Update the ingredients list.
        /// </summary>
        private void UpdateGUI()
        {
            lboxIngredients.Items.Clear();
            lboxIngredients.Items.AddRange(recipe.Ingredients);
            lblNumOfIngredients.Text = recipe.GetCurrentNumOfIngredients().ToString();
        }

        /// <summary>
        /// Reset the properties of the GUI elements to their initial state.
        /// </summary>
        private void ResetGUI()
        {
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Visible = true;
            btnSaveEdit.Visible = false;
            tboxIngredient.Clear();
            lboxIngredients.Enabled = true;

            UpdateGUI();
        }

        /// <summary>
        /// Add a new ingredient to the ingredient list.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tboxIngredient.Text))
                MessageBox.Show("Need to enter ingredients to add");
            else if (!recipe.AddIngredient(tboxIngredient.Text))
                MessageBox.Show("You have reached the maximum number of ingredients!");
            else
                ResetGUI();
        }

        /// <summary>
        /// Edit a marked entry in the ingredient list. Change element
        /// properties to make the user aware he is editing a ingredient.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lboxIngredients.SelectedItem == null)
                return;

            btnAdd.Visible = false;
            btnSaveEdit.Visible = true;
            tboxIngredient.Text = lboxIngredients.GetItemText(lboxIngredients.SelectedItem);
            lboxIngredients.Enabled = false;
        }

        /// <summary>
        /// Delete the selected ingriedient from the ingredient list.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lboxIngredients.SelectedItem == null)
                return;

            bool ok = recipe.DeleteIngredientAt(lboxIngredients.SelectedIndex);
            if (!ok)
                MessageBox.Show("Error! The index of the ingredient selected for deletion is out of range.");
            ResetGUI();
        }

        /// <summary>
        /// Enable edit and delete buttons when user have made a selection in
        /// the ingredient list.
        /// </summary>
        private void lboxIngredients_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
        }

        /// <summary>
        /// Save an edited ingredient to it's original index.
        /// </summary>
        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tboxIngredient.Text))
                MessageBox.Show("Need to enter ingredients to add");
            else
            {
                bool ok = recipe.ChangeIngredientAt(lboxIngredients.SelectedIndex,
                                                    tboxIngredient.Text);
                if (!ok)
                    MessageBox.Show("Error! The index of the edited ingredient is out of range.");
                ResetGUI();
            }
        }
    }
}
