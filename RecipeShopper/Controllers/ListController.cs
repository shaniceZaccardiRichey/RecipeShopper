using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeShopper.Data;
using RecipeShopper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeShopper.Controllers
{
    public class ListController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private ApplicationDbContext context { get; set; }

        public ListController(ApplicationDbContext ctx, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            context = ctx;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult FormList()
        {
            // Gets user ID for session, then calls method to get all recipes belonging to that ID.
            string userID = _userManager.GetUserId(HttpContext.User);

            // Get recipes for user.
            Recipe recipe = new Recipe();
            List<Recipe> recipes = recipe.GetAllRecipes(context, userID);

            List<Recipe> builtRecipes = new List<Recipe>();
            //RecipeViewModel recipeViewModel = new RecipeViewModel();
            //List<RecipeViewModel> recipeViewModels = new List<RecipeViewModel>();

            foreach (Recipe r in recipes)
            {

                Recipe builtRecipe = r.BuildExistingWithID(context, r.ID);

                builtRecipes.Add(builtRecipe);
            }

            return View(builtRecipes);
        }

        public IActionResult Generate()
        {
            return View();
        }

    }
}
