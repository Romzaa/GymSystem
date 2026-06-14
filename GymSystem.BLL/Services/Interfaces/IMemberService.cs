using GymSystem.BLL.ViewModels.MemberViewModels;
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

    }
}
