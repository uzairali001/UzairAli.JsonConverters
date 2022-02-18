#if NET6_0_OR_GREATER
using System;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UzairAli.JsonConverters;

public sealed class JsonStringTimeOnlyConverter : JsonConverterFactory
{
    private readonly string _serializationFormat;
    public JsonStringTimeOnlyConverter(string? serializationFormat = null)
    {
        this._serializationFormat = serializationFormat ?? "HH:mm:ss";
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(TimeOnly) ||
            typeToConvert == typeof(TimeOnly?);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return typeToConvert == typeof(TimeOnly)
            ? new TimeOnlyConverter(_serializationFormat)
            : new NullableTimeOnlyConverter(_serializationFormat);
    }
    private class TimeOnlyConverter : JsonConverter<TimeOnly>
    {
        private readonly string _serializationFormat;
        public TimeOnlyConverter(string serializationFormat)
        {
            this._serializationFormat = serializationFormat;
        }

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeOnly.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_serializationFormat));
        }
    }

    private class NullableTimeOnlyConverter : JsonConverter<TimeOnly?>
    {
        private readonly string _serializationFormat;
        public NullableTimeOnlyConverter(string serializationFormat)
        {
            this._serializationFormat = serializationFormat;
        }

        public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.Null)
            {
                return default;
            }

            return TimeOnly.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value.Value.ToString(_serializationFormat));
        }
    }
}

#endif
