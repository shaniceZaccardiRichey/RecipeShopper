using Microsoft.AspNetCore.Identity;
using RecipeShopper.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeShopper.Models
{
    public class Recipe
    {
        public int ID { get; set; }

        
        [Required(ErrorMessage = "Please enter a name.")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Please enter at least one ingredient.")]
        public List<RecipeIngredient> Ingredients { get; set; }
        //[Required(ErrorMessage = "Please enter at least one direction.")]
        public List<Direction> Directions { get; set; }
        public int Servings { get; set; }
        public double Cost { get; set; }
        [Required(ErrorMessage = "Please select a meal category.")]
        [EnumDataType(typeof(MealCategory))]
        public string MealCategory { get; set; }

        public string UserID { get; set; } // Foreign Key
        public IdentityUser User { get; set; }

        [NotMapped]
        private ApplicationDbContext context { get; set; }

        public Recipe() { 
        
        }

        // Get all recipes using UserID.
        public List<Recipe> GetAllRecipes(ApplicationDbContext ctx, string userID)
        {
            List<Recipe> recipes = ctx.Recipes
                .Where(r => r.UserID == userID)
                .ToList();

            return recipes;
        }

        public List<string> GetAllRecipeNames(ApplicationDbContext ctx, string userID)
        {
            List<string> names = ctx.Recipes
                .Where(r => r.UserID == userID)
                .Select(r => r.Name)
                .ToList();

            return names;
        }

        public Recipe BuildExistingWithID(ApplicationDbContext ctx, int id) {

            Recipe recipe = new Recipe();
            recipe = recipe.GetRecipeByID(ctx, id);

            // get RecIngs using RecID.
            // save to Recipe.
            // get Dirs using RecID.
            // save to Recipe

            // Get RecipeIngredients, add to Recipe onject.
            RecipeIngredient recipeIngredient = new RecipeIngredient();
            List<RecipeIngredient> recipeIngredients = recipeIngredient.GetAllByRecipeID(ctx, id);

            foreach (RecipeIngredient ri in recipeIngredients)
            {
                // Get ingredient by RecIngID
                // Save to RecIng.

                Ingredient ingredient = new Ingredient();
                ingredient = ingredient.GetByID(ctx, ri.IngredientID);

                ri.Ingredient = ingredient;

            }


            recipe.Ingredients = recipeIngredients;

            // Get Directions, add to Recipe onject.
            Direction direction = new Direction();
            List<Direction> directions = direction.GetAllByRecipeID(ctx, id);

            recipe.Directions = directions;

            return recipe;
        }

        // Get recipe using ID.
        public Recipe GetRecipeByID(ApplicationDbContext ctx, int id)
        {
            Recipe recipe = ctx.Recipes
                .Where(r => r.ID == id)
                .FirstOrDefault();
            return recipe;
        }

        // Get recipe by name.
        public Recipe GetRecipeByName(ApplicationDbContext ctx, string name)
        {
            Recipe recipe = ctx.Recipes
                .Where(r => r.Name == name)
                .FirstOrDefault();
            return recipe;
        }

        // Check for existing recipe by name.
        public bool CheckExistingByName(string name) {

            Recipe existingRecipe = new Recipe();
            existingRecipe = existingRecipe.GetRecipeByName(context, name);

            if (existingRecipe == null)
            {
                return false; // Doesn't exist.
            }
            else
            {
                return true; // Exists
            }

        }

        // Save recipe.
        public void SaveRecipe(ApplicationDbContext ctx, Recipe recipe)
        {
            /*
            Recipe existingRecipe = new Recipe();
            existingRecipe = existingRecipe.GetRecipeByName(ctx, recipe.Name);

            if (!(existingRecipe == null))
            {
                return false;
            }
            else
            {
                return true;
            }
            */

            ctx.Add(recipe);
            ctx.SaveChanges();

        }

        // Update recipe.
        public void UpdateRecipe(ApplicationDbContext ctx, Recipe recipe, string userID)
        {
            recipe.UserID = userID;

            ctx.Update(recipe);
            ctx.SaveChanges();
        }
        
        // Delete recipe.
        public void DeleteRecipe(ApplicationDbContext ctx, Recipe recipe)
        {
            ctx.Remove(recipe);
            ctx.SaveChanges();
        }
    }

    // Enums to restrict input.
    public enum MealCategory
    {
        Breakfast,
        Lunch,
        Dinner,
        Dessert,
        Snack,
        Beverage
    };

}
