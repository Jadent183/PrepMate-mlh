using PrepMate_mlh.Components;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Amazon.DynamoDBv2;
using Amazon;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime.CredentialManagement;
using Microsoft.AspNetCore.Components.Web;
using Amazon.Rekognition;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient();


var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Region = RegionEndpoint.USWest2;

var chain = new CredentialProfileStoreChain();
if (chain.TryGetAWSCredentials("default", out var awsCredentials))
{
    awsOptions.Credentials = awsCredentials;
}
else
{
    awsOptions.Credentials = new Amazon.Runtime.AnonymousAWSCredentials();
}
builder.Services.AddAWSService<IAmazonDynamoDB>(awsOptions);
builder.Services.AddScoped<IDynamoDBContext>(provider =>
{
    var client = provider.GetRequiredService<IAmazonDynamoDB>();
    return new DynamoDBContext(client);
});
builder.Services.AddScoped<DynamoDBService>();

builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<Program>>();
    try
    {
        var client = awsOptions.CreateServiceClient<IAmazonDynamoDB>();
        // Test the connection
        client.ListTablesAsync().GetAwaiter().GetResult();
        return client;
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to create DynamoDB client. Please check your AWS credentials and region settings.");
        throw new InvalidOperationException("Failed to initialize AWS services. Please check your configuration and try again.", ex);
    }
});

builder.Services.AddTransient<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddTransient<DynamoDBService>();
builder.Services.AddAWSService<IAmazonRekognition>(awsOptions);
builder.Services.AddScoped<IngredientRecognitionService>();



// Custom Services
builder.Services.AddMudServices();
builder.Services.AddScoped<OpenAIService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
