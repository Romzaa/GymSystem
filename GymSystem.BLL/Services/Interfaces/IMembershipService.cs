using GymSystem.BLL.Helpers;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IMembershipService
    {
        Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default);
        Task<IEnumerable<MembersListViewModel>> GetMembersListAsync(CancellationToken ct = default);
        Task<IEnumerable<PlansListViewModel>> GetPlansListAsync(CancellationToken ct = default);
        Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default);
        Task<Result> DeleteActiveMembershipAsync(int Id, CancellationToken ct = default);
    }
}
