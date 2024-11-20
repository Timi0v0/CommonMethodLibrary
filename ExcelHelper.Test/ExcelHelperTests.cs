using OfficeOpenXml;
using System.Data;
using System.Text;

namespace ExcelHelper.Test
{
    public class ExcelHelperTests
    {
        [Fact]

        public void ExcelHelperTest()
        {
            string path = "C:\\Users\\tm200\\Desktop\\项目\\BTS\\苏州瀚码智能\\采集数据列表.xlsx";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            DataTable result = ExcelHelper.ExcelToDataTable(path);
        }

        [Fact]
        public void ExcelToDataTable_ValidFile_ReturnsDataTable()
        {
            // Arrange
            string filePath = "test.xlsx";
            CreateTestExcelFile(filePath);

            // Act
            DataTable result = ExcelHelper.ExcelToDataTable(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Rows.Count);
            Assert.Equal(2, result.Columns.Count);
            Assert.Equal("Value1", result.Rows[0][0]);
            Assert.Equal("Value2", result.Rows[0][1]);
            Assert.Equal("Value3", result.Rows[1][0]);
            Assert.Equal("Value4", result.Rows[1][1]);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void ExcelToDataTable_NoSheets_ThrowsException()
        {
            // Arrange
            string filePath = "empty.xlsx";
            CreateEmptyExcelFile(filePath);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => ExcelHelper.ExcelToDataTable(filePath));
            Assert.Equal("Excel 文件中没有工作表。", exception.Message);

            // Cleanup
            File.Delete(filePath);
        }

        private void CreateTestExcelFile(string filePath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells[1, 1].Value = "Column1";
                worksheet.Cells[1, 2].Value = "Column2";
                worksheet.Cells[2, 1].Value = "Value1";
                worksheet.Cells[2, 2].Value = "Value2";
                worksheet.Cells[3, 1].Value = "Value3";
                worksheet.Cells[3, 2].Value = "Value4";
                package.Save();
            }
        }

        private void CreateEmptyExcelFile(string filePath)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                package.Save();
            }
        }
    }
}