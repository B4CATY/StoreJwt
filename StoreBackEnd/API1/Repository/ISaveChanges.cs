using System.Threading.Tasks;

namespace API1.Repository
{
    public interface ISaveChanges
    {
        Task<int> SaveChanges();
    }
}
