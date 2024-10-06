using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructures.Context.Data;
using Infrastructures.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Models;
using System.Net.Mime;
using Humanizer;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Infrastructures.Repositories
{
    public class EmailSender : IEmailSender
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<InvoiceGeneratorCollarCompanyContext> _userManager;
        public EmailSender(ApplicationDbContext db, UserManager<InvoiceGeneratorCollarCompanyContext> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task CreateTestMessage4(string emailTo, string attach, string server = "smtp.gmail.com")//, string subject, string message
        {
            //Wysyłanie maila
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("adamus9050@gmail.com");
            mailMessage.To.Add(emailTo);
            mailMessage.Subject = "Test Mail Subject";
            mailMessage.Body = "Test message from my mail";

            //tworzenie załącznika
            Attachment data = new Attachment(attach, MediaTypeNames.Application.Octet);
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(attach);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(attach);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(attach);
            //Dodawanie załącznika do maila
            mailMessage.Attachments.Add(data);

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("adamus9050", "anlwymqqkbfxqalw");
            client.EnableSsl = true;

            try
            {
                await client.SendMailAsync(mailMessage);
                Console.WriteLine("Email was succesfully sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage4(): {0}",
                ex.ToString());
            }
        }
        public async Task ExcellDocGenerator(string filename, IEnumerable<CartDetail> orderDetails)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(filename, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Faktura" };// nazwa arkusza
                sheets.Append(sheet);

                workbookPart.Workbook.Save();
                WriteningToExcell(worksheetPart, orderDetails);

            }
        }

        private static void WriteningToExcell(WorksheetPart worksheetPart, IEnumerable<CartDetail> orderDetails)
        {
            SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());
            
            Row row = new Row();
            row.Append(new Cell() { CellValue = new CellValue("Nazwa Produktu"), DataType = CellValues.String },
                new Cell() { CellValue = new CellValue("Ilość kołnierzy"), DataType = CellValues.String },
                new Cell() { CellValue = new CellValue("Cena kołnierza"), DataType = CellValues.String },
                new Cell() { CellValue = new CellValue("Nazwa materiału"), DataType = CellValues.String },
                new Cell() { CellValue = new CellValue("Cena materiału"), DataType = CellValues.String },
                new Cell() { CellValue = new CellValue("Cena jednostkowa"), DataType = CellValues.String });
            sheetData.AppendChild(row);
            foreach (CartDetail orderDetail in orderDetails)
            {
                Row row1 = new Row();
                row1.Append(new Cell() { CellValue = new CellValue(orderDetail.SizeProducts.Product.ProductName), DataType = CellValues.String },
                new Cell() { CellValue = new CellValue(orderDetail.Quantity), DataType = CellValues.String },
                new Cell() { CellValue = new CellValue(orderDetail.SizeProducts.Product.ProductName), DataType = CellValues.String },
                new Cell() { CellValue = new CellValue(orderDetail.Materials.Name), DataType = CellValues.String },
                new Cell() { CellValue = new CellValue(orderDetail.Materials.Price), DataType = CellValues.String });
                sheetData.AppendChild(row1);
            }

            Row row2 = new Row();
                row2.Append(new Cell() { CellValue = new CellValue("Jakaś cena"), DataType = CellValues.String });

            sheetData.AppendChild(row2);

            worksheetPart.Worksheet.Save();
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }
}
