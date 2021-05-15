using RecipeShopper.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeShopper.Models
{
    public class RecipeIngredient
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please select a measurement unit.")]
        [EnumDataType(typeof(Units))]
        public string MeasurementUnit { get; set; }
        [Required(ErrorMessage = "Please enter a quantity.")]
        public double Quantity { get; set; }

        [Required]
        public int RecipeID { get; set; } // Foreign Key
        public Recipe Recipe { get; set; }

        [Required]
        public int IngredientID { get; set; } // Foreign Key
        public Ingredient Ingredient { get; set; }

        //[NotMapped]
        //private ApplicationDbContext context { get; set; }

        public RecipeIngredient()
        {

        }

        // Get all recipes using UserID.
        public List<RecipeIngredient> GetAllByRecipeID(ApplicationDbContext ctx, int recipeID)
        {
            List<RecipeIngredient> recipeIngredients = ctx.RecipeIngredients
                .Where(r => r.RecipeID == recipeID)
                .ToList();

            return recipeIngredients;
        }
    }

    // Enums to restrict input.
    public enum Units
    {
        Teaspoon,
        Tablespoon,
        FlOz,
        Cup,
        Pint,
        Quart,
        Gallon,
        mL,
        Litre,
        dL,
        Pound,
        Ounce,
        mg,
        g,
        kg,
        Can,
        Bag,
        Piece
    };
}
