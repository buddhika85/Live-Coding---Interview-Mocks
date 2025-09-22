namespace LiveCoding_Console.Mock_1;

internal class Mock1
{

    private List<Product> products = new List<Product>
    {
        new Product { ProductId = 1, Name = "Laptop", Tags = new List<string> { "electronics", "portable", "work" } },
        new Product { ProductId = 2, Name = "Tablet", Tags = new List<string> { "electronics", "portable", "touch" } },
        new Product { ProductId = 3, Name = "Monitor", Tags = new List<string> { "electronics", "display", "work" } },
        new Product { ProductId = 4, Name = "Keyboard", Tags = new List<string> { "accessory", "work", "input" } },
        new Product { ProductId = 5, Name = "Smartphone", Tags = new List<string> { "electronics", "portable", "touch" } }
    };

    //Write a method that, given a target ProductId, returns the top 3 most similar products based on tag overlap.
    //Similarity is defined by the number of shared tags.If multiple products have the same score, sort by ProductId ascending.
    // 4:25 - 4:36
    public Product[] SimilarTopThree(int productId)
    {

        var targettedTags = products.Where(x => x.ProductId == productId)?.FirstOrDefault()?.Tags.ToArray() ?? [];

        if (!targettedTags.Any())
            return [];
        var simillarProductsQuery = (from prod in products

                                     let tagIntersectCount = targettedTags.Intersect(prod.Tags).Count()
                                     let similarityPercentage = tagIntersectCount == 0 ? 0.0 : (tagIntersectCount / targettedTags.Length) * 100

                                     orderby similarityPercentage descending
                                     where prod.ProductId != productId
                                     select prod);
        return simillarProductsQuery.Take(3).ToArray();
    }

}


class Product
{
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public List<string> Tags { get; set; }
}
