using Dash.Areas.Identity.Models;

namespace Dash.Areas.Admin.Models;

public class PaginateUsers
{
    public IEnumerable<AppUser> Users { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    
}

public class PaginateProducts
{
    public IEnumerable<Products> Products { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class PaginateEvents
{
    public IEnumerable<Event> Events { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}