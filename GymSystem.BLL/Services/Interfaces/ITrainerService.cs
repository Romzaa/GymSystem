using GymSystem.BLL.Helpers;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        public Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default);
        public Task<TrainerViewModel?> GetTrainerDetailsAsync(int Id, CancellationToken ct = default);
        Task<Result> CreateTrainerAsync(CreateTrainerViewModel viewModel, CancellationToken ct = default);
        Task<UpdateTrainerViewModel?> GetTrainerToUpdateAsync(int Id, CancellationToken ct = default);
        Task<Result> UpdateTrainerAsync(int Id, UpdateTrainerViewModel model, CancellationToken ct = default);
        Task<Result> RemoveTrainerAsync(int Id, CancellationToken ct = default);

    }
}
