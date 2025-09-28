using System;
using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmbrujoCerveza.Web.Pages.BeerLots
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<BeerLot> BeerLots { get; private set; } = new List<BeerLot>();

        public SelectList BeerStylesSelectList { get; private set; } = default!;
        public SelectList BottleTypesSelectList { get; private set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int? StyleId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? BottleTypeId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        public async Task OnGetAsync()
        {
            await LoadFiltersAsync();

            var query = _context.BeerLots
                .Include(lot => lot.BeerStyle)
                .Include(lot => lot.BottleType)
                .AsNoTracking();

            if (StyleId.HasValue)
            {
                query = query.Where(lot => lot.BeerStyleId == StyleId);
            }

            if (BottleTypeId.HasValue)
            {
                query = query.Where(lot => lot.BottleTypeId == BottleTypeId);
            }

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var trimmed = Search.Trim();
                var term = $"%{trimmed}%";
                var isNumeric = int.TryParse(trimmed, out var lotNumber);

                query = query.Where(lot =>
                    (lot.Notes != null && EF.Functions.Like(lot.Notes, term)) ||
                    (lot.BeerStyle != null && EF.Functions.Like(lot.BeerStyle.Name, term)) ||
                    (lot.BottleType != null && EF.Functions.Like(lot.BottleType.Material, term)) ||
                    (isNumeric && lot.Id == lotNumber));
            }

            BeerLots = await query
                .OrderByDescending(lot => lot.BottledOn.HasValue) // primero los que tienen fecha
                .ThenByDescending(lot => lot.BottledOn)           // luego por la fecha
                .ThenByDescending(lot => lot.Id)
                .ToListAsync();
        }

        private async Task LoadFiltersAsync()
        {
            var styles = await _context.BeerStyles
                .OrderBy(style => style.Name)
                .Select(style => new { style.Id, style.Name })
                .ToListAsync();
            BeerStylesSelectList = new SelectList(styles, "Id", "Name", StyleId);

            var bottleTypes = await _context.BottleTypes
                .OrderBy(type => type.CapacityMl)
                .ThenBy(type => type.Material)
                .Select(type => new
                {
                    type.Id,
                    Display = type.CapacityMl + " ml - " + type.Material
                })
                .ToListAsync();
            BottleTypesSelectList = new SelectList(bottleTypes, "Id", "Display", BottleTypeId);
        }
    }
}
