namespace Dash.Areas.Admin.Models;

public class UpdateCustomerViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }
    public DateTime UpdatedAt { get; set; }

    public UpdateCustomerViewModel()
    {
        UpdatedAt = DateTime.Now;
    }
}