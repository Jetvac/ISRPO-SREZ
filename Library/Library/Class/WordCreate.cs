using Library.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace Library.Class
{
    public class WordCreate
    {
        public static async System.Threading.Tasks.Task Print(Reader data, LibraryСard libraryСard)
        {
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить файл как...";
            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.Filter = "Word документ (*.doc)|*.doc";
            if (savedialog.ShowDialog() == true)
            {
                await System.Threading.Tasks.Task.Run(() =>
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
    }
}
