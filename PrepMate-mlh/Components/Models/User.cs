using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("PrepMateUsersMLH")]
public class User
{
    [DynamoDBHashKey]
    public string Email { get; set; }

    public string Name { get; set; }

    public string Password { get; set; } // Remember to hash this in a real application

    [DynamoDBProperty("SavedRecipes")]
    public List<Recipe> SavedRecipes { get; set; } = new List<Recipe>();
}