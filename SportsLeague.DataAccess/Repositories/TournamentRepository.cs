using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentRepository : GenericRepository<Tournament>, ITournamentRepository
    {
        public TournamentRepository(LeagueDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Tournament>> GetByStatusAsync(TournamentStatus status)
        { 
            return await _dbset
                .Where(t => t.Status == status)
                .ToListAsync();
        }
        public async Task<Tournament?> GetByIdWithTeamAsync(int id)
        {
            return await _dbset
                .Where(t => t.Id == id)
                .Include(t => t.TournamentTeams)
                    .ThenInclude(tt => tt.Team)
                .FirstOrDefaultAsync();
        }
    }
}
