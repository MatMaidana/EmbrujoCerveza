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

        public int TotalBottles => BeerStyles.Sum(style => style.TotalLotBottles);

        public int TotalLots => BeerStyles.Sum(style => style.Lots.Count);

        public int BottleTypeCount { get; private set; }

        public async Task OnGetAsync()
        {
            BeerStyles = await _context.BeerStyles
                .Include(style => style.Lots)
                    .ThenInclude(lot => lot.BottleType)
                .AsNoTracking()
                .OrderBy(style => style.Name)
                .ToListAsync();

            BottleTypeCount = await _context.BottleTypes.CountAsync();
        }
    }
}
