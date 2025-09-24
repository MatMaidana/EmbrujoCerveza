using System;
using System.Linq;
using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmbrujoCerveza.Web.Pages.BottleTypes
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public BottleType? BottleType { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BottleType = await _context.BottleTypes
                .Include(type => type.Lots)
                    .ThenInclude(lot => lot.BeerStyle)
                .AsNoTracking()
                .FirstOrDefaultAsync(type => type.Id == id);

            if (BottleType == null)
            {
                return NotFound();
            }

            BottleType.Lots = BottleType.Lots
                .OrderByDescending(lot => lot.BottledOn ?? DateTime.MinValue)
                .ThenByDescending(lot => lot.Id)
                .ToList();

            return Page();
        }
    }
}
