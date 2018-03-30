using BL.Interfaces;
using DAL;
using DAL.Repositories_EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Managers
{
    public class PlatformManager : IPlatformManager
    {
        IPlatformRepository platformRepository;

        public PlatformManager()
        {
            platformRepository = new PlatformRepository_EF();
        }
    }
}
