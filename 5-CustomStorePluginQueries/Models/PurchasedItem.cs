namespace CustomStorePluginQueries.Models
{
    public class PurchasedItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchasedDate { get; set; }
        public int ItemId { get; set; }

        public Item Item { get; set; }
    }
}
