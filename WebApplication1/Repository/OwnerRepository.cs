﻿using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Owner> GetOwnersOfAPokemon(int pokeId)
        {
            return _context.PokemonOwners.Where(o => o.Pokemon.Id == pokeId).Select(o => o.Owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonsByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(o => o.Owner.Id == ownerId).Select(o => o.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(o => o.Id == ownerId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }
    }
}
