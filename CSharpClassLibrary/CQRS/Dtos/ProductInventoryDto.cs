namespace CSharpClassLibrary.CQRS.Dtos
{
    public class ProductInventoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrentStock { get; set; }
    }
}
