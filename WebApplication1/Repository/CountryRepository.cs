using AutoMapper;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CountryRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public bool CountryExists(int countryId)
        {
            return _context.Countries.Any(c => c.Id == countryId);
        }

        public bool CreateCountry(Country country)
        {
           _context.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _context.Remove(country);
            return Save();
        }

        public Country GetCounrtyByOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).Select(o => o.Country).FirstOrDefault();
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountry(int countryId)
        {
            return _context.Countries.Where(c => c.Id == countryId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromACountry(int countryId)
        {
            return _context.Owners.Where(o => o.Country.Id == countryId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
           _context.Update(country);
            return Save();
        }
    }
}
