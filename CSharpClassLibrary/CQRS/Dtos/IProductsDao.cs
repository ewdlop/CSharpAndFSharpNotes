using System.Collections.Generic;

namespace CSharpClassLibrary.CQRS.Dtos
{
    public interface IProductsDao //Data Access Object
    {
        ProductDisplayDto FindById(int productId);
        ICollection<ProductDisplayDto> FindByName(string name);
        ICollection<ProductInventoryDto> FindOutOfStockProducts();
        ICollection<ProductDisplayDto> FindRelatedProducts(int productId);
    }
}
