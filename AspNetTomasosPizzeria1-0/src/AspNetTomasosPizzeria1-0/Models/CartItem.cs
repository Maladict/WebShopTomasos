using System.Threading;


namespace AspNetTomasosPizzeria1_0.Models
{
    //TODO: Find out why counter increases oddly
    public class CartItem
    {
        private static int _counter;
        public int CartItemId { get; set; }
        public Matratt Matratt { get; set; }
        public int Quantity { get; set; }

        //Make sure each cart item has unique ID
        public CartItem()
        {
            this.CartItemId = Interlocked.Increment(ref _counter);
        }
    }
}
