using RecipeShopper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeShopper.ViewModels
{
    public class RecipeViewModel
    {
        public Recipe Recipe { get; set; }
        public Ingredient Ingredient { get; set; }
        public RecipeIngredient RecipeIngredient { get; set; }
        public Direction Direction { get; set; }

        public List<string> ExistingNames { get; set; }

        public RecipeViewModel() { 
        
        }
    }
}
