using GymSystem.BLL.Services.Interfaces;
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
        private readonly IMemberService _memberService;
        private readonly GymDbContext _dbContext;
        public HomeController(IMemberService memberService , GymDbContext gymDbContext)
        {
            _memberService = memberService;
            _dbContext = gymDbContext;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            ViewBag.MembersCount = _memberService.GetTotalMembersAsync(_dbContext, ct);
            ViewBag.ActiveMembers = await _memberService.GetActiveMembersAsync(_dbContext, ct);
        
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
