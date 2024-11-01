using System;
using System.Linq;
using System.Threading.Tasks;
using PrepMate_mlh.Components.Pages;
using MudBlazor;
using System.Collections.Generic;

public class RecipeService
{
    private readonly IUserService _userService;
    private readonly ISnackbar _snackbar;

    public RecipeService(IUserService userService, ISnackbar snackbar)
    {
        _userService = userService;
        _snackbar = snackbar;
    }

    public async Task<bool> CheckIfRecipeIsSaved(Recipe recipe)
    {
        try
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser != null)
            {
                var savedRecipes = await _userService.GetSavedRecipesForUserAsync(currentUser.Email);
                return savedRecipes.Any(r => r.Name == recipe.Name);
            }
        }
        catch (Exception ex)
        {
            _snackbar.Add($"An error occurred: {ex.Message}", Severity.Error);
        }
        return false;
    }

    public async Task<bool> SaveRecipe(Recipe recipe)
    {
        try
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                _snackbar.Add("Please log in to save recipes", Severity.Warning);
                return false;
            }

            var success = await _userService.AddRecipeForUserAsync(currentUser.Email, recipe);
            if (success)
            {
                _snackbar.Add("Recipe saved successfully!", Severity.Success);
            }
            else
            {
                _snackbar.Add("Failed to save recipe or recipe already saved", Severity.Warning);
            }

            return success;
        }
        catch (Exception ex)
        {
            _snackbar.Add($"An error occurred: {ex.Message}", Severity.Error);
            return false;
        }
    }

    public async Task<bool> RemoveRecipe(Recipe recipe)
    {
        try
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                _snackbar.Add("Please log in to remove recipes", Severity.Warning);
                return false;
            }

            var success = await _userService.RemoveRecipeForUserAsync(currentUser.Email, recipe.Name);
            if (success)
            {
                _snackbar.Add("Recipe removed successfully!", Severity.Success);
            }
            else
            {
                _snackbar.Add("Failed to remove recipe or recipe not found", Severity.Warning);
            }

            return success;
        }
        catch (Exception ex)
        {
            _snackbar.Add($"An error occurred: {ex.Message}", Severity.Error);
            return false;
        }
    }

    private async Task<bool> UpdateAllSavedRecipes(string userEmail, List<Recipe> updatedRecipes)
    {
        try
        {
            bool success = true;
            foreach (var recipe in updatedRecipes)
            {
                success = success && await _userService.AddRecipeForUserAsync(userEmail, recipe);
            }
            return success;
        }
        catch (Exception)
        {
            return false;
        }
    }
}