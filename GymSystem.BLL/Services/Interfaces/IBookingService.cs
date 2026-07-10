using GymSystem.BLL.Helpers;
using GymSystem.BLL.ViewModels.BookingViewModels;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Classes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IBookingService
    {
        public Task<IEnumerable<SessionViewModel>> GetAllBookingsWithMembersAndSessions(Expression<Func<Session, bool>>? filter, CancellationToken ct = default);
        public Task<IEnumerable<MemberBookingViewModel?>> GetAllMembersForBooking(int sessionId, CancellationToken ct = default);
        public Task<IEnumerable<MembersListViewModel>> GetMemberListAsync(int sessionId, CancellationToken ct = default);
        public Task<Result> CreateBookingAsync(CreateBookingViewModel model, CancellationToken ct = default);
        public Task<Result> DeleteBookingAsync(int MemberId, int SessionId, CancellationToken ct = default);
        public Task<Result> MarkAttendedAsync(int MemberId, int SessionId, CancellationToken ct = default);

    }
}
