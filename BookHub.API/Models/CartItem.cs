namespace BookHub.API.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Customer User { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

    }
}
