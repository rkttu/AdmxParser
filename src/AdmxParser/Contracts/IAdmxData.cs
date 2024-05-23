using System.Xml.Linq;

namespace AdmxParser.Contracts
{
    public interface IAdmxData
    {
        AdmxContent Parent { get; }
        XElement SourceElement { get; }
    }

}
