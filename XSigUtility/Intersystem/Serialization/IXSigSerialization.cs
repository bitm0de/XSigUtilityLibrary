using System.Collections.Generic;
using XSigUtilityLibrary.Intersystem.Tokens;

namespace XSigUtilityLibrary.Intersystem.Serialization
{
    /// <summary>
    /// Interface to determine XSig serialization for an object.
    /// </summary>
    public interface IXSigSerialization
    {
        IEnumerable<XSigToken> Serialize();
        T Deserialize<T>(IEnumerable<XSigToken> tokens) where T : class, IXSigSerialization;
    }
}