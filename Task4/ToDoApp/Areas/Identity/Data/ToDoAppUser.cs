using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ToDoAppUser class
public class ToDoAppUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(30)")]
    public required string FirstName { get; set; }
    [PersonalData]
    [Column(TypeName = "nvarchar(30)")]
    public required string LastName { get; set; }
}

