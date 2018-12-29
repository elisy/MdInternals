using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;

using Elisy.MdInternals;
using Elisy.MdInternals.Cf;
using Elisy.MdInternals.Serialization;

namespace Elisy.MdInternals.CfProject
{
    public enum ProjectType
    {
        Xml,
        String
    }

    public class CfProject
    {
        private CfProjectSettings _settings;

        public void Save(MetadataPackage package, string folder, ProjectType projectType)
        {
            _settings = new CfProjectSettings() { Type = projectType };
            package.MetadataObjects.AsParallel().ForAll(m => UnloadObject(folder, m as ImageRowElement, projectType));

            var settings = new XmlWriterSettings() { Indent = true };
            using (XmlWriter xmlWriter = XmlWriter.Create(folder + "/" + package.PackagePart.GeneralProperties.Name + ".cfproj", settings))
            {
                System.Windows.Markup.XamlWriter.Save(_settings, xmlWriter);
            }
        }

        private Uri GetTargetFolder(ImageRowElement o)
        {
            if ((o is RootPointer) || (o is Configuration) || (o is Version) || (o is Versions))
                return new Uri("/", UriKind.Relative);
            else if (o is MetadataPart)
            {
                string path = "/" + o.GetType().Name;
                //if (((MetadataPart)o).GeneralProperties != null)
                //    path = path + "/" + ((MetadataPart)o).GeneralProperties.Name;
                path = path + "/";
                return new Uri(path, UriKind.Relative);
            }
            else if (o is UnresolvedPart)
                return new Uri("/Unresolved/", UriKind.Relative);
            else
                return new Uri("", UriKind.Relative);
        }

        private void UnloadObject(string folder, ImageRowElement o, ProjectType projectType)
        {
            string subFolder = folder + GetTargetFolder(o).ToString() + "\\";
            if (!Directory.Exists(subFolder))
                Directory.CreateDirectory(subFolder);

            string extension = "";
            if (o.ImageRow.BodyType == ImageRowTypes.CompressedMoxcel)
                extension = ".mxl";
            else if (o.ImageRow.BodyType == ImageRowTypes.CompressedUtf8MarkedString)
                extension = ".txt";
            else if (o.ImageRow.BodyType == ImageRowTypes.Utf8MarkedString)
                extension = ".txt";
            else if (o.ImageRow.BodyType == ImageRowTypes.CompressedImage)
                extension = ".img";

            if (o is MetadataPart)
            {
                if (projectType == ProjectType.String)
                    extension = ".txt";
                else
                    extension = ".xml";
            }

            var fileName = o.ImageRow.FileName;
            if ((o is MetadataPart) && ((MetadataPart)o).GeneralProperties != null)
                fileName = ((MetadataPart)o).GeneralProperties.Name;

            Uri relativeUri = new Uri(GetTargetFolder(o).ToString() + fileName + extension, UriKind.Relative);

            string filePath = subFolder + "\\" + fileName + extension;

            if (o.ImageRow.BodyType == ImageRowTypes.CompressedImage)
            {
                using (BinaryWriter binWriter =
                    new BinaryWriter(System.IO.File.Open(filePath, FileMode.Create)))
                {
                    binWriter.Write(o.ImageRow.BinaryData);
                }
            }
            else if (o.ImageRow.BodyType == ImageRowTypes.CompressedMoxcel)
            {
                using (BinaryWriter binWriter =
                    new BinaryWriter(System.IO.File.Open(filePath, FileMode.Create)))
                {
                    binWriter.Write(o.ImageRow.Body as byte[]);
                }
            }
            else if (o is MetadataPart)
            {
                if (projectType == ProjectType.String)
                {
                    using (StreamWriter outfile = new StreamWriter(filePath))
                    {
                        ////DataContractJsonSerializer js = new DataContractJsonSerializer(((MetadataPart)o).Body.GetType());
                        ////js.WriteObject(outfile.BaseStream, ((MetadataPart)o).Body);
                        //DataSerializer reader = new DataSerializer();
                        //var content = reader.Deserialize(o.ImageRow.Body as string);
                        DataSerializer dw = new DataSerializer();
                        outfile.Write(dw.Serialize(((MetadataPart)o).Body));
                    }
                }
                else
                {
                    var settings = new XmlWriterSettings() { Indent = true };
                    using (XmlWriter xmlWriter = XmlWriter.Create(filePath, settings))
                    {
                        System.Windows.Markup.XamlWriter.Save(o, xmlWriter);
                    }
                }
            }
            else
            {
                using (StreamWriter outfile = new StreamWriter(filePath))
                {
                    try
                    {
                        DataSerializer reader = new DataSerializer();
                        var content = reader.Deserialize(o.ImageRow.Body as string);
                        DataSerializer dw = new DataSerializer();
                        outfile.Write(dw.Serialize(content));

                    }
                    catch (Exception)
                    {
                        outfile.Write(o.ImageRow.Body);
                    }
                }
            }

            System.IO.File.SetCreationTime(filePath, o.ImageRow.Creation);
            System.IO.File.SetLastWriteTime(filePath, o.ImageRow.Modified);

            lock (this._settings)
            {
                var fileInfo = new FileInfo()
                    {
                        Path = relativeUri.ToString(),
                        Id = o.ImageRow.FileName,
                        Attributes = o.ImageRow.Attributes,
                        BodyType = o.ImageRow.BodyType,
                        Class = o.GetType().Name
                    };
                this._settings.Files.Add(fileInfo);
            }

        }

    }
}


