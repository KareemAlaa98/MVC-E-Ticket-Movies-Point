using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies_Point.Data;

namespace Movies_Point.Controllers
{
    public class SearchController : Controller
    {
        ApplicationDbContext context;
        public SearchController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Index(string search)
        {
            var searchResults = context.Movies.Include(e => e.Category).Include("Cinema").Where(e => e.Name.Contains(search)).ToList();
            return View(searchResults);
        }
    }
}
