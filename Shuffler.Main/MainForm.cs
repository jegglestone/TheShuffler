using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Shuffler.Helper;

namespace Main
{
    using DocumentFormat.OpenXml.Packaging;
    using Services;

    public partial class MainForm : Form
    {
        private string wordPath;

        private const string _invalidFilePathErrorHeaderText = "File not found";
        private const string _invalidFilePathErrorDescriptionText = "Invalid path - no word document found.";
        private const string _exceptionOccurredErrorMessageTemplate = "Exception Occured, error message is: {0}";

        private const string _documentShuffledSuccessfullyMessage = "The document was shuffled successfully!";
        private const string _WordDocumentFilter = "Word document(*.doc,*.docx)|*.doc;*.docx";

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnOpenWord_Click(object sender, EventArgs e)
        {
            using(var openfileDialog=
                new OpenFileDialog { Filter = _WordDocumentFilter})
            {
                if (openfileDialog.ShowDialog() == DialogResult.OK)
                {
                    txbWordPath.Text = openfileDialog.FileName;
                    wordPath = openfileDialog.FileName;
                }
            }
        }

        private void btnShuffle_Click(object sender, EventArgs e)
        {
            if (SelectedFileDoesNotExist())
            {
                ShowInvalidFilePathErrorMessage();

                return;
            }

            string newDocument = SaveDocumentAsNewFile();

            if (FormatDocument(newDocument))
            {
                MessageBox.Show(_documentShuffledSuccessfullyMessage);
            }
            else
            {
                DisplayException(new Exception("An error occured"));
            }
        }

        private static bool FormatDocument(string documentName)
        {
            var documentFormatter = new DocumentFormatter(
                new ClauserUnitStrategy(new ClauserUnitChecker()),
                new AdverbStrategy());

            using (var document = WordprocessingDocument.Open(documentName, true))
            {
                var docPart = document.MainDocumentPart;
                if (docPart?.Document != null)
                {
                    documentFormatter.ProcessDocument(docPart);
                }
                SaveChanges(document);
            }
            return true;
        }

        private string SaveDocumentAsNewFile()
        {
            var wordapp = new Word.Application { Visible = false };
            var doc = wordapp.Documents.Open(wordPath);

            var path = wordPath.Replace(
                wordPath.Substring(wordPath.LastIndexOf("\\", StringComparison.Ordinal)), "");

            var docNameExtension = 
                doc.Name.Substring(doc.Name.LastIndexOf('.'));

            string newName = 
                doc.Name.Replace(docNameExtension, "S" + docNameExtension);

            string documentNameAndPath = path + "\\" + newName;

            doc.SaveAs(documentNameAndPath);

            doc.Close();
            
            CleanUpUnmangedComResources(ref wordapp, ref doc);

            return documentNameAndPath;
        }

        private static void SaveChanges(WordprocessingDocument docx)
        {
            using (var sr = new StreamReader(docx.MainDocumentPart.GetStream()))
            using (var sw = new StreamWriter(docx.MainDocumentPart.GetStream(FileMode.Create)))
            {
                sw.Write(sr.ReadToEnd());
            }
            docx.MainDocumentPart.Document.Save();
        }

        private static void DisplayException(Exception ex)
        {
            MessageBox.Show(
                string.Format(
                    _exceptionOccurredErrorMessageTemplate
                    , ex.Message));
        }

        private static void ShowInvalidFilePathErrorMessage()
        {
            MessageBox.Show
                (_invalidFilePathErrorDescriptionText
                , _invalidFilePathErrorHeaderText
                , MessageBoxButtons.OK
                , MessageBoxIcon.Error);
        }

        private bool SelectedFileDoesNotExist()
        {
            return !File.Exists(txbWordPath.Text);
        }

        private static void CleanUpUnmangedComResources(
            ref Word.Application wordapp, 
            ref Word.Document doc
            )
        {
            if (doc != null)
            {
                Marshal.FinalReleaseComObject(doc);
                doc = null;
            }
            if (wordapp != null)
            {
                Marshal.FinalReleaseComObject(wordapp);
                wordapp = null;
            }
        }
    }
}
