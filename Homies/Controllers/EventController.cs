using Homies.Data;
using Homies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Homies.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly HomiesDbContext _context;

        public EventController(HomiesDbContext context)
        {
                _context = context;
        }

        public async Task<IActionResult> All()
        {
            var events = await _context.Events
                .AsNoTracking()
                .Select(
                e => new EventInfoViewModel
                (
                e.Id, e.Name, e.Start, e.Type.Name, e.Organiser.UserName
                ))
                .ToListAsync();

            return View(events);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id) 
        {
            var ev = await _context.Events
                .Where(e => e.Id == id)
                .Include(e => e.EventsParticipants)
                .FirstOrDefaultAsync();

            if (ev == null) 
            {
                return BadRequest("Error");
            }

            string userId = GetUserId();

            if (!ev.EventsParticipants.Any(p => p.HelperId == userId)) 
            {
                ev.EventsParticipants.Add(new EventParticipant()
                {
                    EventId = ev.Id,
                    HelperId = userId
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Joined));
        }

        [HttpGet]
        public async Task<IActionResult> Joined() 
        {
            string userId = GetUserId();

            var model = await _context.EventParticipants
                .Where(ep => ep.HelperId == userId)
                .Select(ep => new EventInfoViewModel(
                    ep.EventId,
                    ep.Event.Name,
                    ep.Event.Start,
                    ep.Event.Type.Name,
                    ep.Event.Organiser.UserName
                 ))
                .AsNoTracking()
                .ToListAsync();
                

            return View(model);
        }

        public async Task<IActionResult> Leave(int id) 
        {
            string userId = GetUserId();

            var ev = await _context.Events
              .Where(e => e.Id == id)
              .Include(e => e.EventsParticipants)
              .FirstOrDefaultAsync();

            if (ev == null)
            {
                return BadRequest("Error");
            }

            var ep = ev.EventsParticipants
                       .FirstOrDefault(ep => ep.HelperId == userId);

            if (ep == null) 
            {
                return BadRequest();
            }

            ev.EventsParticipants.Remove(ep);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        private string GetUserId() 
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
    }
}
