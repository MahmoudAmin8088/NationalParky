using Microsoft.EntityFrameworkCore;
using NationalParky.Data;
using NationalParky.Models;
using NationalParky.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NationalParky.Repository
{
    public class TrailRepo : ITrailRepo
    {
        private readonly ApplicationDbContext _db;

        public TrailRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            return _db.Trails.Include(t => t.NationalPark).FirstOrDefault(n => n.Id == trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(t => t.NationalPark).OrderBy(t => t.Name).ToList();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npId)
        {
            return _db.Trails.Include(t => t.NationalPark).Where(n => n.nationalParkId == npId).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }

        public bool TrailExists(string name)
        {
            bool value = _db.Trails.Any(n => n.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(n => n.Id == id);
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }
    }
}
