using Microsoft.JSInterop;

public class UserService : IUserService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly DynamoDBService _dynamoDBService;

    public UserService(IJSRuntime jsRuntime, DynamoDBService dynamoDBService)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        _dynamoDBService = dynamoDBService ?? throw new ArgumentNullException(nameof(dynamoDBService));
    }

    public async Task<bool> AddRecipeForUserAsync(string userEmail, Recipe recipe)
    {
        try
        {
            var user = await _dynamoDBService.LoadItemAsync<User>(userEmail);
            if (user == null) return false;

            user.SavedRecipes ??= new List<Recipe>();

            if (!user.SavedRecipes.Any(r => r.Name == recipe.Name))
            {
                user.SavedRecipes.Add(recipe);
                await _dynamoDBService.SaveItemAsync(user);
                return true;
            }

            return false; // Recipe already exists
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> RemoveRecipeForUserAsync(string userEmail, string recipeName)
    {
        try
        {
            var user = await _dynamoDBService.LoadItemAsync<User>(userEmail);
            if (user == null) return false;

            var recipeToRemove = user.SavedRecipes?.FirstOrDefault(r => r.Name == recipeName);
            if (recipeToRemove != null)
            {
                user.SavedRecipes.Remove(recipeToRemove);
                await _dynamoDBService.SaveItemAsync(user);
                return true;
            }

            return false; // Recipe not found
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<Recipe>> GetSavedRecipesForUserAsync(string userEmail)
    {
        var user = await _dynamoDBService.LoadItemAsync<User>(userEmail);
        return user?.SavedRecipes ?? new List<Recipe>();
    }

    public async Task<User> GetUserAsync(string email)
    {
        return await _dynamoDBService.LoadItemAsync<User>(email);
    }

    public async Task<User> GetCurrentUserAsync()
    {
        var userJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "currentUser");
        return string.IsNullOrEmpty(userJson) ? null : System.Text.Json.JsonSerializer.Deserialize<User>(userJson);
    }

    public async Task SetCurrentUserAsync(User user)
    {
        var userJson = System.Text.Json.JsonSerializer.Serialize(user);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "currentUser", userJson);
    }

    public async Task<User> AuthenticateUserAsync(string email, string password)
    {
        var user = await _dynamoDBService.LoadItemAsync<User>(email);
        return user?.Password == password ? user : null;
    }

    public async Task<bool> RegisterUserAsync(User user)
    {
        var existingUser = await _dynamoDBService.LoadItemAsync<User>(user.Email);
        if (existingUser != null)
        {
            return false;
        }
        await _dynamoDBService.SaveItemAsync(user);
        return true;
    }

    public async Task ClearCurrentUserAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "currentUser");
    }
}