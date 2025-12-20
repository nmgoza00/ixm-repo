using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IXM.DB
{
    public class IXMDBContextFactory
    {
        private readonly IConfiguration _config;

        public IXMDBContextFactory(IConfiguration config)
        {
            _config = config;
        }

        public IXMDBContext Create(string connectionName)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IXMDBContext>();
            var connStr = _config.GetConnectionString(connectionName);
            optionsBuilder.UseFirebird(connStr);
            return new IXMDBContext(optionsBuilder.Options);
        }
    }
    public class IXMWriteDBContextFactory
    {
        private readonly IConfiguration _config;

        public IXMWriteDBContextFactory(IConfiguration config)
        {
            _config = config;
        }

        public IXMWriteDBContext Create(string connectionName)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IXMWriteDBContext>();
            var connStr = _config.GetConnectionString(connectionName);
            optionsBuilder.UseFirebird(connStr);
            return new IXMWriteDBContext(_config, optionsBuilder.Options);
        }
    }


}
