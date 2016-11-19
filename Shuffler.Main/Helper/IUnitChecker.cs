namespace Shuffler.Helper
{
    using System.Xml;

    public interface IUnitChecker
    {
        /// <summary>
        /// Checks that a CS is an intended Clauser unit by 
        /// checking if it is in superscript
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        bool IsValidUnit(XmlNode xmlNode);
    }
}