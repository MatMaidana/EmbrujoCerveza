using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmbrujoCerveza.Web.Pages.BottleTypes
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BottleType BottleType { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BottleTypes.Add(BottleType);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Se agregó el tipo de botella {BottleType.DisplayName}.";
            return RedirectToPage("Index");
        }
    }
}
