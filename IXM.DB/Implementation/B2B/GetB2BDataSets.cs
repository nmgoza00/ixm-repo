using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using IXM.Common;
using IXM.Common.Generics;
using IXM.GeneralSQL;
using IXM.Ingest;
using IXM.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static IXM.Common.Generics.Generics4API;


namespace IXM.DB.B2B
{

    public class GetB2BDataSets : IGetB2BDataSets
    {

        private IXMDBContext _context;
        private IXMDBIdentity _identitycontext;
        private readonly ILogger<FileIngest> _logger;

        public GetB2BDataSets(IXMDBContext context, 
                              IXMDBIdentity identitycontext, 
                              ILogger<FileIngest> logger)
        {
            _context = context;
            _identitycontext = identitycontext;
            _logger = logger;
        }


        public async Task<PageList<TRMBLD>> RemittanceDetailHandle(IXM.Models.Core.Remittance request)
        {
            IQueryable<TRMBLD> query = _context.TRMBLD.Where(a => a.RMBLID == request.RMBLID);
            ///To apply any additional filtering
            query = query.OrderBy(a => a.DTF_IDNUMBER);

            var result = await Generics4API.PageList<TRMBLD>.CreateAsync(query,request.PAGENO, request.PAGESIZE);

            return result;

        }
    }

}
