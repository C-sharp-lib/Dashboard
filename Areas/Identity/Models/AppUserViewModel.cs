using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Dash.Areas.Identity.Models;

public class AppUserViewModel : IdentityUser
{
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Middle name is required")]
    public string MiddleName { get; set; }
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Image is required")]
    public IFormFile ImageUrl { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [Required(ErrorMessage = "Date of birth is required")]
    public DateTime DateOfBirth { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [Required(ErrorMessage = "Date of hire is required")]
    public DateTime DateOfHire { get; set; }
    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }
    [Required(ErrorMessage = "City is required")]
    public string City { get; set; }
    [Required(ErrorMessage = "State is required")]
    public string State { get; set; }
    [Required(ErrorMessage = "Zipcode is required")]
    public string ZipCode { get; set; }
}