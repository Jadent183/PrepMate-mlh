using Newtonsoft.Json.Linq;

public class RecipeParser
{
    public static JObject ParseJson(string jsonString)
    {
        try
        {
            // Remove any leading/trailing whitespace
            jsonString = jsonString.Trim();

            // Remove code block indicators if present
            if (jsonString.StartsWith("```json"))
            {
                jsonString = jsonString.Substring(7);
            }
            if (jsonString.EndsWith("```"))
            {
                jsonString = jsonString.Substring(0, jsonString.Length - 3);
            }

            // Remove any extra quotation marks at the beginning and end
            jsonString = jsonString.Trim('"');

            // Attempt to parse the JSON
            return JObject.Parse(jsonString);
        }
        catch (Newtonsoft.Json.JsonException ex)
        {
            // Log the exception or handle it as needed
            Console.WriteLine($"Error parsing JSON: {ex.Message}");
            return null;
        }
    }

    public static RecipeCollection ParseRecipes(string jsonString) 
    {
        JObject jsonObject = ParseJson(jsonString);
        RecipeCollection recipeCollection = new RecipeCollection
        {
            Recipes = new List<Recipe>()
        };

        JArray recipesArray = (JArray)jsonObject["recipes"];

        foreach (var recipeToken in recipesArray)
        {
            Recipe recipe = new Recipe
            {
                Name = recipeToken["name"].ToString(),
                Ingredients = new List<Ingredient>(),
                Instructions = new List<string>(),
                PrepTime = recipeToken["prep_time"].ToString(),
                CookTime = recipeToken["cook_time"].ToString(),
                TotalTime = recipeToken["total_time"].ToString(),
                Servings = recipeToken["servings"].ToString()
            };

            JArray ingredientsArray = (JArray)recipeToken["ingredients"];
            foreach (var ingredientToken in ingredientsArray)
            {
                Ingredient ingredient = new Ingredient
                {
                    Name = ingredientToken["name"].ToString(),
                    Portion = ingredientToken["portion"].ToString(),
                    Weight = ingredientToken["weight"].ToString()
                };
                recipe.Ingredients.Add(ingredient);
            }

            JArray instructionsArray = (JArray)recipeToken["instructions"];
            foreach (var instructionToken in instructionsArray)
            {
                recipe.Instructions.Add(instructionToken.ToString());
            }

            recipeCollection.Recipes.Add(recipe);
        }

        return recipeCollection;
    }

}
