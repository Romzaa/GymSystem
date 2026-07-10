using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.BookingViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.PL.Controllers
{
    [Authorize]

    public class BookingsController : Controller
    {


        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }


        public async Task<IActionResult> Index(CancellationToken ct =default)
        {
            var sessions = await _bookingService.GetAllBookingsWithMembersAndSessions(null,ct);

            return View(sessions);
        }


        public async Task<IActionResult> Create(int id, CancellationToken ct =default)
        {
            var members = await _bookingService.GetMemberListAsync(id,ct);
            ViewBag.Members = new SelectList(members, "MemberId", "MemberName");
            ViewBag.SessiondId = id;
            return View();

        }


        [HttpPost]

        public async Task<IActionResult> Create(CreateBookingViewModel model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {

                TempData["ErrorMessage"] = "Please Enter Valid Data";
            return View();

            }

            var result = await _bookingService.CreateBookingAsync(model, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Is Booked Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var members = await _bookingService.GetMemberListAsync(model.MemberId, ct);
                ViewBag.Members = new SelectList(members, "MemberId", "MemberName");
                ViewBag.SessiondId = model.SessionId;
                TempData["ErrorMessage"] = result.errorMessage;
                return View(model);
            }

        }


        public async Task<IActionResult> GetMembersForUpcomingSession(int Id, CancellationToken ct)
        {
            var members = await _bookingService.GetAllMembersForBooking(Id, ct);
            return View(members);
        }
        public async Task<IActionResult> GetMembersForOngoingSessions(int Id, CancellationToken ct)
        {
            var members = await _bookingService.GetAllMembersForBooking(Id, ct);
            return View(members);
        }


    }
}
