using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Team14.Data
{
    public class BaseData
    {
        public List<string> fields { get; set; }
        public List<string> roles { get; set; }
        public List<string> languages { get; set; }


        [JsonConverter(typeof(quickDirtySomeSkills))]
        public Dictionary<string, List<string>> skills { get; set; }
        public List<string> Softskills { get; set; }

    }





    class quickDirtySomeSkills : JsonConverter<Dictionary<string, List<string>>>
    {
        public override Dictionary<string, List<string>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<string> sprachen = new();
            List<string> frameworks = new();
            List<string> bibliotheken = new();

            var result = new Dictionary<string, List<string>>();


            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("1");
            reader.Read();

            if (reader.GetString() != "Sprachen und Frameworks")
                throw new JsonException("2");
            reader.Read();
            reader.Read();

            if (reader.GetString() != "Sprachen")
                throw new JsonException("3");
            reader.Read();

            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                sprachen.Add(reader.GetString());
            result.Add("Sprachen", sprachen);

            reader.Read();
            reader.Read();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                frameworks.Add(reader.GetString());
            result.Add("Frameworks", frameworks);

            reader.Read();
            reader.Read();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                bibliotheken.Add(reader.GetString());
            result.Add("Bibliotheken", bibliotheken);

            reader.Read();
            if (reader.TokenType != JsonTokenType.EndObject)
                throw new JsonException("4");

            //  Skip the rest
            int inner = 0;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                    inner++;
                if (reader.TokenType == JsonTokenType.EndObject)
                    inner--;
                if (inner == -1)
                    break;
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<string, List<string>> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }


}
