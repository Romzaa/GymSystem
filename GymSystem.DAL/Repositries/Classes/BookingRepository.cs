using GymSystem.DAL.Models;
using GymSystem.DAL.Repositries.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymSystem.DAL.Repositries.Classes
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly GymDbContext _dbContext;

        public BookingRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsWithMembersAndSessions(Expression<Func<Booking, bool>> filter, CancellationToken ct = default)
        {
            IQueryable<Booking> bookings = _dbContext.Bookings.Include(b => b.Member).Include(b => b.Session);
            if (filter is not null)
            {
                bookings = bookings.Where(filter);
            }

            return await bookings.ToListAsync(ct);
        }

        public async Task<Booking?> GetBookingWithMemberAndSession(int Id, CancellationToken ct = default)
        {
            var booking = await _dbContext.Bookings.Include(b => b.Member).Include(b => b.Session).FirstOrDefaultAsync(b =>  b.SessionId == Id, ct);
            if(booking is null)
            {
                return null;
            }

            return booking;
        }
    
    }
}
