using System.ComponentModel.DataAnnotations;

namespace BlazorWebAssemblyApp.Shared.Models;

public class Person
{
    [Required]
    [StringLength(10, ErrorMessage = "Name is too long.")]
    public string? Name { get; set; }

    [StringLength(10, ErrorMessage = "Description is too long.")]
    public string Description { get; set; }
}
