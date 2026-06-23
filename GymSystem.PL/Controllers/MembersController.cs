using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL;
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

        public  async Task<IActionResult> MemberDetails(int Id , CancellationToken ct)
        {
            var member =await _memberService.GetMemberDetailsAsync(Id, ct);
            if(member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found ! ";
               return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        public async Task<IActionResult> HealthRecordDetails(int Id, CancellationToken ct)
        {
            var healthRecord = await _memberService.GetMemberHealthRecordAsync(Id, ct);
            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }
       
        
        #region Create

        // Create - Display => GET
        public IActionResult Create()
        {
            return View();
        }

        // Create - Action => POST
        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel viewModel, CancellationToken ct)
        {
            if (!ModelState.IsValid)
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

        #endregion

        #region Edit

        // Edit - Display Edit FORM => GET

        public async Task<IActionResult> EditMember([FromRoute]int Id, CancellationToken ct = default)
        {
            var member = await _memberService.GetMemberToUpdateAsync(Id, ct);
            if(member is null)
            {
                TempData["ErrorMessage"] = "Member Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // Edit - Submit Updates => POST
        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute]int Id,UpdateMemberViewModel model , CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
        var result =await _memberService.UpdateMemberAsync(Id, model, ct);
            if(result  == false)
            {
                TempData["ErrorMessage"] = "Failed To Update Member";
                View(model);
            }
            TempData["SuccessMessage"] = "Member Updated Successfully";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        // Delete - Display Delete Alert => GET
        public async Task<IActionResult> Delete(int Id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsAsync(Id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Is Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // Delete - Confirm Delete Member => POST
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int Id, CancellationToken ct)
        {
            var result = await _memberService.RemoveMemberAsync(Id, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Is Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete Member";
            }
            return RedirectToAction(nameof(Index));


        }

        #endregion





    }
}
