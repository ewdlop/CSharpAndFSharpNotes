namespace CSharpClassLibrary.CSharp9.CovariantReturn
{
    public abstract class Product
    {
        protected string Name { get; }
        protected int Id { get; }
        protected Product(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public abstract IProductOrder Order(int quantity);
    }

    public class Book : Product
    {
        public string ISBN { get; }

        public Book(string name, int categoryId, string Isbn) : base(name, categoryId)
        {
            ISBN = Isbn;
        }

        public override BookOrder Order(int quantity) => new BookOrder { Quantity = quantity, Product = this };
    }

    public class Music : Product
    {
        protected Format Format { get; }

        public Music(string name, int categoryId, Format format) : base(name, categoryId)
        {
            Format = format;
        }

        public override MusicOrder Order(int quantity) => new MusicOrder { Quantity = quantity, Product = this };
    }

    public interface IProductOrder
    {
        public int Quantity { get; set; }
    }

    public class BookOrder : IProductOrder
    {
        public Book Product { get; set; }
        public int Quantity { get; set; }
    }

    public class MusicOrder : IProductOrder
    {
        public Music Product { get; set; }
        public int Quantity { get; set; }
    }

    public enum Format
    {
        Mp3,
        Disc
    }
}
