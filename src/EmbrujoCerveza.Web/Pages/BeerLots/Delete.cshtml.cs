using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmbrujoCerveza.Web.Pages.BeerLots
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BeerLot? BeerLot { get; set; }

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beerLot = await _context.BeerLots
                .Include(lot => lot.BeerStyle)
                .FirstOrDefaultAsync(lot => lot.Id == id);

            if (beerLot == null)
            {
                return NotFound();
            }

            var styleId = beerLot.BeerStyleId;

            _context.BeerLots.Remove(beerLot);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Se eliminó el lote #{beerLot.Id}.";
            return RedirectToPage("Index", new { styleId });
        }
    }
}
