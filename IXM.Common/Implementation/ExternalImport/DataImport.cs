using ClosedXML.Excel;
using IXM.Models;
using IXM.Constants;
using IXM.Models.Core;
using Microsoft.Extensions.Logging;

namespace IXM.Common
{
    public class DataImport : IDataImport
    {

        private readonly ILogger<DataImport> _logger;

        private GenValues _ixmcommon;

        public List<DATA_VALUEMAPPING> DataValueMap { get; set; }
        public List<MCOMPANY> Branches { get; set; }
        public List<MCITY> Location { get; set; }
        public DataImport(ILogger<DataImport> logger)
        {
            _logger = logger;
        }
        public List<T>? IxmExcelImporter<T>(string FilePathAndName, string SheetName )
        {
            try
            {
                List<T> list = new List<T>();
                Type type = typeof(T);
                SheetName = "Sheet1";
                string lContField = "DTF_DBCONTRIBUTION";
                using (IXLWorkbook workbook = new XLWorkbook(FilePathAndName))
                {
                    var worksheet = workbook.Worksheets.Where(ws => ws.Name == SheetName).First();
                    var properties = type.GetProperties();
                    var columns = worksheet.Row(20).Cells().Select((v, i) => new { Value = "DTF_" + v.Value, Index = i + 2 }).Where(a => a.Value != null).ToList();

                    _logger.LogInformation("Excel Columns : {Message}", columns);
                    _logger.LogInformation("Excel Stats : Start Row {@1} - Rows {@2} - Used {@3}", CommonConstants.ScheduleRowStart, worksheet.RowCount(), worksheet.RowsUsed(XLCellsUsedOptions.All).Count());
                    ;
                    foreach (var prop in properties)
                    {
                        //_logger.LogInformation("Structure Columns : {Message}", prop);
                        var col = columns.SingleOrDefault(x => x.Value.ToString() == prop.Name.ToString());
                    }




                    foreach (IXLRow row in worksheet.RowsUsed(XLCellsUsedOptions.All).Skip(CommonConstants.ScheduleRowStart))
                    {
                        T obj = (T)Activator.CreateInstance(type);
                        foreach (var prop in properties)
                        {
                            var col = columns.SingleOrDefault(x => x.Value.ToString() == prop.Name.ToString());
                            int colindex = 0;
                            try
                            {
                                if (col != null)
                                {
                                    colindex = columns.SingleOrDefault(x => x.Value.ToString() == prop.Name.ToString()).Index;
                                    var val = row.Cell(colindex).Value;
                                    var type2 = prop.PropertyType;
                                    var tval = Convert.ChangeType(val.ToString(), type2);
                                    prop.SetValue(obj, tval );

                                } else if (prop.Name.ToString() == "DTF_ICONTRIBUTION")
                                {

                                    colindex = columns.SingleOrDefault(x => x.Value.ToString() == lContField).Index;
                                    var val = row.Cell(colindex).Value;
                                    var type2 = prop.PropertyType;
                                    var tval = Convert.ChangeType(val.ToString(), type2);
                                    prop.SetValue(obj, tval);

                                }

                            }
                            catch (Exception e)
                            {
                                _logger.LogError("Excel Importer :: Data Issue - Column {@Column}, Type {@Type} - Error {@Error}", col.Value, prop.PropertyType, e.Message);

                            }

                        }
                       
                        list.Add(obj);
                        //_logger.LogInformation("Importer Column Record : {@1}", list.LastOrDefault());


                    }                    

                }

                _logger.LogInformation("Importer Column Record Last : {@1}", list.LastOrDefault());
                return list;

            }
            catch (Exception e)
            {
                _logger.LogError("Error encountered processing Excel file {@Filename}. Error {@Message} ", FilePathAndName, e.Message);
                return null;
            }
        }

        public int IxmExcelExportDataIssues<T>(List<T> DataTable, List<TCAPTIONS> Captions, string FilePathAndName)
        {
            try
            {


                using (XLWorkbook workbook = new XLWorkbook())
                {
                    var ws = workbook.AddWorksheet().FirstCell().InsertTable<T>(DataTable, false);
                    int cc = ws.ColumnCount();
                    try
                    {
                        for (int i = 1; i < cc + 1; i++)
                        {
                            string ColValue = ws.Cell(1, i).Value.ToString();
                            var Caption = Captions.Where(a => a.FIELD_NAME == ColValue).ToList();
                            if (Caption.Count() > 0)
                            {
                                ws.Cell(1, i).Value = Caption.Single().FIELD_CAPTION;
                            }
                            else
                            {
                                ws.Column(i).Delete();
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        _logger.LogInformation("Error encountered processing row in Excel file {@Filename}. Error {@Message} ", FilePathAndName, e.Message);
                    }


                    //FilePathAndName = FilePathAndName.Replace(".xlsx", "_errors.xlsx");
                    workbook.SaveAs(FilePathAndName);                    

                };

                return 1;

            }
            catch (Exception e)
            {
                _logger.LogError("Error encountered processing Excel file {@Filename}. Error {@Message} ", FilePathAndName, e.Message);
                return 0;
            }
        }

        public int IxmExcelValidateImport(ref List<XLS_REMITTANCE> DataTable)
        {
            //Type type = typeof(XLS_REMITTANCE);
            //var properties = type.GetProperties();

            _ixmcommon = new GenValues();
            string col = "";

            var toVerify = new[] { "DTF_CITYID", "DTF_CCOMPANYID", "DTF_BCOMPANYID", "DTF_DBCONTRIBUTION", "DTF_IDNUMBER", "DTF_IDTYPE", "DTF_GENDER", "DTF_MNAME", "DTF_MSURNAME", "DTF_CELLNUMBER", "DTF_EMPNUMBER" };

            foreach (var row in DataTable)
            {
                try
                {
                    foreach (var pname in toVerify)
                    {
                        col = pname;
                        var nrow = SetRowVal(col, row);
                        if (nrow != null)
                        {
                            //row.GetType().GetProperty(col).SetValue(row,nrow, null);
                            //CopyModelValues(nrow, row);
                            //row = nrow;
                        }
                        //if (getval != null)
                        //{
                        //}

                    }

                }
                catch (Exception e)
                {
                    _logger.LogError("Error : {@1}  {@2}", col, e.Message);
                    return 99;
                }

            }

            return DataTable.Where(a => a.ERRORNUM > 0).Count();
        }

        public async Task<MemoryStream> IxmExportedRemittanceInfo(string FilePathAndName, string SheetName, string PeriodName, MCOMPANY DataInfo)
        {
           
            Type type = typeof(MCOMPANY);
            SheetName = "Sheet1";
            IXLWorkbook workbook = new XLWorkbook(FilePathAndName);

            var worksheet = workbook.Worksheets.Where(ws => ws.Name == SheetName).First();
            var properties = type.GetProperties();
            
            worksheet.Cell(5, 3).Value = DataInfo.COMPANYNUM;
            worksheet.Cell(6, 3).Value = PeriodName;

            worksheet.Cell(4, 10).Value = DataInfo.CNAME;
            worksheet.Cell(5, 10).Value = DataInfo.REG_NUM;
            worksheet.Cell(6, 10).Value = DataInfo.STREETNO.ToString() + " " + DataInfo.STREETNAME;
            worksheet.Cell(7, 10).Value = DataInfo.BUILDINGNAME;
            worksheet.Cell(9, 10).Value = DataInfo.POSTALCODE;
            worksheet.Cell(10, 10).Value = DataInfo.CONTACT_PERSON;
            worksheet.Cell(11, 10).Value = DataInfo.TEL_NUMBER;
            worksheet.Cell(12, 10).Value = DataInfo.E_MAIL;                
            worksheet.Cell(13, 10).Value = DataInfo.FAX_NUMBER;

            using (var msB = new MemoryStream())
            {
                workbook.SaveAs(msB);
                MemoryStream stream = new MemoryStream(msB.GetBuffer(), 0, (int)msB.Length);
                return stream;
            }
        }

        /*public async Task<MemoryStream> IxmExportDataToSheet(string FilePathAndName, string SheetName, string PeriodName, DataTable DataInfo)
        {

            Type type = typeof(MCOMPANY);
            SheetName = "Sheet1";
            IXLWorkbook workbook = new XLWorkbook(FilePathAndName);

            var worksheet = workbook.Worksheets.Where(ws => ws.Name == SheetName).First();
            var properties = type.GetProperties();

            worksheet.Cell(5, 3).Value = DataInfo.COMPANYNUM;
            worksheet.Cell(6, 3).Value = PeriodName;

            worksheet.Cell(4, 10).Value = DataInfo.CNAME;
            worksheet.Cell(5, 10).Value = DataInfo.REG_NUM;
            worksheet.Cell(6, 10).Value = DataInfo.STREETNO.ToString() + " " + DataInfo.STREETNAME;
            worksheet.Cell(7, 10).Value = DataInfo.BUILDINGNAME;
            worksheet.Cell(9, 10).Value = DataInfo.POSTALCODE;
            worksheet.Cell(10, 10).Value = DataInfo.CONTACT_PERSON;
            worksheet.Cell(11, 10).Value = DataInfo.TEL_NUMBER;
            worksheet.Cell(12, 10).Value = DataInfo.E_MAIL;
            worksheet.Cell(13, 10).Value = DataInfo.FAX_NUMBER;

            using (var msB = new MemoryStream())
            {
                workbook.SaveAs(msB);
                MemoryStream stream = new MemoryStream(msB.GetBuffer(), 0, (int)msB.Length);
                return stream;
            }
        }*/




        public XLS_REMITTANCE SetRowVal(string Columnname, XLS_REMITTANCE Datarow)
        {
            try
            {

                var val = Datarow.GetType().GetProperty(Columnname);
                if (val != null)
                {
                    var cvalue = val.GetValue(Datarow, null);

                    if ((Columnname == "DTF_CCOMPANYID") && (cvalue != null))
                    {
                        //var lval2 = DataValueMap.Where(a => a.DHCODE == "CCOMPANYID").ToList();
                        var lval = DataValueMap.Where(a => a.DHCODE == "CCOMPANYID")
                                    .Where(b => b.SVALUE.ToLower() == cvalue.ToString().ToLower()).SingleOrDefault();

                        if (lval != null)
                        {
                            cvalue = lval.TKEY.ToString();
                            var val2 = Datarow.GetType().GetProperty("COMPANYID");
                            val2.SetValue(Datarow, Convert.ToInt32(cvalue));

                        }
                        else return null;

                    }

                    if ((Columnname == "DTF_BCOMPANYID") && (cvalue != null))
                    {

                        //var lval2 = DataValueMap.Where(a => a.DHCODE == "CCOMPANYID").ToList();
                        var lval1 = Branches.Where(a => a.COMPANYNUM.ToLower() == cvalue.ToString().ToLower()).SingleOrDefault();
                        var val2 = Datarow.GetType().GetProperty("TRL_BCOMPANYID");

                        if (lval1 != null)
                        {
                            cvalue = lval1.COMPANYID.ToString();
                            val2.SetValue(Datarow, Convert.ToInt32(cvalue));

                        }

                        //var lval2 = DataValueMap.Where(a => a.DHCODE == "CCOMPANYID").ToList();
                        var lval2 = DataValueMap.Where(a => a.DHCODE == "BCOMPANYID")
                                    .Where(b => b.SVALUE.ToLower() == cvalue.ToString().ToLower()).SingleOrDefault();

                        if (lval2 != null)
                        {
                            cvalue = lval2.TKEY.ToString();
                            //var val2 = Datarow.GetType().GetProperty("TRL_BCOMPANYID");
                            val2.SetValue(Datarow, Convert.ToInt32(cvalue));

                        }


                        // Try and apply Location based on Branch allocation
                        var fval = val2.GetValue(Datarow, null);
                        //var lval2 = DataValueMap.Where(a => a.DHCODE == "CCOMPANYID").ToList();

                        if (fval !=null)
                        {
                            var lval3 = Branches.Where(a => a.COMPANYID == Convert.ToInt32(fval)).SingleOrDefault();

                            if (lval3 != null)
                            {
                                cvalue = lval3.BCITYID.ToString();
                                var val3 = Datarow.GetType().GetProperty("TRL_CITYID");
                                val3.SetValue(Datarow, cvalue);
                                return Datarow;

                            }

                        }
                        else return null;


                    }


                    if (((Columnname == "DTF_DBCONTRIBUTION") || (Columnname == "DTF_ICONTRIBUTION")) && (cvalue != null))
                    {

                        string col2 = "TRL_IAMOUNT";
                        var lreturn1 = _ixmcommon.CheckValueForDecimal(cvalue.ToString());
                        if (lreturn1.Item1 == false)
                        {

                             Datarow.ERRORNUM = 1;
                             Datarow.ERRORDESCRIPTION = "Invalid Amount Value for Contribution.";
                             return null;
                        }
                        var val2 = Datarow.GetType().GetProperty(col2);
                        val2.SetValue(Datarow, lreturn1.Item3);
                        return Datarow;


                    }
                    else if ((Columnname == "DTF_SALARY") && (cvalue != null))
                    {
                        string col2 = "TRL_SALARY";
                        var lreturn1 = _ixmcommon.CheckValueForDecimal(cvalue.ToString());
                        if (lreturn1.Item1 == true)
                        {
                            var val2 = Datarow.GetType().GetProperty(col2);
                            val2.SetValue(Datarow, lreturn1.Item3);
                            return Datarow;

                        }
                        else return null;

                    }
                    else if ((Columnname == "DTF_CITYID") && (cvalue != null))
                    {
                        var val2 = Datarow.GetType().GetProperty("TRL_CITYID");
                        var lval1 = Location.Where(a => a.DESCRIPTION.ToLower() == cvalue.ToString().ToLower()).SingleOrDefault();

                        if (lval1 != null)
                        {
                            cvalue = lval1.CITYID.ToString();
                            val2.SetValue(Datarow, cvalue.ToString());
                            return Datarow;

                        }
                        
                        var lval2 = DataValueMap.Where(a => a.DHCODE == "CITYID")
                                        .Where(b => b.SVALUE.ToLower() == cvalue.ToString().ToLower()).SingleOrDefault();

                        if (lval2 != null)
                        {
                              cvalue = lval2.TKEY.ToString();
                              val2.SetValue(Datarow, Convert.ToInt32(cvalue));
                              return Datarow;

                        }

                        var lval3 = Location.Where(a => a.CITYID.ToLower() == cvalue.ToString().ToLower()).SingleOrDefault();

                        if (lval1 != null)
                        {
                            cvalue = lval3.CITYID.ToString();
                            val2.SetValue(Datarow, cvalue.ToString());
                            return Datarow;

                        }
                        else return null;

                    }
                    else if ((Columnname == "DTF_IDTYPE") && (cvalue != null))
                    {
                        string col2 = "TRL_IDTYPEID";
                        var val2 = Datarow.GetType().GetProperty(col2);
                        if (cvalue.ToString().ToLower() == "id number")
                        {
                            val2.SetValue(Datarow, "ID");
                            return Datarow;

                        }
                        else if (cvalue.ToString().ToLower() == "passport")
                        {
                            val2.SetValue(Datarow, "PA");
                            return Datarow;
                        }
                        else
                        {
                            val2.SetValue(Datarow, "CU");
                            return Datarow;
                        }

                    }
                    else if ((Columnname == "DTF_IDNUMBER") && (cvalue != null))
                    {

                        var val2 = Datarow.GetType().GetProperty("DTF_IDTYPE");
                        var ls1 = val2.GetValue(Datarow, null);
                        if (ls1 == null)
                        {
                            Datarow.ERRORNUM = 1;
                            Datarow.ERRORDESCRIPTION = "ID Type is not specified. Kindly rectify.";
                            return null;
                        }


                        //var val3 = Datarow.GetType().GetProperty(Columnname);
                        //val3.SetValue(Datarow, lreturn1.Item3);

                        //if (ls1.ToString() == "ID Number")
                        //{

                            if (String.IsNullOrEmpty(cvalue.ToString()) )
                            {
                                var cEmpN = Datarow.GetType().GetProperty("DTF_EMPNUMBER");
                                var vEmpN = cEmpN.GetValue(Datarow, null);
                                if (!String.IsNullOrEmpty(vEmpN.ToString()))
                                {
                                    var cComp = Datarow.GetType().GetProperty("COMPANYID");
                                    var vComp = cComp.GetValue(Datarow, null);
                                    cvalue = String.Concat(vEmpN, "_", vComp);
                                    val.SetValue(Datarow, cvalue);
                                    val2.SetValue(Datarow, "Custom");
                                    return Datarow;
                                }                                

                            }

                            var lreturn1 = _ixmcommon.CheckForValidIDNumber(cvalue.ToString(), ls1.ToString());
                            if (lreturn1.Item1 == false)
                            {
                                Datarow.ERRORNUM = lreturn1.Item2;
                                Datarow.ERRORDESCRIPTION = lreturn1.Item3;
                                //return null;

                            }

                            var lreturn2 = _ixmcommon.GetGenderFromIDNumber(cvalue.ToString());
                            Datarow.GetType().GetProperty("TRL_GENDER").SetValue(Datarow, lreturn2.Item3);

                        }
                        else if ((Columnname == "DTF_GENDER") && (cvalue != null))
                        {
                            var cGen = Datarow.GetType().GetProperty("TRL_GENDER");
                            if (!String.IsNullOrEmpty(cvalue.ToString()))
                            { 
                                cGen.SetValue(Datarow, cvalue);
                                return Datarow;

                            }
                            else return null;

                        }

                        /*if (ls1.ToString() == "Custom")
                        {
                            var lreturn1 = _ixmcommon.CheckForValidIDNumber(cvalue.ToString(), Datarow.COMPANYID.ToString());
                            if (lreturn1.Item1 == false)
                            {
                                Datarow.ERRORNUM = lreturn1.Item2;
                                Datarow.ERRORDESCRIPTION = lreturn1.Item3;
                                //return null;
                            }

                        }*/


                        return Datarow;


                    }
                    else
                    {
                        return null;
                    }
                //}
                //else { return null; }

            }
            catch (Exception)
            {

                return null;
            }
        }
        public IXLRow? SetRowVal(string Columnname, IXLRow Datarow)
        {

            var val = Datarow.GetType().GetProperty(Columnname);
            if (val != null)
            {
                var cvalue = val.GetValue(Datarow, null);
                if ((Columnname == "DTF_DBCONTRIBUTION") && (cvalue != null))
                {

                    string col2 = "TRL_IAMOUNT";
                    var lreturn1 = _ixmcommon.CheckValueForDecimal(cvalue.ToString());
                    if (lreturn1.Item1 == true)
                    {
                        var val2 = Datarow.GetType().GetProperty(col2);
                        val2.SetValue(Datarow, lreturn1.Item3);
                        return Datarow;

                    }
                    else return null;

                }
                else
                {
                    return null;
                }
            }
            else { return null; }
        }

        /*public void CopyNewXlsValues(ref XLS_REMITTANCE source, XLS_REMITTANCE target)
        {
            target.MODBY = source.MODBY;
            target.MODAT = source.MODAT;
            target.DTF_ADMINFEE = source.DTF_ADMINFEE;
            target.DTF_BCOMPANYID = source.DTF_BCOMPANYID;
            target.DTF_CELLNUMBER = source.DTF_CELLNUMBER;
            target.DTF_CITYID = source.DTF_CITYID;
            target.DTF_DBCOMPANYID = source.DTF_DBCOMPANYID;
            target.DTF_DOB = source.DTF_DOB;
            target.DTF_ECONTRIBUTION = source.DTF_ECONTRIBUTION;
            target.DTF_EMPNUMBER = source.DTF_EMPNUMBER;
            target.DTF_MNAME = source.DTF_MNAME;
            target.DTF_MSURNAME = source.DTF_MSURNAME;
            target.DTF_GENDER = source.DTF_GENDER;
        }*/


        /// <summary>
        /// 
        /// Amn Imortant Helper procedure. This was geenrated with the assistance oif ChatGPT
        /// This will be a Helper Procxedure.....This will need to be stoed wirth other Helper routines, so they are in a general location
        /// 
        /// </summary>
        /// 

        public static void CopyModelValues<T>(T source, T target)
        {
            if (source == null || target == null)
                throw new ArgumentNullException("Source or target cannot be null");

            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    var value = prop.GetValue(source);
                    prop.SetValue(target, value);
                }
            }
        }


        public List<TRMBLD> IxmRemittanceEntityImporter(IEnumerable<XLS_REMITTANCE> xlsEntity)
        {
            if (xlsEntity != null)
            {
                //List<TRMBL>

            }
         return null;
        }
    }
}
