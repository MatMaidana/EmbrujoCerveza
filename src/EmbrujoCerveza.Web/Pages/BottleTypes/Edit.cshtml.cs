using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmbrujoCerveza.Web.Pages.BottleTypes
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
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

            BottleType = await _context.BottleTypes.FindAsync(id);

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

            var typeToUpdate = await _context.BottleTypes.FirstOrDefaultAsync(type => type.Id == id);
            if (typeToUpdate == null)
            {
                return NotFound();
            }

            BottleType = typeToUpdate;

            if (!await TryUpdateModelAsync(typeToUpdate, nameof(BottleType), type => type.Material, type => type.CapacityMl, type => type.Description))
            {
                return Page();
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Se actualizó el tipo de botella {typeToUpdate.DisplayName}.";
            return RedirectToPage("Index");
        }
    }
}
