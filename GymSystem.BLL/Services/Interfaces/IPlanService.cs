using GymSystem.BLL.Helpers;
using GymSystem.BLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);
        Task<PlanViewModel?> GetPlanById(int Id, CancellationToken ct = default);
        Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int Id, CancellationToken ct = default);
        Task<bool> ToggleActivationAsync(int Id, CancellationToken ct = default);
        Task<Result> UpdatePlanAsync(int Id, UpdatePlanViewModel model, CancellationToken ct = default);
    }
}
