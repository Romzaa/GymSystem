using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async Task<IActionResult> Index( CancellationToken ct)
        {
            var members = await _memberService.GetAllMembersAsync(ct);
            return View(members);
        }

        // Create - Display => GET
        public IActionResult Create()
        {
            return View();
        }

        // Create - Action => POST
        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel viewModel, CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var result = await _memberService.CreateMemberAsync(viewModel, ct);

            if (result)
            {
                TempData["SuccessMessage"] = "Member created successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create member. Please try again.";
            }


                return RedirectToAction(nameof(Index));
        }

    }
}
