using MediatR;

namespace BlazorServerApp.Products
{
    public class AddProductToCartCommand : IRequest<AddProductToCartResponse>
    {
        public Product Product { get; set; }
    }
}