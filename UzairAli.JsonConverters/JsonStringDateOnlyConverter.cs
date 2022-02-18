#if NET6_0_OR_GREATER
using System;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UzairAli.JsonConverters;

public sealed class JsonStringDateOnlyConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(DateOnly) ||
            typeToConvert == typeof(DateOnly?);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return typeToConvert == typeof(DateOnly)
            ? new DateOnlyConverter()
            : new NullableDateOnlyConverter();
    }

    private class DateOnlyConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ParseStringToDateOnly(ref reader) ?? default;
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(ConvertStringToDateOnly(value) ?? default);
        }
    }

    private class NullableDateOnlyConverter : JsonConverter<DateOnly?>
    {
        public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ParseStringToDateOnly(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(ConvertStringToDateOnly(value));
        }
    }

    private static DateOnly? ParseStringToDateOnly(ref Utf8JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => default,
            _ => DateOnly.Parse(reader.GetString()),
        };
    }

    private static string? ConvertStringToDateOnly(DateOnly? value)
    {
        if (value is null)
        {
            return default;
        }

        return value.Value.ToString("O");
    }

}
#endif
