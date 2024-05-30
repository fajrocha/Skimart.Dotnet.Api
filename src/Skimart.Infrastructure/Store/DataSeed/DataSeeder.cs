using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Skimart.Domain.Entities.Order;
using Skimart.Domain.Entities.Products;

namespace Skimart.Infrastructure.Store.DataSeed;

public class DataSeeder
{
    private const string PathToFiles = "../Skimart.Infrastructure/Store/DataSeed/Data";

    public static async Task SeedAsync(StoreContext context, ILogger logger)
    {
        try
        {
            await GetSeedDataAndSave<ProductBrand>(context, "brands.json", logger);
            await GetSeedDataAndSave<ProductType>(context, "types.json", logger);
            await GetSeedDataAndSave<Product>(context, "products.json", logger);
            await GetSeedDataAndSave<DeliveryMethod>(context, "delivery.json", logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading the data.");
        }
    }

    private static async Task GetSeedDataAndSave<T>(
        DbContext context,
        string jsonFilename,
        ILogger logger) where T : class
    {
        if (!context.Set<T>().Any())
        {
            var dataAsString = await File.ReadAllTextAsync($"{PathToFiles}/{jsonFilename}");
            var dataAsObject = JsonSerializer.Deserialize<List<T>>(dataAsString);

            if (dataAsObject is null)
            {
                logger.LogWarning("No data found for {dataType}.", Path.ChangeExtension(jsonFilename, null));
                return;
            }

            foreach (var item in dataAsObject)
            {
                context.Set<T>().Add(item);
            }

            logger.LogInformation("Saving data of type {dataType}.", Path.ChangeExtension(jsonFilename, null));
            
            await context.SaveChangesAsync();
        }
    }
}