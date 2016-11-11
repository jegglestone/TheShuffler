namespace Shuffler.Helper
{
    public interface IUnitChecker
    {
        /// <summary>
        /// Checks that a CS is an intended Clauser unit by checking if it is in superscript
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
        bool IsValidUnit(Microsoft.Office.Interop.Word.Selection selection);
    }
}