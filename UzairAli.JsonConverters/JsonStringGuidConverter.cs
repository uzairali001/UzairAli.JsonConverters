using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UzairAli.JsonConverters;
public class JsonStringGuidConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(Guid) ||
            typeToConvert == typeof(Guid?);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return typeToConvert == typeof(Guid)
           ? new GuidConverter()
           : new NullableGuidConverter();
    }

    private class GuidConverter : JsonConverter<Guid>
    {
        public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return GetValue(ref reader) ?? default;
        }

        public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString() ?? string.Empty);
        }
    }

    private class NullableGuidConverter : JsonConverter<Guid?>
    {
        public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return GetValue(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }
            writer.WriteStringValue(value.ToString());
        }
    }

    private static Guid? GetValue(ref Utf8JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => null,
            _ => string.IsNullOrEmpty(reader.GetString()) is false ? new Guid(reader.GetString()!) : null,
        };
    }
}