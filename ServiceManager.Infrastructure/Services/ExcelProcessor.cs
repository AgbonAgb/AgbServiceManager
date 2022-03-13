using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Net.SourceForge.Koogra;
using Net.SourceForge.Koogra.Excel;
using System.Data;

namespace ServiceManager.Infrastructure.Services
{
    public class ExcelProcessor
    {


        private object convertValue(PropertyInfo pinfo, object value)
        {
            if (pinfo.PropertyType == typeof(DateTime))
            {
                DateTime retDate;
                DateTime.TryParse("" + value, out retDate);
                return retDate;
            }
            if (pinfo.PropertyType == typeof(DateTime?))
            {
                DateTime retDate;
                DateTime.TryParse("" + value, out retDate);
                return retDate;
            }
            if (pinfo.PropertyType == typeof(int))
            {
                int ret;
                int.TryParse("" + value, out ret);
                return ret;
            }
            if (pinfo.PropertyType == typeof(int?))
            {
                int ret;
                int.TryParse("" + value, out ret);
                return ret;
            }
            if (pinfo.PropertyType == typeof(double))
            {
                double ret;
                double.TryParse("" + value, out ret);
                return ret;
            }
            if (pinfo.PropertyType == typeof(double?))
            {
                double ret;
                double.TryParse("" + value, out ret);
                return ret;
            }
            if (pinfo.PropertyType == typeof(decimal))
            {
                decimal ret;
                decimal.TryParse("" + value, out ret);
                return ret;
            }
            if (pinfo.PropertyType == typeof(decimal?))
            {
                decimal ret;
                decimal.TryParse("" + value, out ret);
                return ret;
            }
            if (pinfo.PropertyType == typeof(float))
            {
                float ret;
                float.TryParse("" + value, out ret);
                return ret;
            }
            if (pinfo.PropertyType == typeof(float?))
            {
                float ret;
                float.TryParse("" + value, out ret);
                return ret;
            }
            if (pinfo.PropertyType == typeof(bool))
            {
                bool ret;
                bool.TryParse("" + value, out ret);
                return ret;
            }
            if (pinfo.PropertyType == typeof(bool?))
            {
                bool ret;
                bool.TryParse("" + value, out ret);
                return ret;
            }
            if (pinfo.PropertyType.IsSubclassOf(typeof(Enum)))
            {
                MethodInfo minfo = typeof(Enum).GetMethod("Parse", new[] { typeof(Type), typeof(string), typeof(bool) });
                try
                {
                    return minfo.Invoke(null, new object[] { pinfo.PropertyType, "" + value, true });
                }
                catch (Exception)
                {
                    return null;
                }
            }
            if (pinfo.PropertyType == typeof(string))
            {
                return value.ToString();
            }
            return null;
        }

        public enum ExcelType
        {
            Xls,
            Xlsx
        };


        public IEnumerable<T> ImportExcelData<T>(string Fpath , ExcelType extension) where T : class, new()
        {
            FileStream uploadStream = File.OpenRead(Fpath);


            //var _grdRowCount = 0;
            var output = new List<T>();
            var type = typeof(T);
            Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
            foreach (PropertyInfo pinfo in type.GetProperties()) properties[pinfo.Name.ToLower()] = pinfo;

            var headers = new List<string>();
            //var table = new DataTable(tableName);

            switch (extension)
            {
                case ExcelType.Xls:

                    #region Office 2003

                    var wb = new Workbook(uploadStream);

                    //load by index
                    var ws = wb.Sheets[0];

                    // column header stuff

                    var rowHeader = ws.Rows[0];

                    for (var c = rowHeader.Cells.MinCol; c <= (rowHeader.Cells.MaxCol); ++c)
                    {
                        if (rowHeader != null)
                        {
                            //if (rowHeader.Cells[c].Value == null) continue;
                            if (rowHeader.Cells[c] == null) continue;
                            var val = (string)(rowHeader.Cells[c].Value);
                            if (!string.IsNullOrEmpty(val))
                            {
                                headers.Add(val.Replace(" ", string.Empty).ToLower().Trim());
                            }
                        }

                    }

                    //Read Cells in Order
                    for (uint r = 1; r <= (ws.Rows.MaxRow); ++r)
                    {
                        var item = new T();
                        var row = ws.Rows[r];
                        bool hasValue = false;

                        if (row.Cells.InternalValues.Any())
                        {
                            for (var c = row.Cells.MinCol; c <= row.Cells.MaxCol; c++)
                            {

                                var header = headers[int.Parse(c.ToString())];
                                if (!properties.ContainsKey(header)) continue;
                                //else
                                PropertyInfo pinfo = properties[header];
                                var cell = row.Cells[c].Value ?? "";
                                //string cell;
                                // var cell = row.Cells[c].Value == null ? "" : row.Cells[c].Value;
                                // cell = row.Cells[c].Value == null ? "" : row.Cells[c].Value.ToString();

                                if (cell == "") continue;

                                var val = convertValue(pinfo, row.Cells[c].Value);

                                pinfo.SetValue(item, val);
                                hasValue = true;

                            }
                            if (hasValue)
                                output.Add(item);
                        }


                    }

                    #endregion

                    break;
                case ExcelType.Xlsx:


                    #region Office 2007 And Above

                    var wb2 = new Net.SourceForge.Koogra.Excel2007.Workbook(uploadStream);

                    //load by index
                    var ws2 = wb2.GetWorksheet(0);

                    // column header stuff

                    var rowHeader2 = ws2.GetRow(0);

                    for (var c = ws2.CellMap.FirstCol; c <= ws2.CellMap.LastCol; ++c)
                    {
                        if (rowHeader2.GetCell(c).Value == null) continue;
                        var val = (string)rowHeader2.GetCell(c).Value;

                        if (!string.IsNullOrEmpty(val))
                        {
                            headers.Add(val.Replace(" ", string.Empty).ToLower().Trim());
                        }
                    }

                    //Read Cells in Order
                    for (uint r = 1; r <= ws2.CellMap.LastRow; ++r)
                    {
                        var item = new T();
                        var row = ws2.GetRow(r);
                        bool hasValue = false;


                        for (var c = ws2.CellMap.FirstCol; c <= ws2.CellMap.LastCol; c++)
                        {
                            try
                            {
                                var header = headers[int.Parse(c.ToString())];
                                if (!properties.ContainsKey(header)) continue;
                                //else
                                PropertyInfo pinfo = properties[header];

                                var emptyCell = row.GetCell(c).Value == null ? "" : row.GetCell(c).Value.ToString();
                                if (string.IsNullOrEmpty(emptyCell)) continue;
                                //if (string.IsNullOrEmpty(row.GetCell(c).Value.ToString())) continue;

                                var val = convertValue(pinfo, row.GetCell(c).Value);

                                pinfo.SetValue(item, val);
                                hasValue = true;
                            }
                            catch (Exception)
                            {

                            }

                        }
                        if (hasValue)
                            output.Add(item);

                    }

                    #endregion

                    break;
            }


            return output;
        }

        public DataTable ImportExcelData(string tableName, string path, string extension)
        {

            //var _grdRowCount = 0;

            var table = new DataTable(tableName);

            switch (extension.ToLower())
            {
                case ".xls":
                    #region Office 2003

                    var wb = new Workbook(path);

                    //load by index
                    var ws = wb.Sheets[0];

                    // column header stuff

                    var rowHeader = ws.Rows[0];

                    for (var c = rowHeader.Cells.MinCol; c <= rowHeader.Cells.MaxCol; ++c)
                    {
                        if (rowHeader.Cells[c] == null) continue;
                        var val = (string)(rowHeader.Cells[c].Value);
                        if (val != null)
                        {
                            table.Columns.Add(val);
                        }
                    }



                    for (uint r = 1; r <= ws.Rows.MaxRow; ++r)
                    {
                        var row = ws.Rows[r];
                        var rw = table.NewRow();
                        for (var c = row.Cells.MinCol; c <= row.Cells.MaxCol; ++c)
                        {
                            var val = row.Cells[c].FormattedValue();
                            if (val != null)
                            {
                                rw[(int)c] = val;
                            }
                        }
                        //_grdRowCount++;
                        table.Rows.Add(rw);
                    }

                    #endregion
                    break;
                case ".xlsx":
                    #region Office 2007 and Above
                    // load .xlsx sample
                    var wb2007 = new Net.SourceForge.Koogra.Excel2007.Workbook(path);

                    // load by index
                    var ws2007 = wb2007.GetWorksheet(0);

                    // getting columns headers

                    var rowheader = ws2007.GetRow(0); //the reader is on row 5

                    for (uint c = ws2007.CellMap.FirstCol; c <= ws2007.CellMap.LastCol; ++c)
                    {
                        var val = (string)rowheader.GetCell(c).Value;
                        if (val != null)
                        {
                            table.Columns.Add(val);
                        }
                    }


                    // getting rows
                    for (uint r = 1; r <= ws2007.CellMap.LastRow; ++r) //start from row 6
                    {
                        var row = ws2007.GetRow(r); //the reader is on row 6
                        var rw = table.NewRow();
                        for (var c = ws2007.CellMap.FirstCol; c <= ws2007.CellMap.LastCol; ++c)
                        {
                            var val = row.GetCell(c).GetFormattedValue();
                            if (val != null)
                            {
                                rw[(int)c] = val;
                            }
                        }
                        //_grdRowCount++;
                        table.Rows.Add(rw);
                    }

                    #endregion
                    break;
            }


            return table;
        }
    }
}
