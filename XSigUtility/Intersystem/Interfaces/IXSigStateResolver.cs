using System.Collections.Generic;
using XSigUtilityLibrary.Intersystem.Tokens;

namespace XSigUtilityLibrary.Intersystem.Interfaces
{
    public interface IXSigStateResolver
    {
        IEnumerable<XSigToken> GetXSigState();
    }
}