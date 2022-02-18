using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UzairAli.JsonConverters;

public class JsonStringBooleanConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(bool) ||
            typeToConvert == typeof(bool?);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return typeToConvert == typeof(bool)
            ? new BooleanConverter()
            : new NullableBooleanConverter();
    }


    private class BooleanConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Null => default,
                JsonTokenType.True => true,
                JsonTokenType.False => false,
                JsonTokenType.Number => reader.GetInt32() is 1,
                _ => string.IsNullOrEmpty(reader.GetString()) is false && ParseBoolean(reader.GetString()),
            };
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }

    private class NullableBooleanConverter : JsonConverter<bool?>
    {
        public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Null => default,
                JsonTokenType.True => true,
                JsonTokenType.False => false,
                JsonTokenType.Number => reader.GetInt32() is 1,
                _ => string.IsNullOrEmpty(reader.GetString()) is false && ParseBoolean(reader.GetString()),
            };
        }

        public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }
            writer.WriteBooleanValue(value is true);
        }


    }

    private static bool ParseBoolean(string? value)
    {
        return value?.ToLower().Trim() switch
        {
            null => default,
            "true" or "yes" or "y" or "1" or "t" => true,
            _ => false,
        };
    }
}
