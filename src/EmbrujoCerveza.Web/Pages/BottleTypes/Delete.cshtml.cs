using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmbrujoCerveza.Web.Pages.BottleTypes
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BottleType? BottleType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BottleType = await _context.BottleTypes
                .Include(type => type.Lots)
                .AsNoTracking()
                .FirstOrDefaultAsync(type => type.Id == id);

            if (BottleType == null)
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

            var bottleType = await _context.BottleTypes.FindAsync(id);
            if (bottleType == null)
            {
                return NotFound();
            }

            var displayName = bottleType.DisplayName;

            _context.BottleTypes.Remove(bottleType);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Se eliminó el tipo de botella {displayName}.";
            return RedirectToPage("Index");
        }
    }
}
