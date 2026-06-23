using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Classes;
using GymSystem.DAL.Repositries.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace GymSystem.PL.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanService _planService;

        public PlansController(IPlanService planService
                              )
        {
            _planService = planService;
        }


        public async Task <IActionResult> Index( CancellationToken ct )
        {
            var plans = await _planService.GetAllPlansAsync(ct);

            return View(plans);
        }


        public async Task<IActionResult> Details(int id,  CancellationToken ct)
        { var plan = await _planService.GetPlanById(id, ct);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan Is Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        public async Task<IActionResult> Activate(int Id, CancellationToken ct)
        {
            var result =  await _planService.ToggleActivationAsync(Id, ct);
            if (result) {
            TempData["SuccessMessage"] = "Plan Status Changed Successfully";
            }else if (!result) { 
            TempData["ErrorMessage"] = "Failed To Change Plan Status";
            }
            return RedirectToAction(nameof(Index));
        }


        // Edit - Display Plan Update Form => GET
        public async Task<IActionResult> Edit(int Id, CancellationToken ct)
        {
            var plan = await _planService.GetPlanToUpdateAsync(Id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            else return View(plan);


        }
        [HttpPost]
        public async Task<IActionResult> Edit(int Id, UpdatePlanViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid) 
            { 
                return View(model);

            }
            var result = await _planService.UpdatePlanAsync(Id, model, ct);
            if(result == false)
            {
                TempData["ErrorMessage"] = "Failed To Update Plan";
            }
            else
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            }
            return RedirectToAction(nameof(Index));

        }

    }
}
