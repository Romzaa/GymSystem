using GymSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public async Task<IActionResult> Index(CancellationToken ct =default)
        {
            var sessions = await _sessionService.GetAllSessionsAsync(ct);
            if(!sessions.Any())
            {
                TempData["ErrorMessage"] = "No Sessions Available";
                return View(sessions);
            }
            return View(sessions);
        }

        public async Task<IActionResult> Details(int Id, CancellationToken ct = default)
        {
            var session = await _sessionService.GetSessionDetailsAsync(Id,ct);
            if(session is null)
            {
                TempData["ErrorMessage"] = "ErrorMessage";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }
    }
}
