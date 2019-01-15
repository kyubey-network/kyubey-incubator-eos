using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Andoromeda.Kyubey.Incubator.Repository
{
    public interface IRepository<T>
    {
        IEnumerable<T> EnumerateAll();

        T GetSingle(object id);
    }

    public interface IRepositoryFactory<T>
    {
        IRepository<T> Create(string lang);
        Task<IRepository<T>> CreateAsync(string lang);
    }
}
