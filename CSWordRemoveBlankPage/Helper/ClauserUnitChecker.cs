namespace Shuffler.Helper
{
    using Microsoft.Office.Interop.Word;

    public class ClauserUnitChecker : IUnitChecker
    {
        public bool IsValidUnit(Selection selection)
        {
            return 
                selection.Font.Superscript == 1
                   && selection.Text == "CS";
        }
    }
}
