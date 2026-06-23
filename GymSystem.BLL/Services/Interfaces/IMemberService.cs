using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL;
using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        public Task<IEnumerable<MemberViewModel>> GetAllMembersAsync( CancellationToken ct = default );

        public Task <bool> CreateMemberAsync(CreateMemberViewModel viewModel, CancellationToken ct = default);

        public int GetTotalMembersAsync(GymDbContext gdbcontext, CancellationToken ct = default);
        public Task<int> GetActiveMembersAsync(GymDbContext gdbcontext, CancellationToken ct = default);

        public Task<MemberViewModel?> GetMemberDetailsAsync(int Id, CancellationToken ct = default);

        public Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int Id, CancellationToken ct = default);

        Task<UpdateMemberViewModel?> GetMemberToUpdateAsync(int Id, CancellationToken ct = default);
        Task<bool> UpdateMemberAsync(int Id, UpdateMemberViewModel model , CancellationToken ct = default);
        Task<bool> RemoveMemberAsync(int Id, CancellationToken ct = default);
    }
}
