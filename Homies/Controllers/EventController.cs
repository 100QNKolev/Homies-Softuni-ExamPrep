using Homies.Data;
using Homies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Security.Claims;
using static Homies.Data.DataConstants;

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

        [HttpGet]
        public async Task<IActionResult> Add() 
        {
            var model = await _context.Events.Select(e => new EventFormViewModel()).FirstOrDefaultAsync();

            model.Types = await GetTypes();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventFormViewModel model)
        {
            DateTime start;
            DateTime end;

            if (!DateTime.TryParseExact(
                model.End,
                EventDateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out start))
            {
                ModelState.AddModelError(nameof(model.Start), DateTimeError);
            }

            if (!DateTime.TryParseExact(
                model.End,
                EventDateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out end)) 
            {
                ModelState.AddModelError(nameof(model.End), DateTimeError);
            }

            if (!ModelState.IsValid) 
            {
                model.Types = await GetTypes();

                return View(model);
            }

            var entity = new Event 
            {
                Name = model.Name,
                Description = model.Description,
                Start = start,
                End = end,
                OrganiserId = GetUserId(),
                CreatedOn = DateTime.Now,
                TypeId = model.TypeId
            };

            await _context.Events.AddAsync(entity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string userId = GetUserId();

            var e = await _context.Events
                .Where(e => e.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (e == null) 
            {
                return BadRequest();
            }
            if (e.OrganiserId != userId) 
            {
                return Unauthorized();
            }

            var model = new EventFormViewModel {
                Name = e.Name, 
                Description = e.Description, 
                End = e.End.ToString(EventDateTimeFormat),
                Start = e.Start.ToString(EventDateTimeFormat),
                TypeId = e.TypeId
            };

            model.Types = await GetTypes();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EventFormViewModel model, int id)
        {
            var e = await _context.Events.FindAsync(id);
            string userId = GetUserId();

            if (e == null)
            {
                return BadRequest();
            }
            if (e.OrganiserId != userId)
            {
                return Unauthorized();
            }

            DateTime start;
            DateTime end;

            if (!DateTime.TryParseExact(
                model.Start,
                EventDateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out start))
            {
                ModelState.AddModelError(nameof(model.Start), DateTimeError);
            }

            if (!DateTime.TryParseExact(
                model.End,
                EventDateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out end))
            {
                ModelState.AddModelError(nameof(model.End), DateTimeError);
            }

            if (!ModelState.IsValid)
            {
                model.Types = await GetTypes();

                return View(model);
            }


            e.Name = model.Name;
            e.Description = model.Description;
            e.Start = start;
            e.End = end;
            e.TypeId = model.TypeId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Details(int id) 
        {
            var model = await _context.Events
                .Where(ev => ev.Id == id)
                .AsNoTracking()
                .Select(ev => new EventDetailsViewModel
                {
                    Id = ev.Id,
                    Name = ev.Name,
                    Description = ev.Description,
                    End = ev.End.ToString(EventDateTimeFormat),
                    Start = ev.Start.ToString(EventDateTimeFormat),
                    Type = ev.Type.Name,
                    CreatedOn = ev.CreatedOn.ToString(EventDateTimeFormat),
                    Organiser = ev.Organiser.UserName
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        private string GetUserId() 
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        public async Task<IEnumerable<TypeViewModel>> GetTypes() 
        {
           return await _context.Types
                    .AsNoTracking()
                    .Select(t => new TypeViewModel
                    {
                        Id = t.Id,
                        Name = t.Name
                    }).ToListAsync();
        }
    }
}
