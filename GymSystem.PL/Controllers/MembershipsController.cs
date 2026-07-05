using GymSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    [Authorize]
    public class MembershipsController : Controller
    {
        public readonly IMembershipService _membershipService;
        public MembershipsController(ILogger<MembershipsController> logger , IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var memberships = await _membershipService.GetAllMembershipsAsync(ct);
            if(memberships == null)
            {
                TempData["ErrorMessage"] = "No memberships found.";
                return View(memberships);
            }
        
            return View(memberships);
        }

            
    }
}
