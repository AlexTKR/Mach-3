using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controllers;
using Zenject;

namespace DB
{
    public interface IDatabaseDispatcher
    {
        Task Dispatch(IDatabase dataBase);
    }

    public interface IDatabaseUser
    {
        Task InjectDatabase(IDatabase database);
    }

    public class DatabaseDispatcher : IDatabaseDispatcher
    {
        private List<IDatabaseUser> _databaseUsers;
        private IDatabase _database;

        [Inject]
        void Construct(IList<IDatabaseUser> databaseUsers, IDatabase database)
        {
            _databaseUsers = databaseUsers.ToList();
            _database = database;
        }

        public async Task Dispatch(IDatabase dataBase)
        {
            for(int i = 0; i <  _databaseUsers.Count; i++)
            {
                await _databaseUsers[i].InjectDatabase(_database);
            }
        }
    }
}