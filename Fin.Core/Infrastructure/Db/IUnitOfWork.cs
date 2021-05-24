using System.Threading.Tasks;

namespace Fin.Core.Infrastructure.Db
{
    public interface IUnitOfWork
    {
        Task<int> Save();
    }
}
