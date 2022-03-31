using Library.Models;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace Library.Class
{
    enum FileType
    {
        pdf,
        doc
    }

    class ReportWorker
    {
        public static void KillProcess(string processName)
        {
            foreach (var process in Process.GetProcesses().Where(p => p.ProcessName == processName))
            {
                process.Kill();
            }
        }
        public static async Task ReaderWordGenerate(Reader data, LibraryСard libraryСard)
        {
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить файл как...";
            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.Filter = "Word документ (*.doc)|*.doc";
            if (savedialog.ShowDialog() == true)
            {
                await Task.Run(() =>
                {
                    var word = new Word.Application();
                    var document = word.Documents.Open(Environment.CurrentDirectory + @"\читательский_билет_template.docx");
                    DateTime date = DateTime.Now;
                    try
                    {
                        string imagePath = $@"{Directory.GetCurrentDirectory()}\imageToPaste.jpg";
                        File.WriteAllBytes(imagePath, data.Photo);

                        string bookData = String.Empty;
                        foreach (RecordBook record in libraryСard.Records)
                        {
                            bookData += $"{record.DateStart.ToString("dd.MM.yyyy hh:mm")} {record.DateEnd.ToString("dd.MM.yyyy hh:mm")} {record.Book.Title} {record.Book.Author} {record.Book.Publisher}\n";
                        }

                        document.Bookmarks["Фото"].Range.InlineShapes.AddPicture(imagePath, Type.Missing, true);
                        document.Bookmarks["Номер"].Range.Text = $"{data.LastName.Length}{data.MiddleName.Length}{data.FirstName.Length}";
                        document.Bookmarks["Фамилия"].Range.Text = data.LastName;
                        document.Bookmarks["Имя"].Range.Text = data.FirstName;
                        document.Bookmarks["Отчество"].Range.Text = data.MiddleName;
                        document.Bookmarks["Дата"].Range.Text = DateTime.Now.ToString("dd.MM.yyyy");
                        document.Bookmarks["Книги"].Range.Text = bookData;

                        document.SaveAs2(savedialog.FileName, Word.WdSaveFormat.wdFormatDocument, Word.WdSaveOptions.wdDoNotSaveChanges);
                        document.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                        word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
                        MessageBox.Show("Отчёт успешно создан!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        document.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                        word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
                    }
                });
            }
        }
        public static async Task GenerateExcelReport(LibraryСard libraryCard)
        {
            await Task.Run(() =>
            {
                try
                {
                    SaveFileDialog savedialog = new SaveFileDialog();
                    savedialog.Title = "Сохранить файл как...";
                    savedialog.OverwritePrompt = true;
                    savedialog.CheckPathExists = true;
                    savedialog.Filter = $"Список книг (*.xlsx)|*.xlsx";
                    if (savedialog.ShowDialog() == true)

                        ReportWorker.KillProcess("EXCEL");
                    Excel.Application app = new Excel.Application();
                    Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
                    Excel.Worksheet sheet = (Excel.Worksheet)app.Worksheets[1];

                    sheet.Cells[1, 1] = "№ п/п";
                    sheet.Cells[1, 2] = "Срок возвращения";
                    sheet.Cells[1, 3] = "Автор и название";
                    sheet.Cells[1, 4] = "Издательство";
                    sheet.Cells[1, 5] = "Отметка о сдаче";
                    int row = 2;

                    foreach (RecordBook record in libraryCard.Records)
                    {
                        sheet.Cells[row, 1] = (row - 1).ToString();
                        sheet.Cells[row, 2] = record.DateForReturn.ToString();
                        sheet.Cells[row, 3] = record.Book.AuthorTitle;
                        sheet.Cells[row, 4] = record.Book.PublisherEx;
                        string mark = "";
                        if (record.DateForReturn > 7)
                        {
                            mark = "сдана не вовремя";
                        }
                        else
                        {
                            mark = "сдана вовремя";
                        }
                        sheet.Cells[row, 5] = mark;
                        row++;
                    }

                    app.Application.ActiveWorkbook.SaveAs(savedialog.FileName);
                    app.Application.Quit();
                    MessageBox.Show("Отчет сформирован");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
        public static async Task GenerateReport(LibraryСard libraryСard, FileType fileType)
        {
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить файл как...";
            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.Filter = $"Список книг (*.{fileType.ToString()})|*.{fileType.ToString()}";
            if (savedialog.ShowDialog() == true)
            {
                await Task.Run(() =>
                {
                    var word = new Word.Application();
                    var document = word.Documents.Open(Environment.CurrentDirectory + @"\список_книг_template.docx");
                    DateTime date = DateTime.Now;
                    try
                    {
                        var table1 = document.Tables[1];
                        int row = 1;
                        foreach (var item in libraryСard.Records)
                        {

                            row++;
                            table1.Rows.Add();
                            table1.Cell(row, 1).Range.Text = (row - 1).ToString();
                            table1.Cell(row, 2).Range.Text = item.DateForReturn.ToString();
                            table1.Cell(row, 3).Range.Text = item.Book.AuthorTitle;
                            table1.Cell(row, 4).Range.Text = item.Book.PublisherEx;
                            string mark = "";
                            if (item.DateForReturn > 7)
                            {
                                mark = "сдана не вовремя";
                            }
                            else
                            {
                                mark = "сдана вовремя";
                            }
                            table1.Cell(row, 5).Range.Text = mark;
                        }

                        int T = table1.Rows.Count;
                        table1.Rows[T].Delete();
                        if (fileType == FileType.doc)
                        {
                            document.SaveAs2(savedialog.FileName, Word.WdSaveFormat.wdFormatDocument, Word.WdSaveOptions.wdDoNotSaveChanges);
                        }
                        else if (fileType == FileType.pdf)
                        {
                            document.SaveAs2(savedialog.FileName, Word.WdSaveFormat.wdFormatPDF, Word.WdSaveOptions.wdDoNotSaveChanges);
                        }
                        else
                        {
                            MessageBox.Show("Данный формат не поддерживается");
                        }
                        document.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                        word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
                        MessageBox.Show("Отчёт успешно создан!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        document.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                        word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
                    }
                });
            }
        }
    }
}
