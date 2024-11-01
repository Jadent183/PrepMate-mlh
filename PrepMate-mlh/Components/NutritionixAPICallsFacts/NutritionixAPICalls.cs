using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class NutritionixAPICalls
{
    public double? Calories { get; set; }
    public double? TotalFat { get; set; }
    public double? SaturatedFat { get; set; }
    public double? Cholesterol { get; set; }
    public double? Sodium { get; set; }
    public double? TotalCarbohydrate { get; set; }
    public double? DietaryFiber { get; set; }
    public double? Sugars { get; set; }
    public double? Protein { get; set; }
    public double? Potassium { get; set; }

    public static NutritionixAPICalls GetNutritionFacts(string jsonResponse, string weightInGrams)
    {
        var json = JObject.Parse(jsonResponse);
        var food = json["foods"][0];

        int inputWeight = int.Parse(weightInGrams);
        double defaultWeight = (double)food["serving_weight_grams"];
        double factor = inputWeight / defaultWeight;

        return new NutritionixAPICalls
        {
            Calories = food["nf_calories"].Type != JTokenType.Null ? TruncateToOneDecimal(factor * (double)food["nf_calories"]) : 0,
            TotalFat = food["nf_total_fat"].Type != JTokenType.Null ? TruncateToOneDecimal(factor * (double)food["nf_total_fat"]) : 0,
            SaturatedFat = food["nf_saturated_fat"].Type != JTokenType.Null ? TruncateToOneDecimal(factor * (double)food["nf_saturated_fat"]) : 0,
            Cholesterol = food["nf_cholesterol"].Type != JTokenType.Null ? TruncateToOneDecimal(factor * (double)food["nf_cholesterol"]) : 0,
            Sodium = food["nf_sodium"].Type != JTokenType.Null ? TruncateToOneDecimal(factor * (double)food["nf_sodium"]) : 0,
            TotalCarbohydrate = food["nf_total_carbohydrate"].Type != JTokenType.Null ? TruncateToOneDecimal(factor * (double)food["nf_total_carbohydrate"]) : 0,
            DietaryFiber = food["nf_dietary_fiber"].Type != JTokenType.Null ? TruncateToOneDecimal(factor * (double)food["nf_dietary_fiber"]) : 0,
            Sugars = food["nf_sugars"].Type != JTokenType.Null ? TruncateToOneDecimal(factor * (double)food["nf_sugars"]) : 0,
            Protein = food["nf_protein"].Type != JTokenType.Null ? TruncateToOneDecimal(factor * (double)food["nf_protein"]) : 0,
            Potassium = food["nf_potassium"].Type != JTokenType.Null ? TruncateToOneDecimal(factor * (double)food["nf_potassium"]) : 0
        };
    }

    private static double TruncateToOneDecimal(double value)
    {
        return Math.Truncate(value * 10) / 10;
    }

}



public class NutritionixApi
{
    private const string AppId = "9885650a";
    private const string AppKey = "c9580be26fee88d7792b5b4bc76458e4";

    public static async Task<NutritionixSearchResult> SearchFoodAsync(string query, IHttpClientFactory clientFactory, CancellationToken cancellationToken)
    {
        using (var client = clientFactory.CreateClient("NutritionixApi"))
        {
            string url = $"https://trackapi.nutritionix.com/v2/search/instant?query={Uri.EscapeDataString(query)}";
            client.DefaultRequestHeaders.Add("x-app-id", AppId);
            client.DefaultRequestHeaders.Add("x-app-key", AppKey);

            HttpResponseMessage response = await client.GetAsync(url, cancellationToken);
            string jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<NutritionixSearchResult>(jsonResponse);
        }
    }

    public static async Task<NutritionixAPICalls> GetNutritionFactsAsync(string item, string weight)
    {
        if (string.IsNullOrEmpty(item) || string.IsNullOrEmpty(weight)) return null;
        weight = weight.Replace(" g", "").Trim();
        using (HttpClient client = new HttpClient())
        {
            string url = "https://trackapi.nutritionix.com/v2/natural/nutrients";
            client.DefaultRequestHeaders.Add("x-app-id", AppId);
            client.DefaultRequestHeaders.Add("x-app-key", AppKey);
            var jsonBody = new { query = item };
            var json = JsonConvert.SerializeObject(jsonBody);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return NutritionixAPICalls.GetNutritionFacts(jsonResponse, weight);
        }
    }



    public class NutritionixSearchResult
    {
        [JsonPropertyName("common")]
        public List<CommonFood> Common { get; set; }

        [JsonPropertyName("branded")]
        public List<BrandedFood> Branded { get; set; }
    }

    public class CommonFood
    {
        [JsonProperty("food_name")]
        public string FoodName { get; set; }

        [JsonProperty("serving_unit")]
        public string ServingUnit { get; set; }

        [JsonProperty("tag_name")]
        public string TagName { get; set; }

        [JsonProperty("serving_qty")]
        public double ServingQty { get; set; }

        [JsonProperty("common_type")]
        public object CommonType { get; set; }

        [JsonProperty("tag_id")]
        public string TagId { get; set; }

        [JsonProperty("photo")]
        public Photo Photo { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }
    }

    public class BrandedFood
    {
        [JsonProperty("food_name")]
        public string FoodName { get; set; }

        [JsonProperty("serving_unit")]
        public string ServingUnit { get; set; }

        [JsonProperty("nix_brand_id")]
        public string NixBrandId { get; set; }

        [JsonProperty("brand_name_item_name")]
        public string BrandNameItemName { get; set; }

        [JsonProperty("serving_qty")]
        public double ServingQty { get; set; }

        [JsonProperty("nf_calories")]
        public double? NfCalories { get; set; }

        [JsonProperty("photo")]
        public Photo Photo { get; set; }

        [JsonProperty("brand_name")]
        public string BrandName { get; set; }

        [JsonProperty("region")]
        public int Region { get; set; }

        [JsonProperty("brand_type")]
        public int BrandType { get; set; }

        [JsonProperty("nix_item_id")]
        public string NixItemId { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }
    }

    public class Photo
    {
        [JsonProperty("thumb")]
        public string Thumb { get; set; }
    }

}




