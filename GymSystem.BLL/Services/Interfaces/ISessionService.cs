using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        public Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
        public Task<SessionViewModel?> GetSessionDetailsAsync(int Id, CancellationToken ct = default);
        Task<bool> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);
        Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int Id, CancellationToken ct = default);
        Task<bool> UpdateSessionAsync(int Id, UpdateSessionViewModel model, CancellationToken ct = default);
        Task<bool> RemoveSessionAsync(int Id, CancellationToken ct = default);
        public Task<IEnumerable<Trainer>> GetAllTrainers(CancellationToken ct = default);

    }
}
