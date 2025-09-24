using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace EmbrujoCerveza.Web.Pages.BeerStyles
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EditModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public BeerStyle? BeerStyle { get; set; }

        [BindProperty]
        public IFormFile? ImageUpload { get; set; }

        [BindProperty]
        public bool RemoveImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BeerStyle = await _context.BeerStyles.FindAsync(id);

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

            var styleToUpdate = await _context.BeerStyles.FirstOrDefaultAsync(s => s.Id == id);
            if (styleToUpdate == null)
            {
                return NotFound();
            }

            BeerStyle = styleToUpdate;

            if (!await TryUpdateModelAsync(styleToUpdate, nameof(BeerStyle), s => s.Name, s => s.Description, s => s.Abv, s => s.Ibu, s => s.Stock))
            {
                return Page();
            }

            if (RemoveImage && !string.IsNullOrEmpty(styleToUpdate.ImageFileName))
            {
                DeleteImage(styleToUpdate.ImageFileName);
                styleToUpdate.ImageFileName = null;
            }

            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                if (!IsSupportedImage(ImageUpload))
                {
                    ModelState.AddModelError(nameof(ImageUpload), "Formato de imagen no soportado. Usa JPG, PNG o WEBP.");
                    return Page();
                }

                if (!string.IsNullOrEmpty(styleToUpdate.ImageFileName))
                {
                    DeleteImage(styleToUpdate.ImageFileName);
                }

                styleToUpdate.ImageFileName = await SaveImageAsync(ImageUpload);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Se actualizó el estilo {styleToUpdate.Name}.";
            return RedirectToPage("Index");
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsPath);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadsPath, fileName);

            using var stream = System.IO.File.Create(fullPath);
            await file.CopyToAsync(stream);

            return fileName;
        }

        private void DeleteImage(string fileName)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, "uploads", fileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        private static bool IsSupportedImage(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return extension is ".jpg" or ".jpeg" or ".png" or ".webp";
        }
    }
}
