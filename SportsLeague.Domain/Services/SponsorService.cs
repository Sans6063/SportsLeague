using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepository;

        public SponsorService(ISponsorRepository sponsorRepository)
        {
            _sponsorRepository = sponsorRepository;
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            return await _sponsorRepository.GetAllAsync();
        }
        public async Task<Sponsor> GetByIdAsync(int id)
        {
            var sponsor = await _sponsorRepository.GetByIdAsync(id);
            if (sponsor == null)
                throw new KeyNotFoundException("No se encontro el Sponsor");
            return sponsor;
        }
        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            //Verificar si el nombre ya existe
            if (await _sponsorRepository.ExistsByNameAsync(Sponsor.name))
                throw new InvalidOperationException("El nombre de este Sponsor ya existe");

            // Verificar si es un email valido
            if (!IsValidEmail(Sponsor.ContactEmail))
                throw new InvalidOperationException("Email invalido");

            sponsor.CreateAt = DateTime.UtcNow;

            return await _sponsorRepository.CreateAsync(sponsor);
        }
        public async Task UpdateAsync(int id, Sponsor sponsor)
        {
            var existing = await _sponsorRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException("Sponsor no encontrado");
            //Verificar si el nombre ya existe
            if(existing.Name != Sponsor.name && await _sponsorRepository.ExistsByNameAsync(Sponsor.name))
                throw new InvalidOperationException("El nombre de este Sponsor ya existe");
            if (!IsValidEmail(sponsor.Contactemail))
                throw new InvalidOperationException("Email invalido");

            existing.Name = sponsor.Name;
            existing.Contactemail = sponsor.Contactemail;
            existing.Phone = sponsor.Phone;
            existing.Websiteurl = sponsor.Websiteurl;
            existing.Category = sponsor.Category;
            existing.UpdateAt = DateTime.UtcNow;

            await _sponsorRepository.UpdateAsync(existing);
        }
        public async Task DeleteAsync(int id)
        { 
            var sponsor = await _sponsorRepository.GetByIdAsync(id);
            if (sponsor == null)
                throw new KeyNotFoundException("Sponsor no encontrado");
            await _sponsorRepository.DeleteAsync(id);
        }

        private bool IsValidEmail(string Email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(Email);
                return addr.Address == Email;
            }
            catch
            {
                return false;
            }
        }
    }
}
