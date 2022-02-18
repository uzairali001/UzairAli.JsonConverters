using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UzairAli.JsonConverters;
public class JsonStringDoubleConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(double) ||
            typeToConvert == typeof(double?);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return typeToConvert == typeof(double)
           ? new DoubleConverter()
           : new NullableDoubleConverter();
    }

    private class DoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return GetValue(ref reader) ?? default;
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString() ?? string.Empty);
        }
    }

    private class NullableDoubleConverter : JsonConverter<double?>
    {
        public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return GetValue(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }
            writer.WriteStringValue(value.Value.ToString());
        }
    }

    private static double? GetValue(ref Utf8JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => default,
            _ => string.IsNullOrEmpty(reader.GetString()) is false ? double.Parse(reader.GetString()!) : null,
        };
    }
}