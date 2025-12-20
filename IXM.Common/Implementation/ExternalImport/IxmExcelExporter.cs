using ClosedXML.Excel;
using System.Data;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using FirebirdSql.Data.FirebirdClient;

namespace IXM.Common.Implementation.ExcelExporter
{


    public class IxmExcelExporter
    {
        public void ExportDataSetToExcel(DataSet dataSet, string filePath, string Sheetname)
        {
            // Create a new Excel workbook
            using (var workbook = new XLWorkbook())
            {
                // Loop through each DataTable in the DataSet
                foreach (DataTable dt in dataSet.Tables)
                {
                    // Add a worksheet for the current DataTable
                    var worksheet = workbook.Worksheets.Add(dt, Sheetname);

                    // Auto-adjust columns to fit content
                    worksheet.Columns().AdjustToContents();
                }

                // Save the Excel file
                workbook.SaveAs(filePath);
            }
        }
    }



public static class DataTableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            // Get properties of type T using reflection
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Create columns in the DataTable
            foreach (PropertyInfo prop in properties)
            {
                // Handle nullable types
                Type columnType = prop.PropertyType;
                if (columnType.IsGenericType && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    columnType = Nullable.GetUnderlyingType(columnType);

                dataTable.Columns.Add(prop.Name, columnType);
            }

            // Populate rows
            foreach (T item in data)
            {
                DataRow row = dataTable.NewRow();
                foreach (PropertyInfo prop in properties)
                {
                    object value = prop.GetValue(item, null);
                    row[prop.Name] = value ?? DBNull.Value; // Handle nulls
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }

    public static class DataReaderExtensions
    {

        public static string xmGetOrdString(this FbDataReader reader, string ordin)
        {
            int idx = reader.GetOrdinal(ordin);
            if (reader.IsDBNull(idx))
                return string.Empty;
            else return reader.GetString(idx);


        }
        public static int? xmGetOrdInteger(this FbDataReader reader, string ordin)
        {
            int idx = reader.GetOrdinal(ordin);
            if (reader.IsDBNull(idx))
                return null;
            else return reader.GetInt32(idx);


        }
        public static double? xmGetOrdDouble(this FbDataReader reader, string ordin)
        {
            int idx = reader.GetOrdinal(ordin);
            if (reader.IsDBNull(idx))
                return null;
            else return reader.GetDouble(idx);

        }
        public static DateTime? xmGetOrdDateTime(this FbDataReader reader, string ordin)
        {
            int idx = reader.GetOrdinal(ordin);
            if (reader.IsDBNull(idx))
                return null;
            else return reader.GetDateTime(idx);

        }

    }

    }
