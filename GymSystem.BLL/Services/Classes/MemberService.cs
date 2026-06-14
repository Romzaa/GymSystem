using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepo;
        public MemberService(IGenericRepository<Member> memberRepo)
        {
            _memberRepo = memberRepo;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel viewModel, CancellationToken ct = default)
        {
            var emailExists =await _memberRepo.AnyAsync(m => m.Email == viewModel.Email, ct);
            var phoneExists =await _memberRepo.AnyAsync(m => m.Phone == viewModel.Phone, ct);
            if (emailExists || phoneExists)
                return false;
            var member = new Member()
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Address = new Address
                {
                    BuildingNumber = viewModel.BuildingNumber,
                    Street = viewModel.Street,
                    City = viewModel.City,
                },
                DateOfBirth = viewModel.DateOfBirth,
                Gender = viewModel.Gender,
                HealthRecord = new HealthRecord()
                {
                    BloodType = viewModel.HealthRecordViewModel.BloodType,
                    Height = viewModel.HealthRecordViewModel.Height,
                    Weight = viewModel.HealthRecordViewModel.Weight,
                    Note = viewModel.HealthRecordViewModel.Note
                }

            };
            var result = await _memberRepo.AddAsync(member, ct);
            return result > 0;
            
            }
        

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberRepo.GetAllAsync(ct: ct);
            if (!members.Any())
                return [];
            return members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo! ,
                Gender=m.Gender
            });
        
        }


       
}
}
