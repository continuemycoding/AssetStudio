using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AssetStudio
{
    [System.Serializable]
    public class UISpriteData
    {
        public string name = "Sprite";
        public int x = 0;
        public int y = 0;
        public int width = 0;
        public int height = 0;

        public int borderLeft = 0;
        public int borderRight = 0;
        public int borderTop = 0;
        public int borderBottom = 0;

        public int paddingLeft = 0;
        public int paddingRight = 0;
        public int paddingTop = 0;
        public int paddingBottom = 0;

        //bool rotated = false;
    }

    public sealed class MonoBehaviour : Behaviour
    {
        public PPtr<MonoScript> m_Script;
        public string m_Name;

        private long namePosition;

        public MonoBehaviour(ObjectReader reader) : base(reader)
        {
            m_Script = new PPtr<MonoScript>(reader);
            m_Name = reader.ReadAlignedString();

            namePosition = reader.Position;
        }

        public void ExportUISpriteData(string atlasName)
        {
            if (namePosition + 12 > reader.byteStart + reader.byteSize)
                return;

            var position = reader.Position;

            reader.Position = namePosition + 12;

            var length = reader.ReadInt32();

            var list = new List<UISpriteData>();

            for (var i=0;i<length;i++)
            {
                var data = new UISpriteData
                {
                    name = reader.ReadAlignedString(),

                    x = reader.ReadInt32(),
                    y = reader.ReadInt32(),
                    width = reader.ReadInt32(),
                    height = reader.ReadInt32(),

                    borderLeft = reader.ReadInt32(),
                    borderRight = reader.ReadInt32(),
                    borderTop = reader.ReadInt32(),
                    borderBottom = reader.ReadInt32(),

                    paddingLeft = reader.ReadInt32(),
                    paddingRight = reader.ReadInt32(),
                    paddingTop = reader.ReadInt32(),
                    paddingBottom = reader.ReadInt32()
                };

                list.Add(data);
            }

            var json = JsonMapper.ToJson(list);
            
            Console.WriteLine(json);

            if (!Directory.Exists("Json"))
                Directory.CreateDirectory("Json");

            File.WriteAllText("Json/" + atlasName + ".json", json);

            reader.Position = position;
        }
    }
}
