using System;
using System.IO;
using Utf8Json.Internal;

namespace Utf8Json
{
    // NonGeneric API
    public static partial class JsonSerializer
    {
        public static class NonGeneric
        {
            static readonly Func<Type, CompiledMethods> CreateCompiledMethods;
            static readonly ThreadsafeTypeKeyHashTable<CompiledMethods> serializes = new ThreadsafeTypeKeyHashTable<CompiledMethods>(capacity: 64);

            delegate void SerializeJsonWriter(ref JsonWriter writer, object value, IJsonFormatterResolver resolver);
            delegate object DeserializeJsonReader(ref JsonReader reader, IJsonFormatterResolver resolver);

            static NonGeneric()
            {
                CreateCompiledMethods = t => new CompiledMethods(t);
            }

            static CompiledMethods GetOrAdd(Type type)
            {
                return serializes.GetOrAdd(type, CreateCompiledMethods);
            }

            /// <summary>
            /// Serialize to binary with default resolver.
            /// </summary>
            public static byte[] Serialize(object value)
            {
                if (value == null) return Serialize<object>(value);
                return Serialize(value.GetType(), value, defaultResolver);
            }

            /// <summary>
            /// Serialize to binary with default resolver.
            /// </summary>
            public static byte[] Serialize(Type type, object value)
            {
                return Serialize(type, value, defaultResolver);
            }

            /// <summary>
            /// Serialize to binary with specified resolver.
            /// </summary>
            public static byte[] Serialize(object value, IJsonFormatterResolver resolver)
            {
                if (value == null) return Serialize<object>(value, resolver);
                return Serialize(value.GetType(), value, resolver);
            }

            /// <summary>
            /// Serialize to binary with specified resolver.
            /// </summary>
            public static byte[] Serialize(Type type, object value, IJsonFormatterResolver resolver)
            {
                return GetOrAdd(type).serialize1.Invoke(value, resolver);
            }

            /// <summary>
            /// Serialize to stream.
            /// </summary>
            public static void Serialize(Stream stream, object value)
            {
                if (value == null) { Serialize<object>(stream, value); return; }
                Serialize(value.GetType(), stream, value, defaultResolver);
            }

            /// <summary>
            /// Serialize to stream.
            /// </summary>
            public static void Serialize(Type type, Stream stream, object value)
            {
                Serialize(type, stream, value, defaultResolver);
            }

            /// <summary>
            /// Serialize to stream with specified resolver.
            /// </summary>
            public static void Serialize(Stream stream, object value, IJsonFormatterResolver resolver)
            {
                if (value == null) { Serialize<object>(stream, value, resolver); return; }
                Serialize(value.GetType(), stream, value, resolver);
            }

            /// <summary>
            /// Serialize to stream with specified resolver.
            /// </summary>
            public static void Serialize(Type type, Stream stream, object value, IJsonFormatterResolver resolver)
            {
                GetOrAdd(type).serialize2.Invoke(stream, value, resolver);
            }

            public static void Serialize(ref JsonWriter writer, object value, IJsonFormatterResolver resolver)
            {
                if (value == null)
                {
                    writer.WriteNull();
                    return;
                }
                else
                {
                    Serialize(value.GetType(), ref writer, value, resolver);
                }
            }

            public static void Serialize(Type type, ref JsonWriter writer, object value)
            {
                Serialize(type, ref writer, value, defaultResolver);
            }

            public static void Serialize(Type type, ref JsonWriter writer, object value, IJsonFormatterResolver resolver)
            {
                GetOrAdd(type).serialize3.Invoke(ref writer, value, resolver);
            }

            /// <summary>
            /// Serialize to binary. Get the raw memory pool byte[]. The result can not share across thread and can not hold, so use quickly.
            /// </summary>
            public static ArraySegment<byte> SerializeUnsafe(object value)
            {
                if (value == null) return SerializeUnsafe<object>(value);
                return SerializeUnsafe(value.GetType(), value);
            }

            /// <summary>
            /// Serialize to binary. Get the raw memory pool byte[]. The result can not share across thread and can not hold, so use quickly.
            /// </summary>
            public static ArraySegment<byte> SerializeUnsafe(Type type, object value)
            {
                return SerializeUnsafe(type, value, defaultResolver);
            }

            /// <summary>
            /// Serialize to binary with specified resolver. Get the raw memory pool byte[]. The result can not share across thread and can not hold, so use quickly.
            /// </summary>
            public static ArraySegment<byte> SerializeUnsafe(object value, IJsonFormatterResolver resolver)
            {
                if (value == null) return SerializeUnsafe<object>(value);
                return SerializeUnsafe(value.GetType(), value, resolver);
            }

            /// <summary>
            /// Serialize to binary with specified resolver. Get the raw memory pool byte[]. The result can not share across thread and can not hold, so use quickly.
            /// </summary>
            public static ArraySegment<byte> SerializeUnsafe(Type type, object value, IJsonFormatterResolver resolver)
            {
                return GetOrAdd(type).serializeUnsafe.Invoke(value, resolver);
            }

            /// <summary>
            /// Serialize to JsonString.
            /// </summary>
            public static string ToJsonString(object value)
            {
                if (value == null) return "null";
                return ToJsonString(value.GetType(), value);
            }

            /// <summary>
            /// Serialize to JsonString.
            /// </summary>
            public static string ToJsonString(Type type, object value)
            {
                return ToJsonString(type, value, defaultResolver);
            }

            /// <summary>
            /// Serialize to JsonString with specified resolver.
            /// </summary>
            public static string ToJsonString(object value, IJsonFormatterResolver resolver)
            {
                if (value == null) return "null";
                return ToJsonString(value.GetType(), value, resolver);
            }

            /// <summary>
            /// Serialize to JsonString with specified resolver.
            /// </summary>
            public static string ToJsonString(Type type, object value, IJsonFormatterResolver resolver)
            {
                return GetOrAdd(type).toJsonString.Invoke(value, resolver);
            }

            public static object Deserialize(Type type, string json)
            {
                return Deserialize(type, json, defaultResolver);
            }

            public static object Deserialize(Type type, string json, IJsonFormatterResolver resolver)
            {
                return GetOrAdd(type).deserialize1.Invoke(json, resolver);
            }

            public static object Deserialize(Type type, byte[] bytes)
            {
                return Deserialize(type, bytes, defaultResolver);
            }

            public static object Deserialize(Type type, byte[] bytes, IJsonFormatterResolver resolver)
            {
                return Deserialize(type, bytes, 0, defaultResolver);
            }

            public static object Deserialize(Type type, byte[] bytes, int offset)
            {
                return Deserialize(type, bytes, offset, defaultResolver);
            }

            public static object Deserialize(Type type, byte[] bytes, int offset, IJsonFormatterResolver resolver)
            {
                return GetOrAdd(type).deserialize2.Invoke(bytes, offset, resolver);
            }

            public static object Deserialize(Type type, Stream stream)
            {
                return Deserialize(type, stream, defaultResolver);
            }

            public static object Deserialize(Type type, Stream stream, IJsonFormatterResolver resolver)
            {
                return GetOrAdd(type).deserialize3.Invoke(stream, resolver);
            }

            public static object Deserialize(Type type, ref JsonReader reader)
            {
                return Deserialize(type, ref reader, defaultResolver);
            }

            public static object Deserialize(Type type, ref JsonReader reader, IJsonFormatterResolver resolver)
            {
                return GetOrAdd(type).deserialize4.Invoke(ref reader, resolver);
            }

            class CompiledMethods
            {
                public readonly Func<object, IJsonFormatterResolver, byte[]> serialize1;
                public readonly Action<Stream, object, IJsonFormatterResolver> serialize2;
                public readonly SerializeJsonWriter serialize3;
                public readonly Func<object, IJsonFormatterResolver, ArraySegment<byte>> serializeUnsafe;
                public readonly Func<object, IJsonFormatterResolver, string> toJsonString;
                public readonly Func<string, IJsonFormatterResolver, object> deserialize1;
                public readonly Func<byte[], int, IJsonFormatterResolver, object> deserialize2;
                public readonly Func<Stream, IJsonFormatterResolver, object> deserialize3;
                public readonly DeserializeJsonReader deserialize4;

                public CompiledMethods(Type type)
                {
                }
            }
        }
    }
}
