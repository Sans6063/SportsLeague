using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class MatchLineupService : IMatchLineupService
    {
        private readonly IMatchLineupRepository _lineupRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPlayerRepository _playerRepository;

        public MatchLineupService(
            IMatchLineupRepository lineupRepository,
            IMatchRepository matchRepository,
            IPlayerRepository playerRepository)
        {
            _lineupRepository = lineupRepository;
            _matchRepository = matchRepository;
            _playerRepository = playerRepository;
        }

        public async Task<MatchLineup> AddPlayerToLineupAsync(int matchId, MatchLineup lineup)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);

            // Validacion 1
            if (match == null)
                throw new KeyNotFoundException(
                    $"No se encontró el partido con ID {matchId}");

            var player = await _playerRepository.GetByIdAsync(lineup.PlayerId);

            // Validacion 2
            if (player == null)
                throw new KeyNotFoundException(
                    $"No se encontró el jugador con ID {lineup.PlayerId}");

            // Validacion6
            if (match.Status != MatchStatus.Scheduled)
            {
                throw new InvalidOperationException(
                    "Solo se pueden registrar alineaciones en partidos Scheduled");
            }

            // Validacion 3
            if (player.TeamId != match.HomeTeamId &&
                player.TeamId != match.AwayTeamId)
            {
                throw new InvalidOperationException(
                    "El jugador no pertenece a ninguno de los equipos del partido");
            }

            // Validacion 4
            var exists = await _lineupRepository
                .ExistsByMatchAndPlayerAsync(matchId, lineup.PlayerId);

            if (exists)
            {
                throw new InvalidOperationException(
                    "El jugador ya está registrado en la alineación de este partido");
            }

            // Validacion 5
            if (lineup.IsStarter)
            {
                var teamLineup = await _lineupRepository
                    .GetByMatchAndTeamAsync(matchId, player.TeamId);

                var startersCount = teamLineup.Count(x => x.IsStarter);

                if (startersCount >= 11)
                {
                    throw new InvalidOperationException(
                        "El equipo ya tiene 11 titulares registrados en este partido");
                }
            }

            lineup.MatchId = matchId;

            return await _lineupRepository.CreateAsync(lineup);
        }

        public async Task<IEnumerable<MatchLineup>> GetMatchLineupAsync(int matchId)
        {
            return await _lineupRepository
                .GetByMatchWithDetailsAsync(matchId);
        }

        public async Task<IEnumerable<MatchLineup>> GetTeamLineupAsync(
            int matchId,
            int teamId)
        {
            return await _lineupRepository
                .GetByMatchAndTeamAsync(matchId, teamId);
        }

        public async Task DeletePlayerFromLineupAsync(
            int matchId,
            int lineupId)
        {
            var lineup = await _lineupRepository.GetByIdAsync(lineupId);

            if (lineup == null || lineup.MatchId != matchId)
            {
                throw new KeyNotFoundException(
                    "No se encontró el registro de alineación");
            }

            await _lineupRepository.DeleteAsync(lineupId);
        }
    }
}
