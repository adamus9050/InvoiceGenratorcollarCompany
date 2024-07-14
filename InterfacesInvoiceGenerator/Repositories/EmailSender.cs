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

        //public async Task SendEmailAsync(string email, string subject, string message)
        //{
        //    var yourmail = "adamus9050@gmail.com";
        //    var passwd = "Kazik253033";
        //    MailMessage mail = new MailMessage(
        //        To adresat = new To(yourmail)

        //        );
        //    //var client = new SmtpClient("smtp.office365.com", 587)
        //    //{
        //    //    EnableSsl = true,
        //    //    UseDefaultCredentials = false,
        //    //    Credentials = new NetworkCredential(yourmail, passwd)
        //    //};
        //    Attachment data = new Attachment(subject, MediaTypeNames.Application.Octet);
        //    ContentDisposition disposition = data.ContentDisposition;
        //    disposition.CreationDate = System.IO.File.GetCreationTime(subject);
        //    disposition.ModificationDate = System.IO.File.GetLastWriteTime(subject);
        //    disposition.ReadDate = System.IO.File.GetLastAccessTime(subject);
        //    // Add the file attachment to this email message.
        //    mail.Attachments.Add(data);

        //    //return client.SendMailAsync(
        //    //    new MailMessage(from: yourmail,
        //    //                    to: email,
        //    //                    subject,
        //    //                    message
        //    //                    ));




        //}

        public async Task CreateTestMessage4(string emailTo,string attach ,string server = "smtp.gmail.com")//, string subject, string message
        {
            MailAddress from = new MailAddress("adamus9050@gmail.com");
            MailAddress to = new MailAddress(emailTo);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Using the SmtpClient client";
            message.Body = "snjcsbsdbcjhdsbcljjd";
            
            //tworzenie załącznika
            Attachment data = new Attachment(attach, MediaTypeNames.Application.Octet);
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(attach);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(attach);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(attach);
            // Add the file attachment to this email message.
            message.Attachments.Add(data);

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential("adamus9050@gmail.com", "Kazik253033");
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            try
            {
                await client.SendMailAsync(message);
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
