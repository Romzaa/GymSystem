using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Classes;
using GymSystem.DAL.Repositries.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace GymSystem.PL.Controllers
{
    public class PlansController : Controller
    {
        private readonly IGenericRepository<Plan> _planRepo;
        public PlansController(IGenericRepository<Plan> Iplan)
        {
            _planRepo = Iplan;
        }


        public async Task <IActionResult> Index( CancellationToken ct )
        {
            var plans = await _planRepo.GetAllAsync(ct: ct);
            return View(plans);
        }


        public async Task<IActionResult> Details(int id,  CancellationToken ct)
        { var plan = await _planRepo.GetByIdAsync(id, ct);
            if (plan == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

    }
}
