using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmbrujoCerveza.Web.Pages.BottleTypes
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<BottleType> BottleTypes { get; private set; } = new List<BottleType>();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.BottleTypes
                .Include(type => type.Lots)
                    .ThenInclude(lot => lot.BeerStyle)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var term = $"%{Search.Trim()}%";
                query = query.Where(type =>
                    EF.Functions.Like(type.Material, term) ||
                    (type.Description != null && EF.Functions.Like(type.Description, term)));
            }

            BottleTypes = await query
                .OrderBy(type => type.CapacityMl)
                .ThenBy(type => type.Material)
                .ToListAsync();
        }
    }
}
