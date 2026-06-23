using AutoMapper;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IMapper _mapper;

        public PlanService( IUnitOfWork iUnitOfWork, IMapper mapper) 
        {
            _iUnitOfWork = iUnitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _iUnitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            if (plans is null)
                return [];
            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);

        }

        public async Task<PlanViewModel?> GetPlanById(int Id, CancellationToken ct = default)
        {
            var plan = await _iUnitOfWork.GetRepository<Plan>().GetByIdAsync(Id, ct);
            if (plan is null)
                return null;
            var planModel = _mapper.Map<PlanViewModel>(plan);
            return planModel;

        }

        public async Task<bool> HasActiveMembershipsAsync(int Id, CancellationToken ct = default)
        {
            return await _iUnitOfWork.GetRepository<Membership>().AnyAsync(ms => ms.PlanId == Id && ms.EndDate > DateTime.Now, ct);
        }
        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int Id, CancellationToken ct = default)
        {
            var plan = await _iUnitOfWork.GetRepository<Plan>().GetByIdAsync(Id, ct);
            if (plan is null || !plan.IsActive) return null;
            if(await HasActiveMembershipsAsync(Id , ct)) return null;
            return _mapper.Map<UpdatePlanViewModel>(plan);

        }

        public async Task<bool> ToggleActivationAsync(int Id, CancellationToken ct = default)
        {
            var plan = await _iUnitOfWork.GetRepository<Plan>().GetByIdAsync(Id, ct);
            if (plan is null) return false;


            if(plan.IsActive && await HasActiveMembershipsAsync(Id,ct)) return false;
            if (plan.IsActive)
                plan.IsActive = false;
            else plan.IsActive = true;

            plan.UpdatedAt = DateTime.Now;

            _iUnitOfWork.GetRepository<Plan>().UpdateAsync(plan);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> UpdatePlanAsync(int Id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _iUnitOfWork.GetRepository<Plan>().GetByIdAsync(Id, ct);
            if (plan is null || await HasActiveMembershipsAsync(Id , ct)) return false;

            plan.Name = model.PlanName;
            plan.Description = model.Description;
            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;
            plan.UpdatedAt = DateTime.Now;

            _iUnitOfWork.GetRepository<Plan>().UpdateAsync(plan);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }
    }
}


