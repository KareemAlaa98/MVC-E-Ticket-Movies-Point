using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Movies_Point.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Age { get; set; }
    }
}
