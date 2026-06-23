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
            return sessions.OrderByDescending(s => s.StartTime).Select(s => new SessionViewModel
            {
                Id = s.Id,
                Description = s.Description,
                Capacity = s.Capacity,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                TrainerName = s.Trainer.Name,
                CategoryName = s.Category.Name,
                AvailableSlots = s.AvailableSlots,
            });
        }

        public async Task<SessionViewModel?> GetSessionDetailsAsync(int Id, CancellationToken ct = default)
        {
            var session = await _iUnitOfWork.SessionRepository.GetSessionwithTrainerandCategoryByIdAsync(Id,ct);
            if(session is null) return null;
            return new SessionViewModel
            {
                Id = session.Id,
                Description = session.Description,
                Capacity = session.Capacity,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                TrainerName = session.Trainer.Name,
                CategoryName = session.Category.Name,
                AvailableSlots = session.AvailableSlots
                
            };
        
        }

        public async Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int Id, CancellationToken ct = default)
        {
            var session = await _iUnitOfWork.GetRepository<Session>().GetByIdAsync(Id, ct);
            if(session is null) return null;
            return new UpdateSessionViewModel
            {
                Description = session.Description,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                TrainerId = session.TrainerId
            };
        }
        public async Task<bool> UpdateSessionAsync(int Id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _iUnitOfWork.GetRepository<Session>().GetByIdAsync(Id, ct);
            var futureSession = await _iUnitOfWork.GetRepository<Session>().AnyAsync(s => s.StartTime > DateTime.Now);
            var bookings = await _iUnitOfWork.GetRepository<Booking>().AnyAsync(b => b.SessionId == Id && b.BookingDate > DateTime.Now, ct);
            if (session is null || futureSession || bookings) return false;
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
            var futureSession = await _iUnitOfWork.GetRepository<Session>().AnyAsync(s => s.StartTime > DateTime.Now);
            var bookings = await _iUnitOfWork.GetRepository<Booking>().AnyAsync(b => b.SessionId == Id && b.BookingDate > DateTime.Now, ct);
            if (session is null || futureSession || bookings) return false;

            _iUnitOfWork.SessionRepository.DeleteAsync(session);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }


    }
}
