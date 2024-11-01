public interface IUserService
{
    Task<User> GetCurrentUserAsync();
    Task SetCurrentUserAsync(User user);
    Task<User> AuthenticateUserAsync(string email, string password);
    Task<bool> RegisterUserAsync(User user);
    Task<bool> AddRecipeForUserAsync(string userEmail, Recipe recipe);
    Task<bool> RemoveRecipeForUserAsync(string userEmail, string recipeName);

    Task<List<Recipe>> GetSavedRecipesForUserAsync(string userEmail);
    Task ClearCurrentUserAsync();
    Task<User> GetUserAsync(string email);
}