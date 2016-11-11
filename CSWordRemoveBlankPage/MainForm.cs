using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Shuffler.Helper;

namespace Shuffler
{
    
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

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (SelectedFileDoesNotExist())
            {
                ShowInvalidFilePathErrorMessage();

                return;
            }

            if (FormatDocument())
            {
                MessageBox.Show(_documentShuffledSuccessfullyMessage);
            }
        }


        private bool FormatDocument()
        {
            Word.Application wordapp = null;
            Word.Document doc = null;
            Word.Paragraphs paragraphs = null;

            try
            {
                wordapp = new Word.Application { Visible = false };
                doc = wordapp.Documents.Open(wordPath);
                paragraphs = DocumentFormatter.RemoveBlankPages(doc, wordapp);

                SaveDocumentAsNewFileAndClose(doc);
                wordapp.Quit();
            }
            catch (Exception ex)
            {
                DisplayException(ex);
                return false;
            }
            finally
            {
                CleanUpUnmangedComResources(
                    ref wordapp, ref doc, ref paragraphs);
            }

            return true;
        }

        private void SaveDocumentAsNewFileAndClose(Word.Document doc)
        {
            var path = wordPath.Replace(wordPath.Substring(wordPath.LastIndexOf("\\", StringComparison.Ordinal)), "");
            var docNameExtension = doc.Name.Substring(doc.Name.LastIndexOf('.'));
            string newName = doc.Name.Replace(docNameExtension, "S" + docNameExtension);

            doc.SaveAs(path + "\\" + newName);
            doc.Close();
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
            ref Word.Document doc, 
            ref Word.Paragraphs paragraphs)
        {
            if (paragraphs != null)
            {
                Marshal.FinalReleaseComObject(paragraphs);
                paragraphs = null;
            }
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
