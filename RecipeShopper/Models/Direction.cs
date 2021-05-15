using RecipeShopper.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeShopper.Models
{
    public class Direction
    {
        public int ID { get; set; }
        public int StepNumber { get; set; }
        public string DirectionContent { get; set; }

        public int RecipeID { get; set; } // Foreign Key
        public Recipe Recipe { get; set; }


        //[NotMapped]
        //private ApplicationDbContext context { get; set; }

        public Direction()
        {

        }

        // Get all directions using RecipeID.
        public List<Direction> GetAllByRecipeID(ApplicationDbContext context, int recipeID)
        {
            List<Direction> directions = context.Directions
                .Where(r => r.RecipeID == recipeID)
                .ToList();

            return directions;
        }
    }
}
