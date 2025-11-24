namespace BookHub.API.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Customer User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string PaymentMethod { get; set; } = "COD";
        public string Status { get; set; } = "Pending";

        public ICollection<OrderItem> OrderItems { get; set; }

    }
}
