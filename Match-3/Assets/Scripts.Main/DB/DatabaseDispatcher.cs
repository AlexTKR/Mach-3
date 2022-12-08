using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scripts.Main.Controllers;
using Zenject;

namespace Scripts.Main.DB
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
        private IDatabaseUser[] _databaseUsers;
        private IDatabase _database;

        [Inject]
        void Construct(IList<IDatabaseUser> databaseUsers, IDatabase database)
        {
            _databaseUsers = databaseUsers.ToArray();
            _database = database;
        }

        public async Task Dispatch(IDatabase dataBase)
        {
            for(int i = 0; i <  _databaseUsers.Length; i++)
            {
                await _databaseUsers[i].InjectDatabase(_database);
            }
        }
    }
}