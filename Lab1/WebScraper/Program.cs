// See https://aka.ms/new-console-template for more information
using System.Net;

HttpClient client = new HttpClient()
{
};

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
var responseString = await client.GetStringAsync(@$"https://darwin.md/telefoane/smartphone/apple-iphone");

string path = @"data.txt";
// This text is added only once to the file.
if (!File.Exists(path))
{
    // Create a file to write to.
    using (StreamWriter sw = File.CreateText(path))
    {
        sw.WriteLine(responseString);
    }
}