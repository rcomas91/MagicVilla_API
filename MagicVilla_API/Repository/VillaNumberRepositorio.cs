using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repository.IRepository;

namespace MagicVilla_API.Repository
{
    public class VillaNumberRepositorio : Repositorio<VillaNumber>, IVillaNumberRepositorio
    {

        private readonly ApplicationDbContext _db;

        public VillaNumberRepositorio(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }

        public async Task<VillaNumber> Update(VillaNumber entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _db.VillaNumbers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
