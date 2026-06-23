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

        public TrainerService(IUnitOfWork iUnitOfWork) 
        {
            _iUnitOfWork = iUnitOfWork;
        }
        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel viewModel, CancellationToken ct = default)
        {
            var trainerRepo = _iUnitOfWork.GetRepository<Trainer>();

            var emailExists =await trainerRepo.AnyAsync(t => t.Email == viewModel.Email);
            var phoneExists =await trainerRepo.AnyAsync(t => t.Phone == viewModel.Phone);

            if (emailExists || phoneExists)
                return false;
            var trainer = new Trainer
            {

                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Gender = viewModel.Gender,
                DateOfBirth = viewModel.DateOfBirth,
                Specialities = viewModel.Specialities,
                Address = new Address
                {
                    BuildingNumber = viewModel.BuildingNumber,
                    City = viewModel.City,
                    Street = viewModel.Street
                }

            };
            trainerRepo.AddAsync(trainer);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _iUnitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            if (!trainers.Any()) return [];
            return trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Photo = t.Photo,
                Gender = t.Gender,
                Address = $"{t.Address.BuildingNumber} - {t.Address.Street} - {t.Address.City}",
                Specialities = t.Specialities
            });
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int Id, CancellationToken ct = default)
        {
            var trainer = await _iUnitOfWork.GetRepository<Trainer>().GetByIdAsync(Id , ct);
            if(trainer is null) return null;
            return new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Gender = trainer.Gender,
                Photo = trainer.Photo,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Specialities = trainer.Specialities,
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}"

            };


        
        }

        public async Task<UpdateTrainerViewModel?> GetTrainerToUpdateAsync(int Id, CancellationToken ct = default)
        {
            var trainer = await _iUnitOfWork.GetRepository<Trainer>().GetByIdAsync(Id, ct);
            if (trainer is null) return null;
            var trainerViewModel = new UpdateTrainerViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialities = trainer.Specialities,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City

            };
            return trainerViewModel;
        }
        public async Task<bool> UpdateTrainerAsync(int Id, UpdateTrainerViewModel model, CancellationToken ct = default)
        {
            var trainerRepo = _iUnitOfWork.GetRepository<Trainer>();
            if (model is null) return false;
            var emailExists = await trainerRepo.AnyAsync(t => t.Email == model.Email && t.Id != Id,ct);
            var phoneExists = await trainerRepo.AnyAsync(t => t.Phone == model.Phone && t.Id != Id,ct);

            if (emailExists || phoneExists) return false;

        var trainer = await trainerRepo.GetByIdAsync(Id, ct);
                if(trainer is null) return false;
                trainer.Name = model.Name;
                trainer.Email = model.Email;
                trainer.Phone = model.Phone;
                trainer.Address.BuildingNumber = model.BuildingNumber;
                trainer.Address.Street = model.Street;
                trainer.Address.City = model.City;
                trainer.UpdatedAt = DateTime.Now;
            trainerRepo.UpdateAsync(trainer);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
        public async Task<bool> RemoveTrainerAsync(int Id, CancellationToken ct = default)
        {
            var trainer = await _iUnitOfWork.GetRepository<Trainer>().GetByIdAsync(Id, ct);
            var hasSessions = await _iUnitOfWork.GetRepository<Session>().AnyAsync(s => s.TrainerId == Id && s.StartTime > DateTime.Now, ct);
            if (trainer is null || hasSessions) return false;

            _iUnitOfWork.GetRepository<Trainer>().DeleteAsync(trainer);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }


    }
}
