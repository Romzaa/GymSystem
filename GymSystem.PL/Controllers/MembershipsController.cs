using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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


        // Create => Display => GET

        public async Task<IActionResult> Create(CancellationToken ct = default)
        {
            await PopulateDropDownAsync(ct);
            return View();
        }

        // Create => Process => POST

        [HttpPost]
        public async Task<IActionResult> Create(CreateMembershipViewModel model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data. Please check the form and try again.";
                await PopulateDropDownAsync(ct);
                foreach (var error in ModelState)
                {
                    foreach (var e in error.Value.Errors)
                    {
                        Console.WriteLine($"{error.Key} : {e.ErrorMessage}");
                    }
                }
                return View(model);
            }
            var result = await _membershipService.CreateMembershipAsync(model, ct);
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.errorMessage;
                await PopulateDropDownAsync(ct);
                return View(model);

            }
            else
            {
                TempData["SuccessMessage"] = "Membership created successfully.";
                return RedirectToAction(nameof(Index));

            }
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int Id, CancellationToken ct = default)
        {
            var result = await _membershipService.DeleteActiveMembershipAsync(Id, ct);
            if (result.success)
            {   TempData["SuccessMessage"] = "Membership canceled successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.errorMessage;
              return  RedirectToAction(nameof(Index));

            }
        }

        private async Task PopulateDropDownAsync(CancellationToken ct = default)
        {
            var plans = await _membershipService.GetPlansListAsync(ct);
            var members = await _membershipService.GetMembersListAsync(ct);
            if (plans == null)
                throw new Exception("Plans is null");

            if (members == null)
                throw new Exception("Members is null");

            ViewBag.Plans = plans.Select(p => new SelectListItem { Value = p.PlanId.ToString(), Text = p.PlanName }).ToList();
            ViewBag.Members = members.Select(m => new SelectListItem { Value = m.MemberId.ToString(), Text = m.MemberName }).ToList();

        }

    }
}
