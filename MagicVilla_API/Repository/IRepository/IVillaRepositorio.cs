using MagicVilla_API.Models;

namespace MagicVilla_API.Repository.IRepository
{
    public interface IVillaRepositorio:IRepository<Villa>
    {
       Task<Villa> Update(Villa entity);
    }
}
