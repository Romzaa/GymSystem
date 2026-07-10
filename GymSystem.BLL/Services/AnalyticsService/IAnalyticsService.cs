using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Services.AnalyticsService
{
    public interface IAnalyticsService
    {
        public Task<int> GetTotalMembersAsync( CancellationToken ct);
        public Task<int> GetActiveMembersAsync(CancellationToken ct);
        public Task<int> GetTotalTrainersAsync(CancellationToken ct);
        public Task<int> GetUpcomingSessionsAsync(CancellationToken ct);
        public Task<int> GetOngoingSessionsAssync(CancellationToken ct);
        public Task<int> GetCompletedSessionsAsync(CancellationToken ct);


    }
}
