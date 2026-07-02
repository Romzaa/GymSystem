using AutoMapper;
using GymSystem.BLL.Helpers;
using GymSystem.BLL.Services.AttachementService;
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
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork IUnitOfWork, IMapper mapper, IAttachmentService attachmentService)
        {
            _iUnitOfWork = IUnitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        public async Task<Result> CreateMemberAsync(CreateMemberViewModel viewModel, CancellationToken ct = default)
        {
            var memberRepo =  _iUnitOfWork.GetRepository<Member>();

            var emailExists =await memberRepo.AnyAsync(m => m.Email == viewModel.Email, ct);
            var phoneExists =await memberRepo.AnyAsync(m => m.Phone == viewModel.Phone, ct);
            if (emailExists )
                return Result.Validation("This Email Already Exists");
            if(phoneExists)
                return Result.Validation("This Phone Number Already Exists");

            var photo = await _attachmentService.UploadAsync(viewModel.PhotoFile.OpenReadStream(), viewModel.PhotoFile.FileName, "MembersPictures", ct);
            if (string.IsNullOrEmpty(photo)) return Result.Fail("Failed To Upload Member Profile Photo");

            var member = _mapper.Map<Member>(viewModel);
            member.Photo = photo;
             memberRepo.AddAsync(member);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            if (result == 0)
            {
                _attachmentService.Delete(member.Photo, "MembersPictures");
                return Result.Fail("Failed To Create New Member");
            }
            else return Result.OK();

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

        public async Task<Result> UpdateMemberAsync(int Id, UpdateMemberViewModel model, CancellationToken ct = default)
        {
            var member = await _iUnitOfWork.GetRepository<Member>().GetByIdAsync(Id, ct);
            if(member is null)
            {
                return Result.NotFound("Member Is Not Found");
            }

            var emailExist = await _iUnitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != Id, ct);
            var phoneExist = await _iUnitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != Id, ct);
            if(emailExist )
                return Result.Validation("This Email Already Exists");
            if ( phoneExist)
                return Result.Validation("This Phone Number Already Exists");


            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.Street = model.Street;
            member.Address.City = model.City;
            member.UpdatedAt = DateTime.Now;

            _iUnitOfWork.GetRepository<Member>().UpdateAsync(member);

            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed To Update Member");
         }

        public async Task<Result> RemoveMemberAsync(int Id, CancellationToken ct = default)
        {
            var member = await _iUnitOfWork.GetRepository<Member>().GetByIdAsync(Id, ct);
            if(member is null)
                return Result.NotFound("Member Is Not Found");

            var futureSessions = await _iUnitOfWork.GetRepository<Booking>().AnyAsync(m => m.MemberId == Id && m.Session.StartTime > DateTime.Now, ct);
            if (futureSessions)
                return Result.Validation("Can't Delete Member Has A Future Session");
            _iUnitOfWork.GetRepository<Member>().DeleteAsync(member);
            var result = await _iUnitOfWork.SaveChangesAsync(ct);
            if (result > 0)
            {
                if(!string.IsNullOrEmpty(member.Photo))
                    _attachmentService.Delete(member.Photo, "MembersPictures");
                     return Result.OK();
            }
            else return Result.Fail("Failed To Remove Member");

        }
    }
}
