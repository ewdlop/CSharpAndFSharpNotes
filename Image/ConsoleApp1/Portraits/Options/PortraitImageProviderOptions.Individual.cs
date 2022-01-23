namespace DM2BD.Europa.DAL.Generators.Portraits.Options;

public partial class PortraitImageProviderOptions
{
    public const string INDIVIDUAL = "Individual";
    public class Gender
    {
        public const string MALE = "male";
        public const string FEMALE = "female";
        public const string DEFAULT = "";
    }
    public class IndividualImages
    {
        public string[] Females { get; set; }
        public string[] Males { get; set; }
    }
    /// <summary>
    /// The names of the portrait images to read in with the file extension.
    /// </summary>
    public IndividualImages IndividualImageNames { get; set; }
}
