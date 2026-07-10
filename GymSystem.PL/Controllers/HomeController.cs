using GymSystem.BLL.Services.AnalyticsService;
using GymSystem.DAL;
using GymSystem.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymSystem.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            ViewBag.MembersCount = await _analyticsService.GetTotalMembersAsync( ct);
            ViewBag.ActiveMembers = await _analyticsService.GetActiveMembersAsync(ct);
            ViewBag.TrainersCount = await _analyticsService.GetTotalTrainersAsync(ct);
            ViewBag.UpcomingSessions = await _analyticsService.GetUpcomingSessionsAsync(ct);
            ViewBag.OngoingSessions = await _analyticsService.GetOngoingSessionsAssync(ct);
            ViewBag.CompletedSessions = await _analyticsService.GetCompletedSessionsAsync(ct);
            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    
    }
}
