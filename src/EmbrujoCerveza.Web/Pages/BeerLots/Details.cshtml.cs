using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmbrujoCerveza.Web.Pages.BeerLots
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public BeerLot? BeerLot { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BeerLot = await _context.BeerLots
                .Include(lot => lot.BeerStyle)
                .Include(lot => lot.BottleType)
                .AsNoTracking()
                .FirstOrDefaultAsync(lot => lot.Id == id);

            if (BeerLot == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
