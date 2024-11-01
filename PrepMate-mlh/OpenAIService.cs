using OpenAI_API;
using OpenAI_API.Chat;

public class OpenAIService
{
    private readonly OpenAIAPI _api;

    public OpenAIService(IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"];
        _api = new OpenAIAPI(apiKey);
    }

    public async Task<string> GenerateRecipe(string prompt)
    {
        var chatRequest = new ChatRequest
        {
            Model = "gpt-4o-mini",
            Temperature = 0.7,
            MaxTokens = 3500,
            Messages = new List<ChatMessage>
            {
                new ChatMessage(ChatMessageRole.System, "You are a professional chef and recipe developer. Your task is to create at least three and up to six detailed recipes based on a list of ingredients and an optional category provided by the user. Each recipe should include:\r\n\r\nA descriptive and appetizing title.\r\n\r\nA list of ingredients with precise measurements where each ingredient is represented by its \"name\", \"portion\", and \"weight\" (use \"g\" for grams) fields.\r\n\r\nStep-by-step cooking instructions that are clear and easy to follow.\r\n\r\nNot all the provided ingredients are required in each of the recipes. If necessary, you may assume the availability of basic pantry items like salt, pepper, oil, and common spices but make sure to include \"optional\" in the portion section and \"as needed\" in the weight section.\r\n\r\nI want the output in the following JSON format:\r\n\r\n{\r\n  \"recipes\": [\r\n    {\r\n      \"name\": \"Recipe Name\",\r\n      \"ingredients\": [\r\n        {\r\n          \"name\": \"Ingredient 1\",\r\n          \"portion\": \"X amount\"\r\n         \"weight\":\"X g\"\r\n        },\r\n        {\r\n          \"name\": \"Ingredient 2\",\r\n          \"portion\": \"Y amount\"\r\n         \"weight\":\"Y g\"\r\n        },\r\n        \"... (more ingredients)\"\r\n      ],\r\n      \"instructions\": [\r\n        \"Step 1: Detailed instruction.\",\r\n        \"Step 2: Detailed instruction.\",\r\n        \"... (more steps)\"\r\n      ],\r\n      \"prep_time\": \"X minutes\",\r\n      \"cook_time\": \"Y minutes\",\r\n      \"total_time\": \"Z minutes\",\r\n      \"servings\": \"Number of servings\"\r\n    },\r\n    { (Next Recipe follows the same format) }\r\n  ]\r\n}\r\n\r\nMake sure that the recipe name, ingredients, and instructions are all filled out completely to the fullest extent. Include the prep time, cook time, total time, and number of servings for each recipe.\r\n\r\n"),
                new ChatMessage(ChatMessageRole.User, prompt)
            }
        };

        var result = await _api.Chat.CreateChatCompletionAsync(chatRequest);
        return result.Choices[0].Message.Content;
    }
}