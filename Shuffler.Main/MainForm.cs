using System;
using System.IO;
using System.Windows.Forms;
using Shuffler.Helper;

namespace Main
{
    using System.Collections.Generic;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Services;

    public partial class MainForm : Form
    {
        private static string wordPath;

        private const string _invalidFilePathErrorHeaderText = "File not found";
        private const string _invalidFilePathErrorDescriptionText = "Invalid path - no word document found.";
        private const string _exceptionOccurredErrorMessageTemplate = "Exception Occured, error message is: {0}";

        private const string _documentShuffledSuccessfullyMessage = "The document was shuffled successfully!";
        private const string _documentShuffledUnSuccessfullyMessage = "Something went wrong! Document may not have been shuffled correctly";
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
            }
            else
            {
                MessageBox.Show(FormatDocument()
                    ? _documentShuffledSuccessfullyMessage
                    : _documentShuffledUnSuccessfullyMessage);
            }
        }

        private static bool FormatDocument()
        {
            var documentFormatter = new DocumentFormatter(
                new ClauserUnitStrategy(new ClauserUnitChecker()),
                new AdverbUnitStrategy(),
                new TimerUnitStrategy());

            List<OpenXmlElement> shuffledXmlElements = new List<OpenXmlElement>();

            try
            {
                using (var document = WordprocessingDocument.Open(wordPath, true))
                {
                    var docPart = document.MainDocumentPart;
                    if (docPart?.Document != null)
                    {
                        shuffledXmlElements = documentFormatter.ProcessDocument(docPart);
                    }
                    CreateAndSaveShuffledDocument(shuffledXmlElements);
                }
            }
            catch (Exception ex)
            {
                DisplayException(ex);
                return false;
            }

            return true;
        }

        private static void CreateAndSaveShuffledDocument(
            List<OpenXmlElement> shuffledXmlElements)
        {
            string path = GetPathAndFileNameForNewWordDocument();
            using (
                WordprocessingDocument doc = WordprocessingDocument.Create
                    (path, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();

                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                foreach (var openXmlElement in shuffledXmlElements)
                {
                    body.AppendChild(openXmlElement.CloneNode(true));
                }
                SaveChanges(doc);
            }
        }

        private static string GetPathAndFileNameForNewWordDocument()
        { 
            var docNameExtension =
                wordPath.Substring(wordPath.LastIndexOf('.'));

            string newName =
                wordPath.Replace(docNameExtension, "S" + docNameExtension);

            return newName;
        }

        private static void SaveChanges(WordprocessingDocument docx)
        {
            using (var sr = new StreamReader(docx.MainDocumentPart.GetStream()))
            using (var sw = new StreamWriter(docx.MainDocumentPart.GetStream(FileMode.Create)))
            {
                sw.Write(sr.ReadToEnd());
            }
            docx.MainDocumentPart.Document.Save();
            docx.Close();
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
    }
}
