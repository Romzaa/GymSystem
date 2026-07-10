using AutoMapper;
using GymSystem.BLL.Helpers;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default)
        {
            var memberships = await _unitOfWork.MembershipRepository.GetAllMembershipsWithPlansAndMembers(m => m.EndDate > DateTime.Now , ct);
            var result = _mapper.Map<IEnumerable<MembershipViewModel>>(memberships);

            return result;
        }
        public async Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default)
        {
            if (model is null)
            {
                return Result.Fail("Model Is Empty");
            }

            var member = await _unitOfWork.GetRepository<Member>().AnyAsync(m=> m.Id == model.MemberId, ct);
            if(!member)
            {
                return Result.Fail("Member Not Found");
            }
            var hasActiveMembership = await _unitOfWork.MembershipRepository.AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now, ct);
            if(hasActiveMembership)
            {
                return Result.Fail("Member Already Has An Active Membership");
            } 

            var plan = await _unitOfWork.GetRepository<Plan>().FirstOrDefaultAsync(p=> p.Id == model.PlanId, ct:ct);
            if(plan is null)
            {
                return Result.Fail("Plan Not Found");
            }

            var membership = _mapper.Map<Membership>(model);
            membership.StartDate = model.StartDate ?? DateTime.Now;
            membership.EndDate = membership.StartDate.AddDays(plan.DurationDays);
            _unitOfWork.MembershipRepository.AddAsync(membership);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed to create membership");

        }

        public async Task<Result> DeleteActiveMembershipAsync(int Id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.MembershipRepository.FirstOrDefaultAsync(m => m.MemberId == Id && m.EndDate > DateTime.Now,true, ct);
            if (member is null)
            {
                return Result.Fail("This Member Doesn't Have Active Membership");
            }

            _unitOfWork.MembershipRepository.DeleteAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Delete Membership");
        }



        public  async Task<IEnumerable<MembersListViewModel>> GetMembersListAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(null,ct: ct);
            return _mapper.Map<IEnumerable<MembersListViewModel>>(members);
            
        }

        public async Task<IEnumerable<PlansListViewModel>> GetPlansListAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(null,ct: ct);
            return _mapper.Map<IEnumerable<PlansListViewModel>>(plans);
        }


    }
}
