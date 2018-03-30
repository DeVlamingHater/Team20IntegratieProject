using DAL.EF;

namespace DAL.Repositories_EF
{
    public class UnitOfWork
    {
        private PolitiekeBarometerContext context;

        internal    PolitiekeBarometerContext Context
        {
            get
            {
                if (context == null)
                {
                    context = new PolitiekeBarometerContext(true);
                }
                return context;
            }
        }

        public void CommitChanges()
        {
            context.CommitChanges();
        }
    }
}