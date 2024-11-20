using OfficeOpenXml;
using System.Data;

namespace ExcelHelper
{
    public class ExcelHelper
    {
        /// <summary>
        /// 读取excel文件并且将其转换为DataTable类型
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="sheetIndex">表格索引</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DataTable ExcelToDataTable(string filePath, int sheetIndex = 0)
        {
            DataTable dt = new DataTable();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                if (package.Workbook.Worksheets.Count == 0)
                {
                    throw new Exception("Excel 文件中没有工作表。");
                }
                //获取指定的工作表
                var worksheet = package.Workbook.Worksheets[sheetIndex + 1];
                //获取工作表的行数
                int rowCount = worksheet.Dimension.Rows;
                //获取工作表的列数
                int colCount = worksheet.Dimension.Columns;
                //添加列名
                for (int i = 1; i <= colCount; i++)
                {
                    dt.Columns.Add(worksheet.Cells[1, i].Value.ToString());
                }
                //添加数据
                for (int i = 2; i <= rowCount; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 1; j <= colCount; j++)
                    {
                        dr[j - 1] = worksheet.Cells[i, j].Value;
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static List<T> DataTableToModel<T>(DataTable dt) where T : new()
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                foreach (var prop in typeof(T).GetProperties())
                {
                    var columnAttr = prop.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault() as ColumnAttribute;
                    if (columnAttr != null)
                    {
                        var value = dr[columnAttr.ColumnName];
                        if (value != DBNull.Value)
                        {
                            prop.SetValue(t, value);
                        }
                    }
                }
                list.Add(t);
            }
            return list;
        }
        /// <summary>
        /// 读取Excel文件并将其转换为指定类型的List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        public static List<T> ReadExcel<T>(string filePath, int sheetIndex = 0) where T : new()
        {
            return DataTableToModel<T>(ExcelToDataTable(filePath, sheetIndex));
        }
    }
}
