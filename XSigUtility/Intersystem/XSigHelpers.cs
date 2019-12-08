using System;
using System.Linq;
using XSigUtilityLibrary.Intersystem.Tokens;

namespace XSigUtilityLibrary.Intersystem
{
    /// <summary>
    /// Helper methods for creating XSig byte sequences compatible with the Intersystem Communications (ISC) symbol.
    /// </summary>
    /// <remarks>
    /// Indexing is not from the start of each signal type but rather from the beginning of the first defined signal
    /// the Intersystem Communications (ISC) symbol.
    /// </remarks>
    internal static class XSigHelpers
    {
        /// <summary>
        /// Forces all outputs to 0.
        /// </summary>
        /// <returns>Bytes in XSig format for clear outputs trigger.</returns>
        public static byte[] ClearOutputs()
        {
            return new byte[] { 0xFC };
        }

        /// <summary>
        /// Evaluate all inputs and re-transmit any digital, analog, and permanent serail signals not set to 0.
        /// </summary>
        /// <returns>Bytes in XSig format for send status trigger.</returns>
        public static byte[] SendStatus()
        {
            return new byte[] { 0xFD };
        }

        /// <summary>
        /// Get bytes for a single digital signal.
        /// </summary>
        /// <param name="index">1-based digital index</param>
        /// <param name="value">Digital data to be encoded</param>
        /// <returns>Bytes in XSig format for digtial information.</returns>
        /// <exception cref="ArgumentException">Index out of range for digital information encoded in XSig format.</exception>
        public static byte[] GetBytes(int index, bool value)
        {
            return new XSigDigitalToken(index, value).GetBytes();
        }

        /// <summary>
        /// Get byte sequence for multiple digital signals.
        /// </summary>
        /// <param name="startIndex">Starting index of the sequence.</param>
        /// <param name="values">Digital signal value array.</param>
        /// <returns>Byte sequence in XSig format for digital signal information.</returns>
        public static byte[] GetBytes(int startIndex, bool[] values)
        {
            // Digital XSig data is 2 bytes per value
            const int fixedLength = 2;
            var bytes = new byte[values.Length * fixedLength];
            for (var i = 0; i < values.Length; i++) Buffer.BlockCopy(GetBytes(startIndex++, values[i]), 0, bytes, i * fixedLength, fixedLength);
            return bytes;
        }

        /// <summary>
        /// Get bytes for a single analog signal.
        /// </summary>
        /// <param name="index">1-based analog index</param>
        /// <param name="value">Analog data to be encoded</param>
        /// <returns>Bytes in XSig format for analog signal information.</returns>
        /// <exception cref="ArgumentException">Index out of range for analog information encoded in XSig format.</exception>
        public static byte[] GetBytes(int index, ushort value)
        {
            return new XSigAnalogToken(index, value).GetBytes();
        }

        /// <summary>
        /// Get byte sequence for multiple analog signals.
        /// </summary>
        /// <param name="startIndex">Starting index of the sequence.</param>
        /// <param name="values">Analog signal value array.</param>
        /// <returns>Byte sequence in XSig format for analog signal information.</returns>
        public static byte[] GetBytes(int startIndex, ushort[] values)
        {
            // Analog XSig data is 4 bytes per value
            const int fixedLength = 4;
            var bytes = new byte[values.Length * fixedLength];
            for (var i = 0; i < values.Length; i++) Buffer.BlockCopy(GetBytes(startIndex++, values[i]), 0, bytes, i * fixedLength, fixedLength);
            return bytes;
        }

        /// <summary>
        /// Get bytes for a single serial signal.
        /// </summary>
        /// <param name="index">1-based serial index</param>
        /// <param name="value">Serial data to be encoded</param>
        /// <returns>Bytes in XSig format for serial signal information.</returns>
        /// <exception cref="ArgumentException">Index out of range for serial information encoded in XSig format.</exception>
        public static byte[] GetBytes(int index, string value)
        {
            return new XSigSerialToken(index, value).GetBytes();
        }

        /// <summary>
        /// Get byte sequence for multiple serial signals.
        /// </summary>
        /// <param name="startIndex">Starting index of the sequence.</param>
        /// <param name="values">Serial signal value array.</param>
        /// <returns>Byte sequence in XSig format for serial signal information.</returns>
        public static byte[] GetBytes(int startIndex, string[] values)
        {
            // Serial XSig data is not fixed-length like the other formats
            var offset = 0;
            var bytes = new byte[values.Sum(v => v.Length + 3)];
            for (var i = 0; i < values.Length; i++) {
                var data = GetBytes(startIndex++, values[i]);
                Buffer.BlockCopy(data, 0, bytes, offset, data.Length);
                offset += data.Length;
            }
            return bytes;
        }
    }
}