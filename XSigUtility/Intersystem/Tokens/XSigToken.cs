using System;

namespace XSigUtilityLibrary.Intersystem.Tokens
{
    public abstract class XSigToken
    {
        private readonly int _index;

        protected XSigToken(int index)
        {
            _index = index;
        }

        /// <summary>
        /// XSig 1-based index.
        /// </summary>
        public int Index {
            get { return _index; }
        }

        /// <summary>
        /// XSigToken type.
        /// </summary>
        public abstract XSigTokenType TokenType { get; }

        /// <summary>
        /// Generates the XSig bytes for the corresponding token.
        /// </summary>
        /// <returns>XSig byte array.</returns>
        public abstract byte[] GetBytes();

        public abstract XSigToken GetTokenWithOffset(int offset);
    }
}