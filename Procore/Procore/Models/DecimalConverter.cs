using Newtonsoft.Json;

namespace Procore.Models
{
    public class DecimalConverter : JsonConverter<decimal>
    {
        public override decimal ReadJson(JsonReader reader, Type objectType, decimal existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Integer)
            {
                return Convert.ToDecimal(reader.Value);
            }

            if (reader.TokenType == JsonToken.String && decimal.TryParse((string)reader.Value, out decimal parsedValue))
            {
                return parsedValue;
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, decimal value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}
