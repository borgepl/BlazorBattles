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
    public class UnitRepository : GenericRepository<Unit>, IUnitRepository
    {
        public UnitRepository(DataContext context) : base(context)
        {
            
        }
    }
}
