using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace EmbrujoCerveza.Web.Pages.BeerStyles
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DeleteModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public BeerStyle? BeerStyle { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BeerStyle = await _context.BeerStyles
                .Include(style => style.Lots)
                    .ThenInclude(lot => lot.BottleType)

                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (BeerStyle == null)
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

            var beerStyle = await _context.BeerStyles.FindAsync(id);
            if (beerStyle == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(beerStyle.ImageFileName))
            {
                DeleteImage(beerStyle.ImageFileName);
            }

            _context.BeerStyles.Remove(beerStyle);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Se eliminó el estilo {beerStyle.Name}.";
            return RedirectToPage("Index");
        }

        private void DeleteImage(string fileName)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, "uploads", fileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}
