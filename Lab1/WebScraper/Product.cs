namespace WebScraper
{
    public class Product
    {
        public Product(string name, string price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; set; }
        public string Price { get; set; }
        public string RAM { get; set; }
    }
}