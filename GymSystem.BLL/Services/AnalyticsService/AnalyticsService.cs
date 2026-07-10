using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Services.AnalyticsService
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> GetTotalMembersAsync(CancellationToken ct)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(null!, ct: ct);
            return members.Count();
        }
        public async Task<int> GetActiveMembersAsync(CancellationToken ct)
        {
            var activeMembers =await _unitOfWork.GetRepository<Membership>().GetAllAsync(m => m.StartDate <= DateTime.Now && m.EndDate > DateTime.Now,ct:ct);
            return activeMembers.Count();
        }

        public async Task<int> GetCompletedSessionsAsync(CancellationToken ct)
        {
            var completedSessions = await _unitOfWork.SessionRepository.GetAllAsync(s => s.EndTime < DateTime.Now, ct: ct);
            return completedSessions.Count();
        }

        public async Task<int> GetOngoingSessionsAssync(CancellationToken ct)
        {
            var onGoingSessions = await _unitOfWork.SessionRepository.GetAllAsync(s=> s.StartTime <= DateTime.Now && s.EndTime > DateTime.Now, ct: ct);
            return onGoingSessions.Count();
        }

        public async Task<int> GetUpcomingSessionsAsync(CancellationToken ct)
        {
            var upComingSessions = await _unitOfWork.SessionRepository.GetAllAsync(s => s.StartTime >= DateTime.Now, ct: ct);
            return upComingSessions.Count();
        }


        public async Task<int> GetTotalTrainersAsync(CancellationToken ct)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(null!, ct: ct);
            return trainers.Count();
        }
    }
}
