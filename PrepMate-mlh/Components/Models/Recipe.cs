using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
public class Ingredient
{
    public string Name { get; set; }
    public string Portion { get; set; }
    public string Weight { get; set; }
}

public class Recipe
{
    public string Name { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public List<string> Instructions { get; set; }
    public string PrepTime { get; set; }
    public string CookTime { get; set; }
    public string TotalTime { get; set; }
    public string Servings { get; set; }
}

public class RecipeCollection
{
    public List<Recipe> Recipes { get; set; } = new List<Recipe>();
}



