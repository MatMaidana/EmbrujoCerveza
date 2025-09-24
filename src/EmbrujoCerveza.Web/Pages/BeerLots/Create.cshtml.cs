using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmbrujoCerveza.Web.Pages.BeerLots
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BeerLot BeerLot { get; set; } = new();

        public SelectList BeerStylesSelectList { get; private set; } = default!;
        public SelectList BottleTypesSelectList { get; private set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? styleId, int? bottleTypeId)
        {
            if (styleId.HasValue)
            {
                BeerLot.BeerStyleId = styleId.Value;
            }

            if (bottleTypeId.HasValue)
            {
                BeerLot.BottleTypeId = bottleTypeId.Value;
            }

            await LoadSelectorsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectorsAsync();
                return Page();
            }

            if (!await _context.BeerStyles.AnyAsync(style => style.Id == BeerLot.BeerStyleId))
            {
                ModelState.AddModelError($"{nameof(BeerLot)}.{nameof(BeerLot.BeerStyleId)}", "Selecciona un estilo válido.");
                await LoadSelectorsAsync();
                return Page();
            }

            if (!await _context.BottleTypes.AnyAsync(type => type.Id == BeerLot.BottleTypeId))
            {
                ModelState.AddModelError($"{nameof(BeerLot)}.{nameof(BeerLot.BottleTypeId)}", "Selecciona un tipo de botella válido.");
                await LoadSelectorsAsync();
                return Page();
            }

            _context.BeerLots.Add(BeerLot);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Se registró el lote #{BeerLot.Id}.";
            return RedirectToPage("Index", new { styleId = BeerLot.BeerStyleId });
        }

        private async Task LoadSelectorsAsync()
        {
            var styles = await _context.BeerStyles
                .OrderBy(style => style.Name)
                .Select(style => new { style.Id, style.Name })
                .ToListAsync();
            BeerStylesSelectList = new SelectList(styles, "Id", "Name", BeerLot.BeerStyleId == 0 ? null : BeerLot.BeerStyleId);

            var bottleTypes = await _context.BottleTypes
                .OrderBy(type => type.CapacityMl)
                .ThenBy(type => type.Material)
                .Select(type => new
                {
                    type.Id,
                    Display = type.CapacityMl + " ml - " + type.Material
                })
                .ToListAsync();
            BottleTypesSelectList = new SelectList(bottleTypes, "Id", "Display", BeerLot.BottleTypeId == 0 ? null : BeerLot.BottleTypeId);
        }
    }
}
