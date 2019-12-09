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

        public abstract byte[] GetBytes();
        public abstract XSigToken GetTokenWithOffset(int offset);
    }
}