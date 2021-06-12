using System;
using System.Collections.Generic;
using System.Linq;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Team14.Data
{
    class InnerNodeConverter : JsonConverter<BasicDataNode>
    {
        public override BasicDataNode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<BasicDataLeaf> elements = new();              //every category has sub-categorys
            List<BasicDataNode> children = new();   //or is a final one
            string longName = "";
            List<string> levelNames = new();
            List<SkillExtensionValue> extensions = new();

            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("JSON muss mit { beginnen");

            reader.Read();
            if (reader.GetString() == "long")
            {
                reader.Read();
                longName = reader.GetString();
                reader.Read();
            }
            if (reader.GetString() == "extensions")
            {
                reader.Read();
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    extensions.Add(Enum.Parse<SkillExtensionValue>(reader.GetString()));
                reader.Read();
            }

            if (reader.GetString() == "levels")
            {
                reader.Read();
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    levelNames.Add(reader.GetString());
                reader.Read();
            }

            // and the last property
            if (reader.GetString() == "elements")
            {
                reader.Read();
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    elements.Add(JsonSerializer.Deserialize<BasicDataLeaf>(ref reader, options));
            }
            else
            {
                while (reader.TokenType != JsonTokenType.EndObject)
                {
                    string categoryName = reader.GetString();
                    reader.Read();
                    if (reader.TokenType != JsonTokenType.StartObject)
                        throw new JsonException($"Objekt-Schachtel Anfang bei {categoryName}");
                    var subCat = JsonSerializer.Deserialize<BasicDataNode>(ref reader, options);
                    // the advantage throu init (good domain) outweigh the extra copy work
                    children.Add(new BasicDataNode()
                    {
                        Name = categoryName,
                        LongName = subCat.LongName,
                        Children = subCat.Children,
                        Extensions = subCat.Extensions,
                        LevelNames = subCat.LevelNames
                    });
                    reader.Read();
                }
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.EndObject)
                throw new JsonException($"Objekt-Schachtel Ende");
            else
            {
                return new BasicDataNode()
                {
                    LongName = longName,
                    LevelNames = levelNames,
                    Extensions = extensions,
                    Children = elements.Count == 0 ? children : elements
                };
            }


        }


        public override void Write(Utf8JsonWriter writer, BasicDataNode value, JsonSerializerOptions options)
        {
            if (value.Name.Equals(""))
                writer.WriteStartObject();
            else
                writer.WriteStartObject(value.Name);    // Categorie shortName

            if (!value.LongName.Equals(""))
            {
                writer.WriteString("long", value.LongName);
            }

            if (value.Extensions.Contains(SkillExtensionValue.Periode))
            {
                writer.WriteStartArray("extensions");
                writer.WriteStringValue(SkillExtensionValue.Periode.ToString());
                writer.WriteEndArray();
            }
            if (value.LevelNames.Count() != 0)
            {
                writer.WriteStartArray("levels");
                foreach (var lvl in value.LevelNames)
                    writer.WriteStringValue(lvl);
                writer.WriteEndArray();
            }


            if (value.Children.First() is BasicDataNode)
            {
                foreach (BasicDataNode c in value.Children)   // one Level deeper
                    JsonSerializer.Serialize(writer, c, options);
            }
            else if (value.Children.First() is BasicDataLeaf && !(value.Children.First() is BasicDataNode))
            {
                writer.WriteStartArray("elements");
                foreach (BasicDataLeaf s in value.Children)
                    JsonSerializer.Serialize(writer, s, options);
                writer.WriteEndArray();
            }
            else
                throw new Exception("Tree Structure Error");
            writer.WriteEndObject();
        }
    }



    class LeafConvert : JsonConverter<BasicDataLeaf>
    {
        public override BasicDataLeaf Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new BasicDataLeaf() { Name = reader.GetString() };
        }

        public override void Write(Utf8JsonWriter writer, BasicDataLeaf value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
    }

}
