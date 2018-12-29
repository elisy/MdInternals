using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;
using System.IO.Compression;

using Elisy.MdInternals;
using Elisy.MdInternals.Cf;
using Elisy.MdInternals.Serialization;

namespace Elisy.MdInternals.CfProject
{
    public enum ProjectType
    {
        Xml,
        Debug
    }

    public class CfProject
    {
        public CfProject()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //Resolve 1C:Enterprise error FileNotFoundException
            System.Reflection.Assembly result = AppDomain.CurrentDomain.GetAssemblies().AsParallel().Where(m => m.FullName == args.Name).FirstOrDefault();
            return result;
        }

        private CfProjectSettings _settings;

        public void Save(MetadataPackage package, string path, ProjectType projectType)
        {
            string projectFolder = Path.GetDirectoryName(path);
            if (!Directory.Exists(projectFolder))
                Directory.CreateDirectory(projectFolder);

            _settings = new CfProjectSettings()
            {
                Type = projectType,
                Version = package.GetType().Assembly.GetName().Version.ToString(),
                MdInternalsVersion = this.GetType().Assembly.GetName().Version.ToString(),
                PackageType = package.GetType().FullName
            };

            package.MetadataObjects.AsParallel().ForAll(m =>
                {
                    string relativePath = GetRelativePath(m, projectType);
                    FileInfo fileInfo = null;
                    if (m.ImageRow.BodyType == ImageRowTypes.CompressedImage)
                        fileInfo = UnloadImage(projectFolder, relativePath, m, projectType);
                    else
                        fileInfo = UnloadElement(projectFolder, relativePath, m, projectType);

                    lock (this._settings)
                    {
                        this._settings.Files.Add(fileInfo);
                    }
                });

            var settings = new XmlWriterSettings() { Indent = true };
            using (XmlWriter xmlWriter = XmlWriter.Create(path, settings))
            {
                System.Windows.Markup.XamlWriter.Save(_settings, xmlWriter);
            }
        }

        private string GetRelativePath(ImageRowElement o, ProjectType projectType)
        {
            string path = "";
            if ((o is RootPointer) || (o is Configuration) || (o is Version) || (o is Versions)
                || (o is UpdateInfo)
                || (o is ExternalDataProcessor)
                || (o is ExternalReport))
                path = "";
            else if (o is MetadataPart)
                path = o.GetType().Name;
            else if (o is UnresolvedPart)
                path = "Unresolved";
            else
                path = "";

            string fileName = o.ImageRow.FileName;
            if ((o is MetadataPart) && !String.IsNullOrEmpty(((MetadataPart)o).Name))
                fileName = ((MetadataPart)o).Name;

            string extension = "";
            if (o.ImageRow.BodyType == ImageRowTypes.CompressedMoxcel)
                extension = ".mxl";
            else if (o.ImageRow.BodyType == ImageRowTypes.CompressedUtf8MarkedString)
                extension = ".txt";
            else if (o.ImageRow.BodyType == ImageRowTypes.Utf8MarkedString)
                extension = ".txt";
            else if (o.ImageRow.BodyType == ImageRowTypes.CompressedImage)
            {
                extension = ".img";
            }

            if (o is MetadataPart)
            {
                if (projectType == ProjectType.Debug)
                    extension = ".txt";
                else
                    extension = ".xml";
            }

            return Path.Combine(path, fileName + extension);
        }

        private FileInfo UnloadImage(string projectFolder, string relativePath, ImageRowElement o, ProjectType projectType)
        {
            string folder = Path.GetDirectoryName(Path.Combine(projectFolder, relativePath));
            //if (!Directory.Exists(folder))
            //    Directory.CreateDirectory(folder);

            //string filePath = Path.Combine(projectFolder, relativePath);

            //using (BinaryWriter binWriter =
            //    new BinaryWriter(System.IO.File.Open(filePath, FileMode.Create)))
            //{
            //    binWriter.Write(o.ImageRow.BinaryData);
            //}

            //System.IO.File.SetCreationTime(filePath, o.ImageRow.Creation);
            //System.IO.File.SetLastWriteTime(filePath, o.ImageRow.Modified);

            var fileInfo = new FileInfo()
            {
                Path = Path.Combine(Path.GetDirectoryName(relativePath), Path.GetFileNameWithoutExtension(relativePath)),
                Id = o.ImageRow.FileName,
                //Attributes = o.ImageRow.Attributes,
                BodyType = o.ImageRow.BodyType,
                Class = o.GetType().FullName
            };

            string imageFolder = Path.Combine(folder, Path.GetFileNameWithoutExtension(relativePath));
            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);

            Image image = (Image)o.ImageRow.Body;
            fileInfo.Items = new FileInfoList(); 

            foreach (var row in image.Rows)
            {
                UnresolvedPart rowPart = new UnresolvedPart(row);
                string imageRowPath = GetRelativePath(rowPart, projectType);
                string imageRelativePath = Path.Combine(imageFolder, Path.GetFileName(imageRowPath));
                FileInfo rowFileInfo = UnloadElement(projectFolder, imageRelativePath, rowPart, projectType);
                fileInfo.Items.Add(rowFileInfo);
            }

            return fileInfo;
        }

        private FileInfo UnloadElement(string projectFolder, string relativePath, ImageRowElement o, ProjectType projectType)
        {
            string folder = Path.GetDirectoryName(Path.Combine(projectFolder, relativePath));
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string filePath = Path.Combine(projectFolder, relativePath);

            if ((o is UnresolvedPart) && (o.ImageRow.BodyType == ImageRowTypes.CompressedUtf8MarkedString))
            {
                using (StreamWriter outfile = new StreamWriter(filePath))
                {
                    try
                    {
                        DataSerializer dw = new DataSerializer();
                        outfile.Write(
                            dw.Serialize(
                                dw.Deserialize(o.ImageRow.Body as string)
                            )
                        );
                    }
                    catch (Exception)
                    {
                        outfile.Write(o.ImageRow.Body as string);
                    }
                }
            }
            else if (o is UnresolvedPart)
            {
                using (BinaryWriter binWriter =
                    new BinaryWriter(System.IO.File.Open(filePath, FileMode.Create)))
                {
                    binWriter.Write(o.ImageRow.BinaryData);
                }
            }
            else if (o is MetadataPart)
            {
                if (projectType == ProjectType.Debug)
                {
                    using (StreamWriter outfile = new StreamWriter(filePath))
                    {
                        DataSerializer dw = new DataSerializer();
                        outfile.Write(
                            dw.Serialize( 
                                dw.Deserialize(((MetadataPart)o).ImageRow.Body as string)
                            )
                        );
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

            var fileInfo = new FileInfo()
            {
                Path = relativePath,
                Id = o.ImageRow.FileName,
                //Attributes = o.ImageRow.Attributes,
                BodyType = o.ImageRow.BodyType,
                Class = o.GetType().FullName
            };

            return fileInfo;
        }

        public Image LoadImage(string path)
        {
            using (XmlReader xmlReader = XmlReader.Create(path))
            {
                _settings = System.Windows.Markup.XamlReader.Load(xmlReader) as CfProjectSettings;
            }

            Image image = new Image();

            string folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            image.Rows = CreateImageRows(folder);

            return image;
        }

        public MetadataPackage Load(string path)
        {
            Image image = LoadImage(path);

            var metadataPackage = Activator.CreateInstance(typeof(MetadataPackage).Assembly.GetType(_settings.PackageType), new object[] { image }) as MetadataPackage;

            return metadataPackage;
        }

        private ImageRow[] CreateImageRows(string folder)
        {
            return _settings.Files.AsParallel().Select(m => CreateImageRow(m, Path.Combine(folder, m.Path))).ToArray();
        }

        private ImageRow CreateImageRow(FileInfo fileInfo, string path)
        {
            ImageRow row = new ImageRow();
            row.Creation = System.IO.File.GetCreationTime(path);
            row.Modified = System.IO.File.GetLastWriteTime(path);
            row.FileName = fileInfo.Id;

            string model = "";

            Type fileInfoClass = typeof(MetadataPackage).Assembly.GetType(fileInfo.Class);
            if (((fileInfoClass == typeof(UnresolvedPart)) || fileInfoClass.IsSubclassOf(typeof(UnresolvedPart))) 
                && ((fileInfo.BodyType == ImageRowTypes.CompressedUtf8MarkedString) || (fileInfo.BodyType == ImageRowTypes.Utf8MarkedString)))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    model = reader.ReadToEnd();
                }
            }
            else if (fileInfoClass.IsSubclassOf(typeof(MetadataPart)))
            {
                if (_settings.Type == ProjectType.Debug)
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        model = reader.ReadToEnd();
                    }
                }
                else if (_settings.Type == ProjectType.Xml)
                {
                    MetadataPart mp = null;
                    using (XmlReader xmlReader = XmlReader.Create(path))
                    {
                        mp = System.Windows.Markup.XamlReader.Load(xmlReader) as MetadataPart;
                        //mp.CreateModelFromProperties();
                    }
                    DataSerializer ds = new DataSerializer();
                    model = ds.Serialize(mp.GetModel());
                }
                else
                    throw new InvalidOperationException("Unknown project type");
            }
            else if (fileInfo.BodyType == ImageRowTypes.CompressedImage)
            {
                Image image = new Image();

                if (fileInfo.Items != null)
                    image.Rows = fileInfo.Items.AsParallel().Select(m => CreateImageRow(m, Path.Combine(fileInfo.Path, m.Path))).ToArray();

                using (MemoryStream stream = new MemoryStream())
                {
                    ImageWriter imageWriter = new ImageWriter(stream);
                    imageWriter.WriteImage(image);
                    imageWriter.Dispose();

                    row.BinaryData = stream.GetBuffer();
                    return row;
                }
                
            }
            else if ((fileInfoClass == typeof(UnresolvedPart)) || fileInfoClass.IsSubclassOf(typeof(UnresolvedPart)))
            {
                row.BinaryData = System.IO.File.ReadAllBytes(path);
                return row;
            }
            else
                throw new InvalidOperationException("Unknown fileInfo class");

            UTF8Encoding utf8 = new UTF8Encoding(true);
            MemoryStream memory = new MemoryStream();
            byte[] preamble = utf8.GetPreamble();
            memory.Write(preamble, 0, preamble.Length);
            byte[] content = utf8.GetBytes(model);
            memory.Write(content, 0, content.Length);
            byte[] deflatedBody = memory.ToArray();

            if (fileInfo.BodyType == ImageRowTypes.Utf8MarkedString)
                row.BinaryData = deflatedBody;
            else if (fileInfo.BodyType == ImageRowTypes.CompressedUtf8MarkedString)
            {
                MemoryStream compressedStream = new MemoryStream();
                DeflateStream stream = new DeflateStream(compressedStream, CompressionMode.Compress);
                stream.Write(deflatedBody, 0, deflatedBody.Length);
                stream.Close();
                row.BinaryData = compressedStream.ToArray();
            }
            else
                throw new InvalidOperationException("Unknown File Info Body Type");

            return row;
        }


    }
}


