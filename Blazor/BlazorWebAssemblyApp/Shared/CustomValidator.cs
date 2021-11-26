using System.ComponentModel.DataAnnotations;
public class CustomValidator : ValidationAttribute
{
    protected override ValidationResult IsValid(object value,
        ValidationContext validationContext)
    {

        return new ValidationResult("Validation message to user.",
            new[] { validationContext.MemberName });
    }
}