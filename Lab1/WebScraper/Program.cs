using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using WebScraper;

HttpClient client = new HttpClient();
decimal exchangeRate = 19.5M; // Assuming 1 EUR = 18.5 MDL for conversion purposes

var responseString = await client.GetStringAsync(@$"https://darwin.md/telefoane/smartphone/apple-iphone/");

var htmlDoc = new HtmlDocument();
htmlDoc.LoadHtml(responseString);

var phoneNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'img-wrap w-100')]");

List<Product> listOfProducts = new();
foreach (var phone in phoneNodes)
{
    var start = phone.InnerHtml.IndexOf("title") + "title".Length + 2;
    var end = phone.InnerHtml.IndexOf($"class=\"ga-item\"", start) - 2;

    var titleValue = phone.InnerHtml.Substring(start, end - start).Replace("\\\"", "\"");
    Console.Write("Title: " + titleValue);

    start = phone.InnerHtml.IndexOf("price") + "price".Length;
    end = phone.InnerHtml.IndexOf($"quantity", start);

    var result = phone.InnerHtml.Substring(start, end - start).Replace("\\\"", "\"");

    //only get the digits
    var priceValue = string.Join(string.Empty, Regex.Matches(result, @"\d+").OfType<Match>().Select(m => m.Value));

    Console.Write(", Price (MDL): " + priceValue);

    var product = new Product(titleValue, priceValue);

    if (ValidateProductData(product))
        listOfProducts.Add(product);

    await ExtractSubProductData(phone.InnerHtml);
}

// Map: Convert MDL to EUR
var mappedProducts = listOfProducts.Select(product =>
{
    decimal priceInMDL = Convert.ToDecimal(product.Price, CultureInfo.InvariantCulture);
    decimal priceInEUR = priceInMDL / exchangeRate;
    product.Price = priceInEUR.ToString("F2", CultureInfo.InvariantCulture);
    return product;
}).ToList();

decimal minPrice = 1200;
decimal maxPrice = 1500;

var filteredProducts = mappedProducts.Where(product =>
{
    decimal price = Convert.ToDecimal(product.Price, CultureInfo.InvariantCulture);
    return price >= minPrice && price <= maxPrice;
}).ToList();

decimal totalSum = filteredProducts
    .Select(product => Convert.ToDecimal(product.Price, CultureInfo.InvariantCulture))
    .Aggregate((sum, next) => sum + next);

var resultData = new
{
    Products = filteredProducts,
    TotalPriceInEUR = totalSum.ToString("F2", CultureInfo.InvariantCulture),
    TimestampUTC = DateTime.UtcNow
};

Console.WriteLine("\nFiltered Products(1200-1500 EUR):");
foreach (var product in filteredProducts)
{
    Console.WriteLine($"Title: {product.Name}, Price (EUR): {product.Price}");
}

Console.WriteLine($"\nTotal Price of Filtered Products (EUR): {resultData.TotalPriceInEUR}");
Console.WriteLine($"Timestamp UTC: {resultData.TimestampUTC}");

bool ValidateProductData(Product product)
{
    if (!decimal.TryParse(product.Price, out _))
        return false;

    if (string.IsNullOrEmpty(product.Name))
        return false;

    return true;
}

async Task ExtractSubProductData(string text)
{
    var start = text.IndexOf("href") + "href".Length + 2;
    var end = text.IndexOf($"title", start) - 2;

    var link = text.Substring(start, end - start).Replace("\\\"", "\"");
    Console.WriteLine($", URL: {link}");

    HttpClient client = new HttpClient();
    var responseString = await client.GetStringAsync(link);

    string path = @"subData.txt";

    if (!File.Exists(path))
    {
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.WriteLine(responseString);
        }
    }

    var htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(responseString);

    var phoneNodes = htmlDoc.DocumentNode.SelectNodes("//ul[contains(@class, 'features')]");

    if (phoneNodes != null)
    {
        foreach (var ul in phoneNodes)
        {
            var ramNode = ul.SelectSingleNode(".//li[contains(text(), 'Memorie RAM')]");

            if (ramNode != null)
            {
                // Extracting the RAM value
                var ramText = ramNode.InnerText;
                var ramValue = ramText.Split(':')[1].Trim();
                Console.WriteLine($"Memorie RAM: {ramValue}");
                break;
            }
            else
            {
                Console.WriteLine("Memorie RAM not found.");
            }
        }
    }
}