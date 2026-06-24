using AutoMapper;
using GymSystem.BLL.Helpers;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Enums;
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

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model , CancellationToken ct = default)
        {
            var sessionRepo = _iUnitOfWork.GetRepository<Session>();
            var trainerRepo = _iUnitOfWork.GetRepository<Trainer>();
            var trainer = await trainerRepo.GetByIdAsync(model.TrainerId);
            if (model is null) return Result.NotFound();
            if(model.StartTime > model.EndTime) return Result.Validation("Error!! End-Date Must Be After Start-Date");
            if( model.StartTime < DateTime.Now ) return Result.Validation("Error!! Start-Date Must Be In The Future");
            if (trainer is null) return Result.NotFound("Trainer Is Not Found");
            if( await _iUnitOfWork.SessionRepository.AnyAsync(
                    s => (
                    s.Trainer.Id == model.TrainerId && 
            (
            (model.StartTime >= s.StartTime && model.StartTime <= s.EndTime) ||
            (model.StartTime <= s.StartTime && model.EndTime >= s.EndTime) ||
            (model.StartTime >= s.StartTime && model.EndTime <= s.EndTime ) ||
            (model.EndTime >= s.StartTime && model.EndTime <= s.EndTime)
            ) 
            ), ct
            ) ) return Result.Validation("The Trainer Has A Session At The Same Time !!");

            var categoryRepo = _iUnitOfWork.GetRepository<Category>();
            var category = await categoryRepo.GetByIdAsync(model.CategoryId);
            if (category is null) return Result.NotFound("Category Is Not Found");
            if(trainer.Specialities != category.Name) return Result.Validation("Trainer Speciality Must Be Like Category");
            var session = _Mapper.Map<Session>(model);
            sessionRepo.AddAsync(session);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.OK() : Result.Fail("Failed To Create New Session");
        }

        public async Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int Id, CancellationToken ct = default)
        {
            var session = await _iUnitOfWork.GetRepository<Session>().GetByIdAsync(Id, ct);
            if(session is null) return null;
            return _Mapper.Map<UpdateSessionViewModel>(session);
        }
        public async Task<Result> UpdateSessionAsync(int Id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _iUnitOfWork.SessionRepository.GetSessionwithTrainerandCategoryByIdAsync(Id,ct);
            var bookings = await _iUnitOfWork.GetRepository<Booking>().AnyAsync(b => b.SessionId == Id && b.BookingDate > DateTime.Now, ct);
            if(session is null ) return Result.NotFound("Session Is Not Found");
            if (session.StartTime <= DateTime.Now && session.EndTime > DateTime.Now) return Result.Validation("Can't Update Completed or Ongoing Session");
            if (bookings) return Result.Validation("Can't Update Session That Already Booked");
            if (model.StartTime > model.EndTime) return Result.Validation("Error!! End-Date Must Be After Start-Date");
            if (model.StartTime < DateTime.Now) return Result.Validation("Error!! Start-Date Must Be In The Future");

            if (await _iUnitOfWork.SessionRepository.AnyAsync(
                        s => (
                        s.Trainer.Id == model.TrainerId &&
                (
                (model.StartTime >= s.StartTime && model.StartTime <= s.EndTime) ||
                (model.StartTime <= s.StartTime && model.EndTime >= s.EndTime) ||
                (model.StartTime >= s.StartTime && model.EndTime <= s.EndTime) ||
                (model.EndTime >= s.StartTime && model.EndTime <= s.EndTime)
                )
                ), ct
                )) return Result.Validation("The Trainer Has A Session At The Same Time !!");

            var trainerRepo = _iUnitOfWork.GetRepository<Trainer>();
            var trainer = await trainerRepo.GetByIdAsync(model.TrainerId);
            if (trainer is null) return Result.NotFound("Trainer Is Not Found");
            if (trainer.Specialities != session.Category.Name) return Result.Validation("Trainer Speciality Must Be Like Training Type");


            _Mapper.Map(model, session);
            session.UpdatedAt = DateTime.Now;
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Update Session");
        }
        public async Task<Result> RemoveSessionAsync(int Id, CancellationToken ct = default)
        {
            var session = await _iUnitOfWork.GetRepository<Session>().GetByIdAsync(Id, ct);
            var bookings = await _iUnitOfWork.GetRepository<Booking>().AnyAsync(b => b.SessionId == Id && b.BookingDate > DateTime.Now, ct);
            if (session is null || bookings) return Result.Validation("Can't Delete Session That Already Booked");

            _iUnitOfWork.SessionRepository.DeleteAsync(session);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Delete Session");
        }

        public async Task<IEnumerable<Trainer>> GetAllTrainers(CancellationToken ct = default)
        {
            var Trainers = await _iUnitOfWork.GetRepository<Trainer>().GetAllAsync(ct:ct);
            return Trainers.ToList();
        }

    }
}
