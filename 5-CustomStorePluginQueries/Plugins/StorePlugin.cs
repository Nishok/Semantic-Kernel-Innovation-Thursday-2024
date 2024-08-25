using CustomStorePluginQueries.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text.Json;
using Dapper;

namespace CustomStorePluginQueries.Plugins
{
    public class StorePlugin
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public StorePlugin(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //I don't have to tell you how bad it is to allow raw SQL queries to be ran directly into the databse by the client, right?
        //Not to mention having the LLM generate it and then run it directly.
        //This is for demo purpose!
        [KernelFunction]
        [Description("Execute a query on the SQL Server database and it will return a string result")]
        public async Task<string> ExecuteSqlQueryAsync(string query)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var result = await connection.QueryAsync(query);
            return JsonSerializer.Serialize(result);
        }

        //All the other functions are commented to show they aren't being used.
        //[KernelFunction]
        //[Description("A List of all the items that can be purchased.")]
        //[return: Description("A list of purchasable items.")]
        //public async Task<List<Item>> ListPurchasableItemsAsync()
        //{
        //    return await _context.Items.ToListAsync();
        //}

        //[KernelFunction]
        //[Description("Purchase an item.")]
        //[return: Description("The purchased item.")]
        //public async Task<PurchasedItem> PurchaseItemAsync(int itemId, int quantity)
        //{
        //    var item = await _context.Items.FindAsync(itemId);

        //    if (item == null)
        //    {
        //        return null;
        //    }

        //    var purchasedItem = new PurchasedItem
        //    {
        //        ItemId = item.Id,
        //        ItemName = item.Name,
        //        Quantity = quantity,
        //        PurchasedDate = DateTime.UtcNow
        //    };

        //    _context.PurchasedItems.Add(purchasedItem);
        //    await _context.SaveChangesAsync();

        //    return purchasedItem;
        //}

        //[KernelFunction]
        //[Description("Get all purchased items.")]
        //[return: Description("A list of purchased items.")]
        //public async Task<List<PurchasedItem>> GetPurchasedItemsAsync()
        //{
        //    return await _context.PurchasedItems.Include(pi => pi.PurchasableItem).ToListAsync();
        //}
    }
}
