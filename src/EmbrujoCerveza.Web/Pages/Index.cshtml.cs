using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmbrujoCerveza.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<BeerStyle> BeerStyles { get; private set; } = new List<BeerStyle>();

        public int TotalStock => BeerStyles.Sum(style => style.Stock);

        public async Task OnGetAsync()
        {
            BeerStyles = await _context.BeerStyles
                .AsNoTracking()
                .OrderBy(style => style.Name)
                .ToListAsync();
        }
    }
}
