using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace EmbrujoCerveza.Web.Pages.BeerStyles
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public BeerStyle BeerStyle { get; set; } = new();

        [BindProperty]
        public IFormFile? ImageUpload { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                if (!IsSupportedImage(ImageUpload))
                {
                    ModelState.AddModelError(nameof(ImageUpload), "Formato de imagen no soportado. Usa JPG, PNG o WEBP.");
                    return Page();
                }

                BeerStyle.ImageFileName = await SaveImageAsync(ImageUpload);
            }

            _context.BeerStyles.Add(BeerStyle);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Se agregó el estilo {BeerStyle.Name}.";
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

        private static bool IsSupportedImage(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return extension is ".jpg" or ".jpeg" or ".png" or ".webp";
        }
    }
}
