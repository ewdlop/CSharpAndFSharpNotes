namespace BlazorServerApp.Products
{
    public class AddProductToBasketEventArgs : EventArgs
    {
        public Product Product { get; set; }
    }
}