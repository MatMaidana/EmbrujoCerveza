using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmbrujoCerveza.Web.Pages.BeerStyles
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<BeerStyle> BeerStyle { get; private set; } = new List<BeerStyle>();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.BeerStyles
                .Include(style => style.Lots)
                    .ThenInclude(lot => lot.BottleType)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var searchTerm = $"%{Search.Trim()}%";
                query = query.Where(style => EF.Functions.Like(style.Name, searchTerm) || (style.Description != null && EF.Functions.Like(style.Description, searchTerm)));
            }

            BeerStyle = await query
                .OrderBy(style => style.Name)
                .ToListAsync();
        }
    }
}
