using AutoMapper;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork IUnitOfWork, IMapper mapper)
        {
            _iUnitOfWork = IUnitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel viewModel, CancellationToken ct = default)
        {
            var memberRepo =  _iUnitOfWork.GetRepository<Member>();

            var emailExists =await memberRepo.AnyAsync(m => m.Email == viewModel.Email, ct);
            var phoneExists =await memberRepo.AnyAsync(m => m.Phone == viewModel.Phone, ct);
            if (emailExists || phoneExists)
                return false;
            var member = _mapper.Map<Member>(viewModel);
             memberRepo.AddAsync(member);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0;
            
            }


        public  async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _iUnitOfWork.GetRepository<Member>().GetAllAsync(ct:ct);
            if (!members.Any())
                return [];
            return _mapper.Map<IEnumerable<MemberViewModel>>(members);


        }


        public int GetTotalMembersAsync(GymDbContext gdbcontext, CancellationToken ct = default)
        {
            return gdbcontext.Members.Count();
        }

        public async Task<int> GetActiveMembersAsync(GymDbContext gdbcontext, CancellationToken ct = default)
        {
            var result = gdbcontext.Memberships.CountAsync(ms => ms.EndDate > (DateTime.Now), ct);
            return await result ;
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int Id, CancellationToken ct )
        {
            var member = await _iUnitOfWork.GetRepository<Member>().GetByIdAsync(Id,ct);
            if (member is null)
                return null;
            var memberViewModel = _mapper.Map<MemberViewModel>(member);

            var membership =await _iUnitOfWork.GetRepository<Membership>().FirstOrDefaultAsync((m => m.Id == Id && m.EndDate > DateTime.Now) , false , ct);

            if(membership is not null)
            {
                var plan = await _iUnitOfWork.GetRepository<Plan>().GetByIdAsync(membership.PlanId, ct);
                memberViewModel.PlanName = plan?.Name!;
                memberViewModel.MembershipStartDate = membership.StartDate.ToShortDateString();
                memberViewModel.MembershipEndDate = membership.EndDate.ToShortDateString();

            }

            return memberViewModel;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int Id, CancellationToken ct = default)
        {
            var healthRecord = await _iUnitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(h => h.MemberId == Id,false, ct);
            if (healthRecord is null)
                return null;
            var newModel = _mapper.Map<HealthRecordViewModel>(healthRecord);
            return  newModel;
        }

        public async Task<UpdateMemberViewModel?> GetMemberToUpdateAsync(int Id, CancellationToken ct = default)
        {
            
            var member = await _iUnitOfWork.GetRepository<Member>().GetByIdAsync(Id, ct);
            if(member is null)
            {

                return null;
            }
            return _mapper.Map<UpdateMemberViewModel>(member);
    }

        public async Task<bool> UpdateMemberAsync(int Id, UpdateMemberViewModel model, CancellationToken ct = default)
        {
            var member = await _iUnitOfWork.GetRepository<Member>().GetByIdAsync(Id, ct);
            if(member is null)
            {
                return false;
            }

            var emailExist = await _iUnitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != Id, ct);
            var phoneExist = await _iUnitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != Id, ct);
            if(emailExist || phoneExist) 
            {
                return false;
            }

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.Street = model.Street;
            member.Address.City = model.City;
            member.UpdatedAt = DateTime.Now;

            _iUnitOfWork.GetRepository<Member>().UpdateAsync(member);

            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0 ;
         }

        public async Task<bool> RemoveMemberAsync(int Id, CancellationToken ct = default)
        {
            var member = await _iUnitOfWork.GetRepository<Member>().GetByIdAsync(Id, ct);
            if(member is null)
                return false;

            var futureSessions = await _iUnitOfWork.GetRepository<Booking>().AnyAsync(m => m.MemberId == Id && m.Session.StartTime > DateTime.Now, ct);
            if (futureSessions)
                return false;
            _iUnitOfWork.GetRepository<Member>().DeleteAsync(member);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }
    }
}
