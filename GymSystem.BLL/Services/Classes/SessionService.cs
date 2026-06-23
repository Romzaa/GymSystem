using AutoMapper;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IMapper _Mapper;

        public SessionService( IUnitOfWork iUnitOfWork, IMapper Mapper)
        {
            _iUnitOfWork = iUnitOfWork;
            _Mapper = Mapper;
        }
        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _iUnitOfWork.SessionRepository.GetAllSessionswithTrainerandCategory(ct);
            if (sessions is null)
                return [];
            return _Mapper.Map<IEnumerable<SessionViewModel>>(sessions.OrderByDescending(s => s.StartTime));
        }

        public async Task<SessionViewModel?> GetSessionDetailsAsync(int Id, CancellationToken ct = default)
        {
            var session = await _iUnitOfWork.SessionRepository.GetSessionwithTrainerandCategoryByIdAsync(Id,ct);
            if(session is null) return null;
            return _Mapper.Map<SessionViewModel>(session);
        
        }

        public async Task<bool> CreateSessionAsync(CreateSessionViewModel model , CancellationToken ct = default)
        {
            var sessionRepo = _iUnitOfWork.GetRepository<Session>();
            var session = _Mapper.Map<Session>(model);
            if(session is null) return false;
            sessionRepo.AddAsync(session);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);

            return result > 0;
        }

        public async Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int Id, CancellationToken ct = default)
        {
            var session = await _iUnitOfWork.GetRepository<Session>().GetByIdAsync(Id, ct);
            if(session is null) return null;
            return _Mapper.Map<UpdateSessionViewModel>(session);
        }
        public async Task<bool> UpdateSessionAsync(int Id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _iUnitOfWork.GetRepository<Session>().GetByIdAsync(Id, ct);
            var bookings = await _iUnitOfWork.GetRepository<Booking>().AnyAsync(b => b.SessionId == Id && b.BookingDate > DateTime.Now, ct);
            if (session is null || bookings) return false;
            session.Description = model.Description;
            session.StartTime = model.StartTime;
            session.EndTime = model.EndTime;
            session.TrainerId = model.TrainerId;

            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
        public async Task<bool> RemoveSessionAsync(int Id, CancellationToken ct = default)
        {
            var session = await _iUnitOfWork.GetRepository<Session>().GetByIdAsync(Id, ct);
            var bookings = await _iUnitOfWork.GetRepository<Booking>().AnyAsync(b => b.SessionId == Id && b.BookingDate > DateTime.Now, ct);
            if (session is null || bookings) return false;

            _iUnitOfWork.SessionRepository.DeleteAsync(session);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<IEnumerable<Trainer>> GetAllTrainers(CancellationToken ct = default)
        {
            var Trainers = await _iUnitOfWork.GetRepository<Trainer>().GetAllAsync(ct:ct);
            return Trainers.ToList();
        }

    }
}
