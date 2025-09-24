using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmbrujoCerveza.Web.Pages.BeerLots
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BeerLot? BeerLot { get; set; }

        public SelectList BeerStylesSelectList { get; private set; } = default!;
        public SelectList BottleTypesSelectList { get; private set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BeerLot = await _context.BeerLots
                .Include(lot => lot.BeerStyle)
                .Include(lot => lot.BottleType)
                .FirstOrDefaultAsync(lot => lot.Id == id);

            if (BeerLot == null)
            {
                return NotFound();
            }

            await LoadSelectorsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lotToUpdate = await _context.BeerLots.FirstOrDefaultAsync(lot => lot.Id == id);
            if (lotToUpdate == null)
            {
                return NotFound();
            }

            BeerLot = lotToUpdate;

            if (!await TryUpdateModelAsync(lotToUpdate, nameof(BeerLot), lot => lot.BeerStyleId, lot => lot.BottleTypeId, lot => lot.BottleCount, lot => lot.BottledOn, lot => lot.Notes))
            {
                await LoadSelectorsAsync();
                return Page();
            }

            if (!await _context.BeerStyles.AnyAsync(style => style.Id == lotToUpdate.BeerStyleId))
            {
                ModelState.AddModelError($"{nameof(BeerLot)}.{nameof(BeerLot.BeerStyleId)}", "Selecciona un estilo válido.");
                await LoadSelectorsAsync();
                return Page();
            }

            if (!await _context.BottleTypes.AnyAsync(type => type.Id == lotToUpdate.BottleTypeId))
            {
                ModelState.AddModelError($"{nameof(BeerLot)}.{nameof(BeerLot.BottleTypeId)}", "Selecciona un tipo de botella válido.");
                await LoadSelectorsAsync();
                return Page();
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Se actualizó el lote #{lotToUpdate.Id}.";
            return RedirectToPage("Index", new { styleId = lotToUpdate.BeerStyleId });
        }

        private async Task LoadSelectorsAsync()
        {
            var styles = await _context.BeerStyles
                .OrderBy(style => style.Name)
                .Select(style => new { style.Id, style.Name })
                .ToListAsync();
            BeerStylesSelectList = new SelectList(styles, "Id", "Name", BeerLot?.BeerStyleId);

            var bottleTypes = await _context.BottleTypes
                .OrderBy(type => type.CapacityMl)
                .ThenBy(type => type.Material)
                .Select(type => new
                {
                    type.Id,
                    Display = type.CapacityMl + " ml - " + type.Material
                })
                .ToListAsync();
            BottleTypesSelectList = new SelectList(bottleTypes, "Id", "Display", BeerLot?.BottleTypeId);
        }
    }
}
