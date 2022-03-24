using System;
using Crestron.SimplSharp.CrestronIO;
using XSigUtilityLibrary.Serialization;

namespace XSigUtilityLibrary
{
    public static class XSigExtensions
    {
        /// <summary>
        /// Get bytes for an IXSigStateResolver object.
        /// </summary>
        /// <param name="serializer">XSig state resolver.</param>
        /// <param name="obj">Object to serialize as bytes.</param>
        /// <typeparam name="T">Object type.</typeparam>
        /// <returns>Bytes in XSig format for each token within the state representation.</returns>
        /// <exception cref="ArgumentNullException">Object or XSig serializer is null.</exception>
        public static byte[] GetBytes<T>(this IXSigSerializer<T> serializer, T obj)
        {
            if (serializer == null)
                throw new ArgumentNullException("serializer");

            return serializer.GetBytes(obj, 0);
        }

        /// <summary>
        /// Get bytes for an IXSigStateResolver object, with a specified offset.
        /// </summary>
        /// <param name="serializer">XSig state resolver.</param>
        /// <param name="obj">Object to serialize as bytes.</param>
        /// <param name="offset">Offset to which the data will be aligned.</param>
        /// <typeparam name="T">Object type.</typeparam>
        /// <returns>Bytes in XSig format for each token within the state representation.</returns>
        /// <exception cref="ArgumentNullException">Object or XSig serializer is null.</exception>
        public static byte[] GetBytes<T>(this IXSigSerializer<T> serializer, T obj, int offset)
        {
            if (serializer == null)
                throw new ArgumentNullException("serializer");

            var tokens = serializer.Serialize(obj);
            if (tokens == null) return new byte[0];
            using (var memoryStream = new MemoryStream())
            {
                using (var tokenWriter = new XSigTokenStreamWriter(memoryStream))
                    tokenWriter.SerializeToStream(obj, serializer, offset);

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Deserialize object from stream.
        /// </summary>
        /// <param name="reader">XSig token stream reader.</param>
        /// <param name="serializer">XSig serializer.</param>
        /// <typeparam name="T">Object type.</typeparam>
        /// <returns>Deserialized object.</returns>
        /// <exception cref="ArgumentNullException">Object or XSig serializer is null.</exception>
        public static T DeserializeFromStream<T>(this XSigTokenStreamReader reader, IXSigSerializer<T> serializer)
        {
            if (serializer == null)
                throw new ArgumentNullException("serializer");

            return serializer.Deserialize(reader.ReadAllXSigTokens());
        }

        /// <summary>
        /// Write the serialized XSig object to the stream.
        /// </summary>
        /// <param name="writer">XSig token stream writer.</param>
        /// <param name="obj">Object to serialize to the stream.</param>
        /// <param name="serializer">XSig serializer.</param>
        /// <typeparam name="T">Object type.</typeparam>
        /// <exception cref="ArgumentNullException">Object or XSig serializer is null.</exception>
        public static void SerializeToStream<T>(this XSigTokenStreamWriter writer, T obj, IXSigSerializer<T> serializer)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (serializer == null)
                throw new ArgumentNullException("serializer");

            writer.SerializeToStream(obj, serializer, 0);
        }

        /// <summary>
        /// Write the serialized XSig object to the stream.
        /// </summary>
        /// <param name="writer">XSig token stream writer.</param>
        /// <param name="obj">Object to serialize to the stream.</param>
        /// <param name="serializer">XSig serializer.</param>
        /// <param name="offset"></param>
        /// <typeparam name="T">Object type.</typeparam>
        /// <exception cref="ArgumentNullException">Object or XSig serializer is null.</exception>
        public static void SerializeToStream<T>(this XSigTokenStreamWriter writer, T obj, IXSigSerializer<T> serializer, int offset)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (serializer == null)
                throw new ArgumentNullException("serializer");

            var tokens = serializer.Serialize(obj);
            writer.WriteXSigData(tokens, offset);
        }
    }
}