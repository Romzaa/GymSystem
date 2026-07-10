using AutoMapper;
using GymSystem.BLL.Helpers;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.BookingViewModels;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymSystem.BLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService( IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateBookingAsync(CreateBookingViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(model.SessionId, ct);
            if(session is null) return Result.NotFound("Session not found");
            if(session.StartTime <= DateTime.Now) return Result.Fail("Cannot book a session that has already started");

            var hasActiveMembership = await _unitOfWork.MembershipRepository.AnyAsync(m => m.MemberId == model.MemberId && m.EndDate >= DateTime.Now, ct);
            if (!hasActiveMembership) return Result.Fail("Member does not have an active membership");

            var bookedSession = await _unitOfWork.BookingRepository.AnyAsync(b=>b.SessionId == model.SessionId && b.MemberId == model.MemberId , ct);
            if(bookedSession) return Result.Fail("Member has already booked this session");

            var complete = await _unitOfWork.SessionRepository.GetAvailableSlotsAsync(model.SessionId, ct);
            if(complete <= 0) return Result.Fail("Session is fully booked");

            _unitOfWork.BookingRepository.AddAsync(new Booking { MemberId = model.MemberId, SessionId = model.SessionId, BookingDate = DateTime.Now  });
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed to create booking");

        }

        public async Task<Result> DeleteBookingAsync(int MemberId, int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(SessionId, ct);
            if(session is null)
            {
                return Result.NotFound("Session not found");
            }
            if(session.StartTime <= DateTime.Now)
            {
                return Result.Fail("Cannot delete booking for a session that has already started");
            }
            
            var booking = await _unitOfWork.BookingRepository.FirstOrDefaultAsync(b => b.SessionId == SessionId && b.MemberId == MemberId, ct:ct);
            if(booking is null)
            {
                return Result.NotFound("Booking not found");
            }
            
            _unitOfWork.BookingRepository.DeleteAsync(booking);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed to delete booking");
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllBookingsWithMembersAndSessions(Expression<Func<Session, bool>>? filter, CancellationToken ct = default)
        {
            var Sessions = await _unitOfWork.SessionRepository.GetAllSessionswithTrainerandCategory(s => s.EndTime >= DateTime.Now ,ct);
            if (!Sessions.Any()) return null!;

          var activeSessions = _mapper.Map<IEnumerable<SessionViewModel>>(Sessions);
            foreach (var session in activeSessions)
            {
               session.AvailableSlots = await _unitOfWork.SessionRepository.GetAvailableSlotsAsync(session.Id, ct);
            }
            return activeSessions;
        }

        public async Task<IEnumerable<MemberBookingViewModel?>> GetAllMembersForBooking(int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetAllBookingsWithMembersAndSessions(b => b.SessionId == sessionId, ct: ct);
            return bookings.Select(b => new MemberBookingViewModel
            {
                MemberId = b.MemberId,
                SessionId = b.SessionId,
                MemberName = $"{b.Member.Name}",
                BookingDate = b.BookingDate.ToString("yyyy-MM-dd HH:mm:ss"),
                IsAttended = b.IsAttended,
            }).ToList();

        }

        public async Task<IEnumerable<MembersListViewModel>> GetMemberListAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetAllAsync(b => b.SessionId == sessionId, ct:ct);
            var bookingmembers = bookings.Select(b => b.MemberId).ToList();
            var avilableMembers = await _unitOfWork.GetRepository<Member>().GetAllAsync(m => !bookingmembers.Contains(m.Id), ct: ct);
            return _mapper.Map<IEnumerable<MembersListViewModel>>(avilableMembers);
        }

        public async Task<Result> MarkAttendedAsync(int MemberId, int SessionId, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.BookingRepository.FirstOrDefaultAsync(b => b.SessionId == SessionId && b.MemberId == MemberId, ct: ct);
            if(booking is null) return Result.NotFound("Booking not found");
            booking.IsAttended = true;
            booking.UpdatedAt = DateTime.Now;

            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed to mark attendance");
        }
    }
}
