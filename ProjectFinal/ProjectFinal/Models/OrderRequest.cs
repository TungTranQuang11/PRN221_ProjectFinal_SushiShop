namespace ProjectFinal.Models
{
    public class OrderRequest
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal TotalAmount { get; set; }
    }
}
