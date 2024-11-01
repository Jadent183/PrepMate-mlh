# PrepMate

**PrepMate** is an AI-powered recipe-generating web application, designed and developed over a 24-hour period at SunHacks, where it won 2nd place for "Best Use of AWS." This web app allows users to input ingredients and generate recipes using AI, track nutritional information, and even scan ingredients using AWS Rekognize for recipe suggestions.

## Features

- **AI Recipe Generation**: Users can input ingredients, and PrepMate will generate creative, personalized recipes using ChatGPT.
  
- **Nutritional Information**: Each recipe includes detailed nutritional information (calories, macros, etc.) using the Nutritionix API, helping users track their diet more accurately.

- **AWS Rekognize Integration**: Users can snap a photo of their ingredients, and PrepMate will utilize a custom-trained AWS Rekognize model to identify the ingredients and generate recipe suggestions accordingly.

- **User Accounts & Recipe Saving**: Users can create accounts to save their favorite recipes for easy access later. All account data and saved recipes are stored securely using AWS DynamoDB.

## Tech Stack

- **Frontend**: C#, Blazor, MudBlazor (UI)
- **AI Integration**: ChatGPT for recipe generation
- **Nutritional API**: Nutritionix API for tracking macros and calories
- **Image Recognition**: AWS Rekognize for ingredient identification
- **Database**: AWS DynamoDB for user account management and recipe storage

## Setup

### Prerequisites
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

### Installation
1. Clone this repository:
   ```bash
   git clone https://github.com/your-username/prepmate.git
   cd prepmate
   ```

2. Install dependencies:
   ```bash
   dotnet restore
   ```

3. Configure AWS Rekognize and DynamoDB in the app settings.

4. Add your Nutritionix API key to the environment variables or configuration file.

5. Run the application:
   ```bash
   dotnet run
   ```

## Usage
- **Input Ingredients**: Manually input ingredients or upload a photo for recipe suggestions.
- **View Recipes**: Explore AI-generated recipes based on your ingredients.
- **Track Nutrition**: Get detailed nutritional breakdowns for each recipe.
- **Save Recipes**: Create an account to save your favorite recipes for later access.

## Screenshots

![Screenshot 2024-09-29 105228](https://github.com/user-attachments/assets/16a657df-8195-40c1-a2c4-0e1aa0671b1e)

![Screenshot 2024-09-29 110900](https://github.com/user-attachments/assets/0d7d83dc-9f81-483e-8810-ccffadcc4051)

![Screenshot 2024-09-29 110052](https://github.com/user-attachments/assets/c0c15c07-5914-4320-9d5e-c6d3fdfeb168)

![Screenshot 2024-09-29 105222](https://github.com/user-attachments/assets/def3b584-274e-4a65-a5d4-0e2fb9953de5)


## Future Enhancements
- Expanding the recipe database with more diverse cuisine types.
- Enhancing user profile features to include meal planning and grocery list generation.
