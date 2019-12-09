using System;

namespace XSigUtilityLibrary.Intersystem.Tokens
{
    public sealed class XSigAnalogToken : XSigToken
    {
        private readonly ushort _value;

        public XSigAnalogToken(int index, ushort value)
            : base(index)
        {
            _value = value;
        }

        public ushort Value {
            get { return _value; }
        }

        public override XSigTokenType TokenType {
            get { return XSigTokenType.Analog; }
        }

        public override byte[] GetBytes()
        {
            // 10-bits available for analog encoded data
            if (Index >= 1024 || Index < 0)
                throw new ArgumentException("index");

            return new[] {
                (byte)(0xC0 | ((Value & 0xC000) >> 10) | (Index >> 7)),
                (byte)((Index - 1) & 0x7F),
                (byte)((Value & 0x3F80) >> 7),
                (byte)(Value & 0x7F)
            };
        }

        public override XSigToken GetTokenWithOffset(int offset)
        {
            return new XSigAnalogToken(Index + offset, Value);
        }

        public override string ToString()
        {
            return "0x" + Value.ToString("X4");
        }
    }
}