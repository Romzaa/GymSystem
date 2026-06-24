using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                TempData["ErrorMessage"] = "Session Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        #region Create

        // Create - Display => GET
        public async Task<IActionResult> Create(CancellationToken ct = default)
        {
            var Trainers =await _sessionService.GetAllTrainers(ct);
            ViewBag.Trainers = Trainers.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();
            return View();
        }

        // Create - Action => POST
        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model ,CancellationToken ct = default)
        {
            var Trainers = await _sessionService.GetAllTrainers(ct);
            ViewBag.Trainers = Trainers.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Couldn't Create New Session";
                return RedirectToAction(nameof(Index));
            }
            var result = await _sessionService.CreateSessionAsync(model, ct);
            if(!result.success)
            {
                TempData["ErrorMessage"] = result.errorMessage;
                return View(model);
            }
            TempData["SuccessMessage"] = "Session Is Created Successfully";
            return RedirectToAction(nameof(Index));
        }


        #endregion


        #region Edit

        // Edit - Display => GET
        public async Task<IActionResult> Edit([FromRoute] int Id, CancellationToken ct =default )
        {
            var Trainers = await _sessionService.GetAllTrainers(ct);
            var session = await _sessionService.GetSessionToUpdateAsync(Id, ct);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Trainers = Trainers.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();
            return View(session);
        }

        // Edit - Action => POST
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int Id,UpdateSessionViewModel model ,CancellationToken ct =default)
        {
            var Trainers = await _sessionService.GetAllTrainers(ct);
            ViewBag.Trainers = Trainers.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid Inputs";
                return View(model);
            }
            var result = await _sessionService.UpdateSessionAsync(Id, model, ct);
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.errorMessage;
                return View(model);

            }else
            TempData["SuccessMessage"] = "Session Is Updated Successfully";
            return RedirectToAction(nameof(Index));

        }

        #endregion

        #region Delete

        // Delete - Display => GET

        public async Task<IActionResult> Delete([FromRoute] int Id, CancellationToken ct =default)
        {
            var session = await _sessionService.GetSessionDetailsAsync(Id, ct);
            if (session == null)
            {
                TempData["ErrorMessage"] = "Session Is Not Available";
                return null!;
            }
            return View();
        }

        // Delete - Confirm Delete => POST
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int Id , CancellationToken ct =default) 
        {
            var result = await _sessionService.RemoveSessionAsync(Id, ct);
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.errorMessage;
                return RedirectToAction(nameof(Index));
            }
            TempData["SuccessMessage"] = "Session Is Deleted Successfully";
            return RedirectToAction(nameof(Index));

        }

    }

        #endregion

    
}
