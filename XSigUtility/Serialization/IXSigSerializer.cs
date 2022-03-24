using System.Collections.Generic;
using XSigUtilityLibrary.Tokens;

namespace XSigUtilityLibrary.Serialization
{
    /// <summary>
    /// Interface for XSig serialization/deserialization.
    /// </summary>
    public interface IXSigSerializer<T>
    {
        IEnumerable<XSigToken> Serialize(T obj);
        T Deserialize(IEnumerable<XSigToken> tokens);
    }
}