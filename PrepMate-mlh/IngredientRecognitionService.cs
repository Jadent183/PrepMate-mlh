using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

public class IngredientRecognitionService
{
    private readonly IAmazonRekognition _rekognitionClient;
    private readonly ILogger<IngredientRecognitionService> _logger;
    private readonly string _customModelArn = "arn:aws:rekognition:us-west-2:686255967361:project/Ingredients/version/Ingredients.2024-09-29T03.41.46/1727606507669";

    public IngredientRecognitionService(
        IAmazonRekognition rekognitionClient,
        ILogger<IngredientRecognitionService> logger,
        IConfiguration configuration)
    {
        _rekognitionClient = rekognitionClient;
        _logger = logger;
        _customModelArn = "arn:aws:rekognition:us-west-2:686255967361:project/Ingredients/version/Ingredients.2024-09-29T03.41.46/1727606507669";
    }

    public async Task<List<string>> RecognizeIngredientsAsync(byte[] imageBytes)
    {
        try
        {
            _logger.LogInformation("Starting custom image recognition. Image size: {Size} bytes", imageBytes.Length);
            var detectCustomLabelsRequest = new DetectCustomLabelsRequest
            {
                Image = new Image
                {
                    Bytes = new MemoryStream(imageBytes)
                },
                ProjectVersionArn = _customModelArn,
                MaxResults = 10,
                MinConfidence = 5F
            };

            _logger.LogInformation("Sending request to AWS Rekognition Custom Labels");
            DetectCustomLabelsResponse detectCustomLabelsResponse;
            try
            {
                detectCustomLabelsResponse = await _rekognitionClient.DetectCustomLabelsAsync(detectCustomLabelsRequest);
                _logger.LogInformation("Rekognition Custom Labels API call successful. Labels detected: {LabelCount}", detectCustomLabelsResponse.CustomLabels.Count);
            }
            catch (AmazonRekognitionException are)
            {
                _logger.LogError(are, "AWS Rekognition specific error occurred");
                throw;
            }
            catch (AmazonServiceException ase)
            {
                _logger.LogError(ase, "An error occurred with the AWS service");
                _logger.LogError("Error Code: {ErrorCode}, Error Type: {ErrorType}", ase.ErrorCode, ase.ErrorType);
                throw;
            }
            catch (AmazonClientException ace)
            {
                _logger.LogError(ace, "An error occurred with the AWS SDK client");
                throw;
            }

            var ingredients = detectCustomLabelsResponse.CustomLabels
                .Where(l => l.Confidence > 0)
                .Select(l => l.Name)
                .ToList();

            _logger.LogInformation("Filtered ingredients: {IngredientCount}", ingredients.Count);
            return ingredients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error detecting custom labels");
            throw; // Re-throw the exception to be handled by the caller
        }
    }
}