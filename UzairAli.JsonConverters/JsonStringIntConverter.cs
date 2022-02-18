using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UzairAli.JsonConverters;
public class JsonStringIntConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(int) ||
            typeToConvert == typeof(int?);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return typeToConvert == typeof(int)
           ? new IntConverter()
           : new NullableIntConverter();
    }

    private class IntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return GetValue(ref reader) ?? default;
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    private class NullableIntConverter : JsonConverter<int?>
    {
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return GetValue(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }
            writer.WriteNumberValue(value.Value);
        }
    }

    private static int? GetValue(ref Utf8JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => default,
            JsonTokenType.Number => reader.GetInt32(),
            _ => string.IsNullOrEmpty(reader.GetString()) is false ? int.Parse(reader.GetString()!) : null,
        };
    }
}