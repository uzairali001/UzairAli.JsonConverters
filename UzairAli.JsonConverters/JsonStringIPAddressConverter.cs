using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UzairAli.JsonConverters;

public class JsonStringIPAddressConverter : JsonConverterFactory
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(IPAddress));
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return typeToConvert == typeof(IPAddress)
            ? new IPAddressConverter()
            : new NullableIPAddressConverter();
    }

    private class IPAddressConverter : JsonConverter<IPAddress>
    {
        public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return IPAddress.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    private class NullableIPAddressConverter : JsonConverter<IPAddress?>
    {
        public override IPAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? s = reader.GetString();
            return s is not null ? IPAddress.Parse(s) : default;
        }

        public override void Write(Utf8JsonWriter writer, IPAddress? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }
            writer.WriteStringValue(value.ToString());
        }
    }
}
