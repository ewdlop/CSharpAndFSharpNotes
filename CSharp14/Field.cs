namespace CSharp14
{
    public class Field(string? @field)
    {
        public string? Name
        {
            get
            {
                if (field is null) return string.Empty;
                return field;
            }
            set
            {
                field = @field?.ToString();
            }
        }


    }
}