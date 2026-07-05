using GymSystem.BLL.Services.AttachementService;
using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using GymSystem.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;
        private readonly IAttachmentService _attachmentService;

        public TrainersController(ITrainerService trainerService, IAttachmentService attachmentService)
        {
            _trainerService = trainerService;
            _attachmentService = attachmentService;
        }


        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var trainers = await _trainerService.GetAllTrainersAsync(ct);
            if(!trainers.Any())
            {
                TempData["ErrorMessage"] = "No Trainers Available";
            }
            return View(trainers);
        }

        public async Task<IActionResult> Details(int Id, CancellationToken ct = default) 
        {
            var trainer = await _trainerService.GetTrainerDetailsAsync(Id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Is Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        
        }


        #region Create

        // Create - Display Create Form => GET
        public IActionResult Create(CancellationToken ct = default)
        {
            return View();
        }

        // Create - Submit Create => POST
        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model , CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Something Went Wrong Please Try Again";
                return View(model);
            }
            var result = await _trainerService.CreateTrainerAsync(model, ct);
            if (!result.success) 
            { 
                TempData["ErrorMessage"] = result.errorMessage;
                return View(model);
            }
            TempData["SuccessMessage"] = "Trainer Was Created Successfully";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        // Edit - Display Edit Form => GET
        public async Task<IActionResult> Edit(int Id,CancellationToken ct = default)
        {
            var trainer = await _trainerService.GetTrainerToUpdateAsync(Id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Is Not Found";
                return RedirectToAction(nameof(Index));
            
            }
            return View(trainer);
        }

        // Edit - Submit Edit => POST
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]int Id,UpdateTrainerViewModel model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Something Went Wrong Please Try Again";
                return View(model);
            }
            var result = await _trainerService.UpdateTrainerAsync(Id, model, ct);
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.errorMessage;
                return View(model);
            }
            TempData["SuccessMessage"] = "Trainer Was Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete
        // Edit - Display Edit Form => GET
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var trainer = await _trainerService.GetTrainerDetailsAsync(id, ct);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // Edit - Submit Edit => POST
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int Id, CancellationToken ct = default)
        {

            var result = await _trainerService.RemoveTrainerAsync(Id, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Trainer Was Removed Successfully";
                return RedirectToAction(nameof(Index));

            }
            else { 
            TempData["ErrorMessage"] = result.errorMessage;
            return RedirectToAction(nameof(Index));
            }
        }


        #endregion

        public async Task<IActionResult> Picture(int Id)
        {
            var trainer = await _trainerService.GetTrainerDetailsAsync(Id);
            if(trainer is null || string.IsNullOrEmpty(trainer.Photo))
            return NotFound();

            var result =  _attachmentService.GetFile(trainer.Photo, "TrainersPitures");
            if(result is null)
                return NotFound();

         return  File(result.Value.stream , result.Value.contentType);

        }



    }
}
