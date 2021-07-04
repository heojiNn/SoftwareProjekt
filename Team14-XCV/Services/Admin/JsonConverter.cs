using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace XCV.Data
{
    class SCategoryConverter : JsonConverter<SkillCategory>
    {
        public override SkillCategory Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("skills muss mit { beginnen");

            SkillCategory current = new();
            reader.Read();

            while (reader.TokenType != JsonTokenType.EndObject)
            {
                var name = reader.GetString().Trim();
                reader.Read();


                if (reader.TokenType == JsonTokenType.StartObject) // is a higher category
                {
                    var subCat = JsonSerializer.Deserialize<SkillCategory>(ref reader, options);
                    subCat.Name = name;
                    subCat.Parent = current;

                    current.Children.Add(subCat);
                }
                else                                            // is in a base category
                {
                    SkillCategory baseC = new();
                    baseC.Name = name;
                    baseC.Parent = current;
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        var s = JsonSerializer.Deserialize<Skill>(ref reader, options);
                        s.Category = baseC;
                        baseC.Children.Add(s);
                    }

                    current.Children.Add(baseC);
                }
                reader.Read();
            }
            return current;
        }


        public override void Write(Utf8JsonWriter writer, SkillCategory value, JsonSerializerOptions options)
        {
            if (value.Name.Equals(""))      //if highest level/ start of file
                writer.WriteStartObject();
            else
                writer.WriteStartObject(value.Name);


            foreach (SkillCategory child in value.Children)
            {
                if (!child.Children.Any())
                {
                    writer.WriteEndObject();
                    return;
                }
                if (child.Children.First() is Skill)
                {
                    writer.WriteStartArray(child.Name);
                    foreach (Skill s in child.Children)
                        JsonSerializer.Serialize(writer, s, options);
                    writer.WriteEndArray();
                }
                else
                {
                    JsonSerializer.Serialize(writer, child, options);
                }
            }
            writer.WriteEndObject();
        }
    }

    class SkillConverter : JsonConverter<Skill>
    {
        public override Skill Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new Skill() { Name = reader.GetString().Trim() };
        }

        public override void Write(Utf8JsonWriter writer, Skill value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
    }

}
