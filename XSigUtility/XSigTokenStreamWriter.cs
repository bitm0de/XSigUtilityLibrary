using System;
using System.Collections.Generic;
using Crestron.SimplSharp.CrestronIO;
using XSigUtilityLibrary.Tokens;

namespace XSigUtilityLibrary
{
    /// <summary>
    /// XSigToken stream writer.
    /// </summary>
    public sealed class XSigTokenStreamWriter : IDisposable
    {
        private readonly Stream _stream;
        private readonly bool _leaveOpen;

        /// <inheritdoc />
        /// <summary>
        /// XSigToken stream writer constructor.
        /// </summary>
        /// <param name="stream">Input stream to write to.</param>
        /// <exception cref="T:System.ArgumentNullException">Stream is null.</exception>
        /// <exception cref="T:System.ArgumentException">Stream cannot be written to.</exception>
        public XSigTokenStreamWriter(Stream stream)
            : this(stream, false) { }

        /// <summary>
        /// XSigToken stream writer constructor.
        /// </summary>
        /// <param name="stream">Input stream to write to.</param>
        /// <param name="leaveOpen">Determines whether to leave the stream open or not.</param>
        /// <exception cref="ArgumentNullException">Stream is null.</exception>
        /// <exception cref="ArgumentException">Stream cannot be written to.</exception>
        public XSigTokenStreamWriter(Stream stream, bool leaveOpen)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanWrite)
                throw new ArgumentException("The specified stream cannot be written to.");

            _stream = stream;
            _leaveOpen = leaveOpen;
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
            WriteXSigData(new[] { token }, offset);
        }

        /// <summary>
        /// Write an enumerable collection of XSigTokens to the stream.
        /// </summary>
        /// <param name="tokens">XSigToken objects.</param>
        public void WriteXSigData(IEnumerable<XSigToken> tokens)
        {
            WriteXSigData(tokens, 0);
        }

        /// <summary>
        /// Write an enumerable collection of XSigTokens to the stream.
        /// </summary>
        /// <param name="tokens">XSigToken objects.</param>
        /// <param name="offset">Index offset for each XSigToken.</param>
        public void WriteXSigData(IEnumerable<XSigToken> tokens, int offset)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "Offset must be greater than or equal to 0.");

            if (tokens != null)
            {
                foreach (XSigToken token in tokens)
                {
                    if (token == null) continue;
                    var bytes = token.GetTokenWithOffset(offset).GetBytes();
                    _stream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        /// Disposes of the internal stream if specified to not leave open.
        /// </summary>
        public void Dispose()
        {
            if (!_leaveOpen)
                _stream.Dispose();
        }
    }
}