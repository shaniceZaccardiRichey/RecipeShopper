using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeShopper.Data;
using RecipeShopper.Models;
using RecipeShopper.ViewModels;

namespace RecipeShopper.Controllers
{
    public class RecipeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private ApplicationDbContext context { get; set; }

        public RecipeController(ApplicationDbContext ctx, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            context = ctx;
        }

        [Authorize]
        public IActionResult MyRecipes()
        {
            // Gets user ID for session, then calls method to get all recipes belonging to that ID.
            string userID = _userManager.GetUserId(HttpContext.User);

            // Get recipes for user.
            Recipe recipe = new Recipe();
            List<Recipe> recipes = recipe.GetAllRecipes(context, userID);

            List<Recipe> builtRecipes = new List<Recipe>();
            //RecipeViewModel recipeViewModel = new RecipeViewModel();
            //List<RecipeViewModel> recipeViewModels = new List<RecipeViewModel>();

            foreach (Recipe r in recipes) {

                Recipe builtRecipe = r.BuildExistingWithID(context, r.ID);

                builtRecipes.Add(builtRecipe);
            }

            return View(builtRecipes);
        }

        public IActionResult Details(int id)
        {
            Recipe recipe = new Recipe();
            recipe = recipe.BuildExistingWithID(context, id);

            return View(recipe);

        }

        public IActionResult New() {
            return View();
        }

        [HttpPost]
        public IActionResult New(string name)
        {
            System.Diagnostics.Debug.WriteLine("test");

            ViewBag.tempName = name;

            string userID = _userManager.GetUserId(HttpContext.User);

            Recipe recipe = new Recipe();
            List<string> names = recipe.GetAllRecipeNames(context, userID);

            bool exists = false;

            foreach (string n in names) {
                System.Diagnostics.Debug.WriteLine(n + "," + name);
                if (name == n) {
                    exists = true;
                }
            }

            System.Diagnostics.Debug.WriteLine(exists);

            if (exists == true)
            {
                return View();
            }
            else {
                return Redirect("/Recipe/Add");
            }
        }

        public IActionResult Add()
        {
            string userID = _userManager.GetUserId(HttpContext.User);

            Recipe recipe = new Recipe();

            List<string> names = recipe.GetAllRecipeNames(context, userID);

            // Add names to session variable?

            return View();
        }

        [HttpPost]
        public IActionResult Add(Recipe newRecipe, string[] ingredientName, string[] ingredientQty, string[] ingredientUnit, string[] direction)
        {
            // Validates, gets user ID to complete character model, then calls method to save character.
            newRecipe.UserID = _userManager.GetUserId(HttpContext.User);
            System.Diagnostics.Debug.WriteLine("hits");

            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("isValid");
 
                System.Diagnostics.Debug.WriteLine(newRecipe);

                // Save recipe.
                newRecipe.SaveRecipe(context, newRecipe);


                // Get Recipe ID.
                Recipe buildRecipe = newRecipe.GetRecipeByName(context, newRecipe.Name);

                // Create RecipeIngredient List
                List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();

                // Check for new Ingredients, save.
                foreach (string name in ingredientName)
                {

                    // Get ingredient for ID. (by name)
                    Ingredient ingredient = new Ingredient() { 
                        Name = name
                    };

                    int ingredientID = ingredient.GetID(context, name);

                    if (ingredientID == 0) {
                        ingredient.Save(context, ingredient);
                    }
                }

                for (int i = 0; i < ingredientName.Length; i++) {

                    // Get ingredient ID, build objects. 
                    Ingredient ingredient = new Ingredient();
                    int ingredientID = ingredient.GetID(context, ingredientName[i]);

                    // Build RecipeIngredient
                    RecipeIngredient recipeIngredient = new RecipeIngredient() {
                        IngredientID = ingredientID,
                        RecipeID = buildRecipe.ID,
                        MeasurementUnit = ingredientUnit[i],
                        Quantity = Convert.ToDouble(ingredientQty[i])
                    };

                    // Push to list.
                    recipeIngredients.Add(recipeIngredient);
                }

                System.Diagnostics.Debug.WriteLine(recipeIngredients);

                // Create Direction list.
                List<Direction> recipeDirections = new List<Direction>();

                // Build directions.
                for (int i = 0; i < direction.Length; i++) {
                    // Create direction object.
                    Direction newDirection = new Direction()
                    {
                        RecipeID = buildRecipe.ID,
                        StepNumber = i + 1,
                        DirectionContent = direction[i]
                    };

                    recipeDirections.Add(newDirection);
                }

                System.Diagnostics.Debug.WriteLine(recipeDirections);

                buildRecipe.Ingredients = recipeIngredients;
                buildRecipe.Directions = recipeDirections;

                buildRecipe.UpdateRecipe(context, buildRecipe, newRecipe.UserID);

                return Redirect("/Recipe/MyRecipes");
            }
            else
            {
                return View(newRecipe);
            }


        }

        public IActionResult Edit(int id)
        {

            Recipe recipe = new Recipe();
            recipe = recipe.BuildExistingWithID(context, id);

            return View(recipe);

        }

        public IActionResult Delete(int id) {

            Recipe recipe = new Recipe();
            recipe = recipe.BuildExistingWithID(context, id);

            return View(recipe);
        
        }

        [HttpPost]
        public IActionResult Delete(Recipe recipe, string delete, string cancel) {
            if (!string.IsNullOrEmpty(delete))
            {
                recipe.DeleteRecipe(context, recipe);
            }
            if (!string.IsNullOrEmpty(cancel))
            {

            }
            return Redirect("/Recipe/MyRecipes");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}