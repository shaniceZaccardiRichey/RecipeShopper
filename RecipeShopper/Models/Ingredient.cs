using RecipeShopper.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeShopper.Models
{
    public class Ingredient
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please enter an ingredient name.")]
        public string Name { get; set; }

        public Ingredient() { 
        
        }

        public Ingredient GetByID(ApplicationDbContext ctx, int ingredientID) {
            Ingredient ingredient = ctx.Ingredients
                .Where(i => i.ID == ingredientID)
                .FirstOrDefault();
            return ingredient;
        }

        public int GetID(ApplicationDbContext ctx, string name) {

            int id;

            Ingredient ingredient = ctx.Ingredients
                .Where(i => i.Name == name)
                .FirstOrDefault();

            if (ingredient == null)
            {
                id = 0;
            }
            else {
                id = ingredient.ID;
            }

            return id;
        }

        public void Save(ApplicationDbContext ctx, Ingredient ingredient) {
            ctx.Add(ingredient);
            ctx.SaveChanges();
        }


    }
}
