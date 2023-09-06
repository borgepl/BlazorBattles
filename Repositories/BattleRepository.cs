using DataAccess.Data;
using DataAccess.Data.Domain;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BattleRepository : GenericRepository<Battle>, IBattleRepository
    {
        public BattleRepository(DataContext context) : base(context)
        {
            
        }
    }
}
