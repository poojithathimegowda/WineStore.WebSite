using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net.Http;
using WineStore.WebSite.Models.Admin;

namespace WineStore.WebSite.Managers
{
    public class CustomPdfPageEventHelper : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document )
        {
            PdfPTable headerTable = new PdfPTable(1);
            headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;

           

            headerTable.AddCell(new PdfPCell(new Phrase("Header: Profit and Loss Statement"))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
           
            });
            headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 10, writer.DirectContent);

            PdfPTable footerTable = new PdfPTable(1);
            footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footerTable.AddCell(new PdfPCell(new Phrase("Footer: Page " + writer.PageNumber))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin - 10, writer.DirectContent);
        }
    }
    public static class Helper
    {
    


        public static TOutput ConvertToObjectType<TOutput>(IFormCollection collection) where TOutput : new()
        {
            TOutput output = new TOutput();

            foreach (var key in collection.Keys)
            {
                var propertyInfo = typeof(TOutput).GetProperty(key);
                if (propertyInfo != null)
                {
                    try
                    {
                        var value = Convert.ChangeType(collection[key].ToString(), propertyInfo.PropertyType);
                        propertyInfo.SetValue(output, value);
                    }
                    catch (InvalidCastException)
                    {
                        // Handle the error or ignore if the property is optional
                    }
                }
            }

            return output;
        }

    }
}





