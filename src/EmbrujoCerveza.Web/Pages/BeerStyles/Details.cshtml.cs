using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmbrujoCerveza.Web.Pages.BeerStyles
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public BeerStyle? BeerStyle { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BeerStyle = await _context.BeerStyles
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (BeerStyle == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
