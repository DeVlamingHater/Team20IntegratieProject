using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories_EF
{
    public class PlatformRepository_EF : IPlatformRepository
    {
        PolitiekeBarometerContext context;

        public PlatformRepository_EF()
        {
            context = new PolitiekeBarometerContext();
        }
        public PlatformRepository_EF(UnitOfWork unitOfWork)
        {
            context = unitOfWork.Context;
        }
    }
}
