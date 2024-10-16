// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using WebScraper;

HttpClient client = new HttpClient();

var responseString = await client.GetStringAsync(@$"https://darwin.md/telefoane/smartphone/apple-iphone");

var htmlDoc = new HtmlDocument();
htmlDoc.LoadHtml(responseString);

var phoneNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'img-wrap w-100')]");

List<Product> listOfProducts = new();
foreach (var phone in phoneNodes)
{
    var start = phone.InnerHtml.IndexOf("title") + "title".Length + 2;
    var end = phone.InnerHtml.IndexOf($"class=\"ga-item\"", start) - 2;

    var titleValue = phone.InnerHtml.Substring(start, end - start).Replace("\\\"", "\"");
    Console.WriteLine("Title: " + titleValue);

    start = phone.InnerHtml.IndexOf("price") + "price".Length;
    end = phone.InnerHtml.IndexOf($"quantity", start);

    var result = phone.InnerHtml.Substring(start, end - start).Replace("\\\"", "\"");

    //only get the digits
    var priceValue = string.Join(string.Empty, Regex.Matches(result, @"\d+").OfType<Match>().Select(m => m.Value));

    Console.WriteLine("Price: " + result);

    listOfProducts.Add(new Product(titleValue, result));
}

Console.WriteLine();