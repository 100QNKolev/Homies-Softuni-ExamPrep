using Homies.Data;
using Homies.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homies.Controllers
{
    public class EventController : Controller
    {
        private readonly HomiesDbContext _context;

        public EventController(HomiesDbContext context)
        {
                _context = context;
        }

        public async Task<IActionResult> All()
        {
            var events = _context.Events.Select(e => new EventInfoViewModel(
                e.Id, e.Name, e.Start, e.Type.Name, e.Organiser.UserName
                ));

            return View(events);
        }
    }
}
