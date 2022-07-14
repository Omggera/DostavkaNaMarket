using OfficeOpenXml;

namespace DostavkaNaMarket.Models
{
    public class ExcelCreate
    {
        public void Excel()
        {
            FileInfo file = new FileInfo(@"D:\Base.xlsx");
            using (ExcelPackage excelPackage = new ExcelPackage(file))
            {
                //Get a WorkSheet by index. Note that EPPlus indexes are base 1, not base 0!
                ExcelWorksheet ws = excelPackage.Workbook.Worksheets["Sheet"];

                //Get the content from cells A1 and B1 as string, in two different notations
                ws.Cells["B26"].Value = "10";

                //Save your file
                excelPackage.SaveAs(new FileInfo(@"D:\myworkbook.xlsx"));


            }
        } 
    }
}
