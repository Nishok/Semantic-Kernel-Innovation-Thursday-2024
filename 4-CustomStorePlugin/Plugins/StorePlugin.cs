using CustomStorePlugin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace CustomStorePlugin.Plugins
{
    public class StorePlugin
    {
        private readonly ApplicationDbContext _context;

        public StorePlugin(ApplicationDbContext context)
        {
            _context = context;
        }

        [KernelFunction]
        [Description("A List of all the items that can be purchased")]
        [return: Description("A list of purchasable items")]
        public async Task<List<Item>> ListPurchasableItemsAsync()
        {
            return await _context.Items.ToListAsync();
        }

        [KernelFunction]
        [Description("Purchase an item.")]
        [return: Description("The purchased item")]
        public async Task<PurchasedItem> PurchaseItemAsync(int itemId, int quantity)
        {
            var item = await _context.Items.FindAsync(itemId);

            if (item == null)
            {
                return null;
            }

            var purchasedItem = new PurchasedItem
            {                
                ItemId = item.Id,
                ItemName = item.Name,
                Quantity = quantity,
                PurchasedDate = DateTime.UtcNow
            };

            _context.PurchasedItems.Add(purchasedItem);
            await _context.SaveChangesAsync();

            return purchasedItem;
        }

        [KernelFunction]
        [Description("Get all purchased items.")]
        [return: Description("A list of purchased items")]
        public async Task<List<PurchasedItem>> GetPurchasedItemsAsync()
        {
            return await _context.PurchasedItems.Include(pi => pi.Item).ToListAsync();
        }
    }
}
