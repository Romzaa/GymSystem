using AutoMapper;
using GymSystem.BLL.Helpers;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Interfaces;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GymSystem.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork iUnitOfWork, IMapper mapper) 
        {
            _iUnitOfWork = iUnitOfWork;
            _mapper = mapper;
        }
        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel viewModel, CancellationToken ct = default)
        {
            var trainerRepo = _iUnitOfWork.GetRepository<Trainer>();

            var emailExists =await trainerRepo.AnyAsync(t => t.Email == viewModel.Email);
            var phoneExists =await trainerRepo.AnyAsync(t => t.Phone == viewModel.Phone);
            if (phoneExists)
                return Result.Validation("This Phone Already Exists");
            if (emailExists)
                return Result.Validation("This Email Already Exists");
            var trainer =  _mapper.Map<Trainer>(viewModel);
            trainerRepo.AddAsync(trainer);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Create New Trainer");
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _iUnitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            if (!trainers.Any()) return [];
            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int Id, CancellationToken ct = default)
        {
            var trainer = await _iUnitOfWork.GetRepository<Trainer>().GetByIdAsync(Id , ct);
            if(trainer is null) return null;
            return _mapper.Map<TrainerViewModel>(trainer);

        }

        public async Task<UpdateTrainerViewModel?> GetTrainerToUpdateAsync(int Id, CancellationToken ct = default)
        {
            var trainer = await _iUnitOfWork.GetRepository<Trainer>().GetByIdAsync(Id, ct);
            if (trainer is null) return null;
            var trainerViewModel = _mapper.Map< UpdateTrainerViewModel>(trainer);
            return trainerViewModel;
        }
        public async Task<Result> UpdateTrainerAsync(int Id, UpdateTrainerViewModel model, CancellationToken ct = default)
        {
            var trainerRepo = _iUnitOfWork.GetRepository<Trainer>();
            if (model is null) return Result.NotFound("Trainer Is Not Found");
            var emailExists = await trainerRepo.AnyAsync(t => t.Email == model.Email && t.Id != Id,ct);
            var phoneExists = await trainerRepo.AnyAsync(t => t.Phone == model.Phone && t.Id != Id,ct);

            if (phoneExists)
                return Result.Validation("This Phone Is Already Linked To A Trainer");
            if (emailExists)
                return Result.Validation("This Email Is Already Linked To A Trainer");

            var trainer = await trainerRepo.GetByIdAsync(Id, ct);
                if(trainer is null) return Result.NotFound("Trainer Is Not Found");
                trainer.Name = model.Name;
                trainer.Email = model.Email;
                trainer.Phone = model.Phone;
                trainer.Address.BuildingNumber = model.BuildingNumber;
                trainer.Address.Street = model.Street;
                trainer.Address.City = model.City;
                trainer.UpdatedAt = DateTime.Now;
            trainerRepo.UpdateAsync(trainer);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Update Trainer");
        }
        public async Task<Result> RemoveTrainerAsync(int Id, CancellationToken ct = default)
        {
            var trainer = await _iUnitOfWork.GetRepository<Trainer>().GetByIdAsync(Id, ct);
            var hasSessions = await _iUnitOfWork.GetRepository<Session>().AnyAsync(s => s.TrainerId == Id && s.StartTime > DateTime.Now, ct);
            if (trainer is null) return Result.NotFound("Trainer Is Not Found");
            if (hasSessions) return Result.Validation("Can't Remove A Trainer That Has Future Sessions");

            _iUnitOfWork.GetRepository<Trainer>().DeleteAsync(trainer);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Delete Trainer");
        }


    }
}
