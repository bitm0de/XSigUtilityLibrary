using System;

namespace XSigUtilityLibrary.Intersystem.Tokens
{
    public sealed class XSigDigitalToken : XSigToken
    {
        private readonly bool _value;

        public XSigDigitalToken(int index, bool value)
            : base(index)
        {
            _value = value;
        }

        public bool Value {
            get { return _value; }
        }

        public override XSigTokenType TokenType {
            get { return XSigTokenType.Digital; }
        }

        public override byte[] GetBytes()
        {
            // 12-bits available for digital encoded data
            if (Index >= 4096 || Index < 0)
                throw new ArgumentException("index");
            
            return new[] {
                (byte)(0x80 | (Value ? 0 : 0x20) | (Index >> 7)),
                (byte)((Index - 1) & 0x7F)
            };
        }

        public override string ToString()
        {
            return Value ? "High" : "Low";
        }
    }
}