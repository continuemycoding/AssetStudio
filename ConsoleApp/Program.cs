using AssetStudio;
using System.IO;
using System.Drawing.Imaging;

namespace ConsoleApp
{
    class Program
    {
        private static bool ExportFileExists(string filename)
        {
            if (File.Exists(filename))
            {
                return true;
            }
            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            return false;
        }

        public static bool ExportTexture2D(Texture2D texture, string exportPathName)
        {
            var converter = new Texture2DConverter(texture);
            var bitmap = converter.ConvertToBitmap(true);
            if (bitmap == null)
                return false;
            ImageFormat format = null;
            var ext = "PNG";
            switch (ext)
            {
                case "BMP":
                    format = ImageFormat.Bmp;
                    break;
                case "PNG":
                    format = ImageFormat.Png;
                    break;
                case "JPEG":
                    format = ImageFormat.Jpeg;
                    break;
            }
            var exportFullName = exportPathName + texture.m_Name + "." + ext.ToLower();
            if (ExportFileExists(exportFullName))
                return false;
            bitmap.Save(exportFullName, format);
            bitmap.Dispose();
            return true;
        }

        public static bool ExportAudioClip(AudioClip audioClip, string exportPath)
        {
            var m_AudioData = audioClip.m_AudioData.Value;
            if (m_AudioData == null || m_AudioData.Length == 0)
                return false;
            var converter = new AudioClipConverter(audioClip);
            if (converter.IsFMODSupport)
            {
                var exportFullName = exportPath + audioClip.m_Name + ".wav";
                if (ExportFileExists(exportFullName))
                    return false;
                var buffer = converter.ConvertToWav();
                if (buffer == null)
                    return false;
                File.WriteAllBytes(exportFullName, buffer);
            }
            else
            {
                var exportFullName = exportPath + audioClip.m_Name + converter.GetExtensionName();
                if (ExportFileExists(exportFullName))
                    return false;
                File.WriteAllBytes(exportFullName, m_AudioData);
            }
            return true;
        }

        public static bool ExportTextAsset(TextAsset textAsset, string exportPath)
        {
            var exportFullName = exportPath + textAsset.m_Name + ".txt";//textAsset.Extension
            if (ExportFileExists(exportFullName))
                return false;
            File.WriteAllBytes(exportFullName, textAsset.m_Script);
            return true;
        }

        static void Main(string[] args)
        {
            var assetsManager = new AssetsManager();

            //assetsManager.LoadFiles(@"C:\Users\lin\Desktop\Data\f50e2c51e98086b40be703f1806860b9");
            //assetsManager.LoadFiles(@"C:\Users\lin\Desktop\Data\9147256f18c49e944a1ce8d4ec9fb0c7");

            assetsManager.LoadFolder(@"C:\Users\lin\Desktop\Data");
            
            foreach (var assetsFile in assetsManager.assetsFileList)
            {
                foreach (var asset in assetsFile.Objects.Values)
                {
                    var exportPath = @"C:\Users\lin\Desktop\Data\" + asset.type + @"\";

                    switch (asset)
                    {
                        case Texture2D texture:
                            ExportTexture2D(texture, exportPath);
                            break;
                        case AudioClip audioClip:
                            ExportAudioClip(audioClip, exportPath);
                            break;
                        case TextAsset textAsset:
                            ExportTextAsset(textAsset, exportPath);
                            break;
                    }
                }
            }
        }
    }
}
