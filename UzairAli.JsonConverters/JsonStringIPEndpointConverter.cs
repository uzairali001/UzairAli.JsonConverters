using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace UzairAli.JsonConverters;

public class JsonStringIPEndPointConverter : JsonConverterFactory
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(IPEndPoint));
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return typeToConvert == typeof(IPEndPoint)
            ? new IPEndPointConverter()
            : new NullableIPEndPointConverter();
    }

    private class IPEndPointConverter : JsonConverter<IPEndPoint>
    {

        public override IPEndPoint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonObject jo = JsonNode.Parse(ref reader)!.AsObject();
            IPAddress address = IPAddress.Parse(jo["Address"]!.GetValue<string>());
            int port = jo["Port"]!.GetValue<int>();
            return new IPEndPoint(address, port);
        }

        public override void Write(Utf8JsonWriter writer, IPEndPoint value, JsonSerializerOptions options)
        {
            JsonObject jo = new()
            {
                { "Address", value.Address.ToString() },
                { "Port", value.Port }
            };
            jo.WriteTo(writer);
        }
    }

    private class NullableIPEndPointConverter : JsonConverter<IPEndPoint?>
    {
        public override IPEndPoint? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonObject? jo = JsonNode.Parse(ref reader)?.AsObject();
            if (jo is null)
            {
                return default;
            }
            if (!jo.TryGetPropertyValue("Address", out JsonNode? jsonNodeAddress) || jsonNodeAddress is null
                || !jo.TryGetPropertyValue("Port", out JsonNode? jsonNodePort) || jsonNodePort is null)
            {
                return default;
            }


            IPAddress address = IPAddress.Parse(jsonNodeAddress.ToString());
            int port = jsonNodePort.GetValue<int>();
            return new IPEndPoint(address, port);
        }

        public override void Write(Utf8JsonWriter writer, IPEndPoint? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }
            JsonObject jo = new()
            {
                { "Address", value.Address.ToString() },
                { "Port", value.Port }
            };
            jo.WriteTo(writer);
        }
    }
}
