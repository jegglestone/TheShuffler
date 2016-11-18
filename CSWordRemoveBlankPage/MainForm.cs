using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Shuffler.Helper;

namespace Shuffler
{
    using DocumentFormat.OpenXml.Packaging;
    using System.IO.Packaging;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

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
            var documentFormatter = new DocumentFormatter(new ClauserUnitChecker());

            using (var document = WordprocessingDocument.Open(wordPath, false))
            {
                var docPart = document.MainDocumentPart;
                if (docPart?.Document != null)
                {
                    documentFormatter.ProcessDocument(docPart);
                }
            }
            return true;
        }

        #region alternative code

        private bool FormatDocument2()
        {
            const string fileName = "SampleDoc.docx";

            const string documentRelationshipType =
              "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument";
    
            const string wordmlNamespace =
              "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

            XNamespace w = wordmlNamespace;

            using (Package wdPackage = Package.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                PackageRelationship docPackageRelationship =
                  wdPackage.GetRelationshipsByType(documentRelationshipType).FirstOrDefault();
                if (docPackageRelationship != null)
                {
                    Uri documentUri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative),
                      docPackageRelationship.TargetUri);
                    PackagePart documentPart = wdPackage.GetPart(documentUri);
 
                    XmlDocument xDoc  = new XmlDocument();
                    xDoc.Load(XmlReader.Create(documentPart.GetStream()));
                    
                }
            }
            return true;
        }


        //private bool FormatDocument()
        //{
        //    Word.Application wordapp = null;
        //    Word.Document doc = null;
        //    Word.Paragraphs paragraphs = null;

        //    try
        //    {
        //        wordapp = new Word.Application { Visible = false };
        //        doc = wordapp.Documents.Open(wordPath);
        //        paragraphs = DocumentFormatter.RemoveBlankPages(doc, wordapp);

        //        SaveDocumentAsNewFileAndClose(doc);
        //        wordapp.Quit();
        //    }
        //    catch (Exception ex)
        //    {
        //        DisplayException(ex);
        //        return false;
        //    }
        //    finally
        //    {
        //        CleanUpUnmangedComResources(
        //            ref wordapp, ref doc, ref paragraphs);
        //    }

        //    return true;
        //}

        #endregion

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
