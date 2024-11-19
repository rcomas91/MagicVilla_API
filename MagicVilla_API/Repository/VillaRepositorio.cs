using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repository.IRepository;

namespace MagicVilla_API.Repository
{
    public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
    {

        private readonly ApplicationDbContext _db;

        public VillaRepositorio(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }

        public async Task<Villa> Update(Villa entity)
        {
            entity.UpdateAt = DateTime.Now;
            _db.Villas.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
