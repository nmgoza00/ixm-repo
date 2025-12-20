using System.Text;
using IXM.Common;
using System.Text.Json;
using IXM.DB;

namespace IXM.API.Repositories
{
    public abstract class GenericCUP<TEntity> where TEntity : class
    {

        protected readonly IXMDBContext _context;

        protected GenericCUP(IXMDBContext context)
        {
            _context = context;

        }

        public void Add(TEntity entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

    }


}
