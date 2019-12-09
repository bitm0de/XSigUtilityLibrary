using System;
using System.Collections.Generic;
using Crestron.SimplSharp.CrestronIO;
using XSigUtilityLibrary.Intersystem.Interfaces;
using XSigUtilityLibrary.Intersystem.Tokens;

namespace XSigUtilityLibrary.Intersystem
{
    /// <summary>
    /// XSigToken stream writer.
    /// </summary>
    public sealed class XSigTokenStreamWriter : IDisposable
    {
        private readonly Stream _stream;

        /// <summary>
        /// XSigToken stream writer constructor.
        /// </summary>
        /// <param name="stream">Input stream to write to.</param>
        /// <exception cref="ArgumentNullException">Stream is null.</exception>
        /// <exception cref="ArgumentException">Stream cannot be written to.</exception>
        public XSigTokenStreamWriter(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanWrite)
                throw new ArgumentException("The specified stream cannot be written to.");

            _stream = stream;
        }

        /// <summary>
        /// Write XSig data gathered from an IXSigStateResolver to the stream.
        /// </summary>
        /// <param name="xSigStateResolver">IXSigStateResolver object.</param>
        public void WriteXSigData(IXSigStateResolver xSigStateResolver)
        {
            WriteXSigData(xSigStateResolver, 0);
        }

        /// <summary>
        /// Write XSig data gathered from an IXSigStateResolver to the stream.
        /// </summary>
        /// <param name="xSigStateResolver">IXSigStateResolver object.</param>
        /// <param name="offset">Index offset for each XSigToken.</param>
        public void WriteXSigData(IXSigStateResolver xSigStateResolver, int offset)
        {
            var tokens = xSigStateResolver.GetXSigState();
            WriteXSigData(tokens, offset);
        }

        /// <summary>
        /// Write XSigToken to the stream.
        /// </summary>
        /// <param name="token">XSigToken object.</param>
        public void WriteXSigData(XSigToken token)
        {
            WriteXSigData(token, 0);
        }

        /// <summary>
        /// Write XSigToken to the stream.
        /// </summary>
        /// <param name="token">XSigToken object.</param>
        /// <param name="offset">Index offset for each XSigToken.</param>
        public void WriteXSigData(XSigToken token, int offset)
        {
            WriteXSigData(new [] { token }, offset);
        }

        /// <summary>
        /// Write an enumerable collection of XSigToken's to the stream.
        /// </summary>
        /// <param name="tokens">XSigToken objects.</param>
        public void WriteXSigData(IEnumerable<XSigToken> tokens)
        {
            WriteXSigData(tokens, 0);
        }

        /// <summary>
        /// Write an enumerable collection of XSigToken's to the stream.
        /// </summary>
        /// <param name="tokens">XSigToken objects.</param>
        /// <param name="offset">Index offset for each XSigToken.</param>
        public void WriteXSigData(IEnumerable<XSigToken> tokens, int offset)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "Offset must be greater than or equal to 0.");

            if (offset != 0) {
                // Offset is 0, so the current index is still value (avoid copying objects).
                foreach (var token in tokens) {
                    var bytes = token.GetBytes();
                    _stream.Write(bytes, 0, bytes.Length);
                }
            } else {
                // Create a copy of the immutable object with a new offset.
                foreach (var token in tokens) {
                    var bytes = token.GetTokenWithOffset(offset).GetBytes();
                    _stream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}