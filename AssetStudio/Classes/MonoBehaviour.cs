using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetStudio
{
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

        public void ExportUISpriteData()
        {
            var position = reader.Position;

            reader.Position = namePosition + 12;

            var length = reader.ReadInt32();

            for(var i=0;i<length;i++)
            {
                var name = reader.ReadAlignedString();

                var x = reader.ReadInt32();
                var y = reader.ReadInt32();
                var width = reader.ReadInt32();
                var height = reader.ReadInt32();

                var borderLeft = reader.ReadInt32();
                var borderRight = reader.ReadInt32();
                var borderTop = reader.ReadInt32();
                var borderBottom = reader.ReadInt32();

                var paddingLeft = reader.ReadInt32();
                var paddingRight = reader.ReadInt32();
                var paddingTop = reader.ReadInt32();
                var paddingBottom = reader.ReadInt32();

                Console.WriteLine(name + " " + x + " " + y + " " + width + " " + height);
            }

            reader.Position = position;
        }
    }
}
