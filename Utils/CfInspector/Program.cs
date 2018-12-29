using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.IO.Compression;
using System.Globalization;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals;
using Elisy.MdInternals.Serialization;
using Elisy.MdInternals.Cil;
using Elisy.MdInternals.CfProject;
using Elisy.MdInternals.DataElements;

using System.Runtime.Serialization.Json;

namespace CfInspector
{
    class Program
    {
        static void Main(string[] args)
        {

            //var collections = mp.MetadataObjects.OfType<Elisy.MdInternals.Enum>().Select(m => m.List_1).ToArray();
            //Collection template = collections[0].Clone() as Collection;
            //FindTemplate(template, collections, new int[] { });

            //DataSerializer ds = new DataSerializer();
            //string stringTemplate = ds.Serialize(template);



            var epf = new EpfPackage();
            epf.Open(@"D:\336-cftest\epf.epf");
            var project = new CfProject();
            //project.Save(mp, @"D:\336-cftest\Txt1\UT82.cfproj", ProjectType.Debug);
            project.Save(epf, @"D:\336-cftest\Epf\Xml\Epf.cfproj", ProjectType.Xml);
            var mp1 = project.Load(@"D:\336-cftest\Epf\Xml\Epf.cfproj");
            //project.Save(mp1, @"D:\336-cftest\Txt2\UT82.cfproj", ProjectType.Debug);
            mp1.Save(@"D:\336-cftest\epf-new.epf");

            var erf = new ErfPackage();
            erf.Open(@"D:\336-cftest\erf.erf");
            project.Save(erf, @"D:\336-cftest\Erf\Xml\Erf.cfproj", ProjectType.Xml);
            var mpErf = project.Load(@"D:\336-cftest\Erf\Xml\Erf.cfproj");
            mpErf.Save(@"D:\336-cftest\erf-new.erf");

            var cfu = new CfuPackage();
            cfu.Open(@"D:\336-cftest\cfu.cfu");
            //var project = new CfProject();
            project.Save(cfu, @"D:\336-cftest\Cfu\Xml\Cfu.cfproj", ProjectType.Xml);
            var mpCfu = project.Load(@"D:\336-cftest\Cfu\Xml\Cfu.cfproj");
            mpCfu.Save(@"D:\336-cftest\cfu-new.cfu");

            ////var mp = CfPackage.Open(typeof(CfPackage), @"D:\336-cftest\1Cv82-sb-eng.cf");
            //var mp = new CfPackage();
            //mp.Open(@"D:\336-cftest\1Cv82-sb-eng.cf");
            //var project = new CfProject();
            ////project.Save(mp, @"D:\336-cftest\Txt1\SB82.cfproj", ProjectType.Debug);
            //project.Save(mp, @"D:\336-cftest\Xml\SB82.cfproj", ProjectType.Xml);
            //var mp1 = project.Load(@"D:\336-cftest\Xml\SB82.cfproj");
            ////project.Save(mp1, @"D:\336-cftest\Txt2\SB82.cfproj", ProjectType.Debug);
            //mp1.Save(@"D:\336-cftest\1Cv82-sb-eng-new.cf");


            ////Find enum template
            //var collections = mp.MetadataObjects.OfType<Elisy.MdInternals.Enum>().Select(m => m.List_1).ToArray();
            //Collection template = collections[0].Clone() as Collection;
            //FindTemplate(template, collections, new int[] { });

            //DataSerializer ds = new DataSerializer();
            //string stringTemplate = ds.Serialize(template);


            ////Find Epf/Erf template
            //List<string> strings = new List<string>();
            //var files = Directory.GetFiles(@"D:\336-cftest\EpfErf", "*.erf");
            //foreach (var file in files)
            //{
            //    var mp = new EpfPackage();
            //    mp.Open(file);
            //    var root = mp.MetadataObjects.Where(m => m.ImageRow.FileName == "root").FirstOrDefault();
            //    RootPointer rp = new RootPointer(root.ImageRow);
            //    var part = mp.MetadataObjects.Where(m => m.ImageRow.FileName == rp.MetadataPackageFileName.ToString()).FirstOrDefault();
            //    strings.Add(part.ImageRow.Body.ToString());
            //}

            //DataSerializer ds = new DataSerializer();
            //var collections = strings.Select(m => ds.Deserialize(m)).ToArray();

            //Collection template = collections[0].Clone() as Collection;
            //FindTemplate(template, collections, new int[] { });

            //string stringTemplate = ds.Serialize(template);

            //var mp = MetadataPackage.Open(@"D:\336-cfproject\1Cv82.cf");
            //var project = new CfProject();
            ////project.Save(mp, @"D:\336-cfproject\Txt\UT82.cfproj", ProjectType.Debug);
            //project.Save(mp, @"D:\336-cfproject\Xml\UT82.cfproj", ProjectType.Xml);



            ////Create templates
            //MetadataPackage mp = MetadataPackage.Open(@"D:\336-cfproject\1Cv82.cf");
            //MetadataPackage mp1 = MetadataPackage.Open(@"D:\336-cfproject\1Cv82-sb-eng.cf");
            ////MetadataPackage mp2 = MetadataPackage.Open(@"D:\336-cfproject\1Cv82-ssl.cf");
            //MetadataPackage mp3 = MetadataPackage.Open(@"D:\336-cfproject\1Cv82-net.cf");

            //List<ImageRowElement> items = mp.MetadataObjects;
            //items = items.Union(mp1.MetadataObjects).ToList();
            ////items = items.Union(mp2.MetadataObjects).ToList();
            //items = items.Union(mp3.MetadataObjects).ToList();

            //CreateTemplates(items, @"D:\336-cfproject\templates\");





            
            //var stream = new FileStream(@"D:\228-Свойства мастеров\GLTU.cf", FileMode.Open, FileAccess.Read, FileShare.Read);
            //var stream = new FileStream(@"D:\244-decompile\Int-6130.cf", FileMode.Open, FileAccess.Read, FileShare.Read);
            //Image image = ImageReader.ReadImageFrom(stream);
            //UnloadCommonModules(image, @"D:\244-decompile\Int-6130\");


            //var stream = new FileStream(@"D:\244-decompile\TestCf.cf", FileMode.Open, FileAccess.Read, FileShare.Read);
            //Image image = ImageReader.ReadImageFrom(stream);
            //UnloadCommonModules(image, @"D:\244-decompile\TestCf\");

            //var stream = new FileStream(@"D:\336-cfproject\configsave\configsave.cf", FileMode.Open, FileAccess.Read, FileShare.Read);
            //Image image = ImageReader.ReadImageFrom(stream);
            //var r = from row in image.Rows
            //        where row.FileName.Length < 10
            //        orderby row.FileName 
            //        select row.FileName;
            //MetadataPackage mp = MetadataPackage.Open(@"D:\336-cfproject\configsave\configsave.cf");
            



            //MetadataPackage mp = MetadataPackage.Open(@"D:\244-decompile\Int-6130-82.cf");
            //UnloadCommonModules(mp, @"D:\244-decompile\Int-6130-82\");
            //MetadataPackage mp = MetadataPackage.Open(@"D:\244-decompile\1c-zk\1c-zk.cf");
            //UnloadCommonModules(mp, @"D:\244-decompile\1c-zk\1c-zk\1c-zk");
            //MetadataPackage mp = MetadataPackage.Open(@"D:\244-decompile\Int-60314-82.cf");
            //UnloadCommonModules(mp, @"D:\244-decompile\Int-60314-82\");
            //MetadataPackage mp = MetadataPackage.Open(@"D:\244-decompile\Int-7090.cf");
            //UnloadCommonModules(mp, @"D:\244-decompile\Int-7090\");


            string opCodeString = System.IO.File.ReadAllText(@"D:\244-decompile\OpCode1.txt");
            CodeReader reader = new CodeReader(opCodeString, true);
            string decompiledString = reader.GetSourceCode();
            using (StreamWriter outfile =
                new StreamWriter(@"D:\244-decompile\OpCode1-decompiled.txt"))
            {
                outfile.Write(decompiledString);
            }


            //CfProject cf = new CfProject();
            //var request = from a in AppDomain.CurrentDomain.GetAssemblies().Where(m => m.GlobalAssemblyCache == false)
            //              select new { 
            //                  Name = a.FullName, 
            //                  Company = a.GetCustomAttributes(typeof(System.Reflection.AssemblyCompanyAttribute), false).FirstOrDefault(),
            //                  PublicKey = a.GetName().GetPublicKey(),
            //                  Assembly = a };
            //var assemblies = request.ToArray();

            //foreach (var i in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    var an = i.GetName();
            //    Console.WriteLine(an.FullName);
            //    var key = an.GetPublicKey();
            //    foreach (var a in key)
            //        Console.Write("{0:x}", a);
            //    Console.WriteLine();
            //}

            //CfProject project = new CfProject();
            //project.Save(mp, @"D:\244-decompile\Int-6130-82\Debug\8-2-14.cfproj", ProjectType.Debug);
            //project.Save(mp, @"D:\244-decompile\Int-6130-82\Xml\8-2-14.cfproj", ProjectType.Xml);


            //var stream = new FileStream(@"D:\244-decompile\Int-6131.cf", FileMode.Open, FileAccess.Read, FileShare.Read);
            //Image image = ImageReader.ReadImageFrom(stream);
            //UnloadCommonModules(image, @"D:\244-decompile\Int-6131\");
            
            
            
            return;


            //Image image = ImageReader.ReadImageFromConfig(@"data source=192.168.1.2\SQL2005;user=sa;pwd=ala1230;database=GalTUS2");

            //var request1 = from ir in image.Rows.AsParallel()
            //               where ir.FileName == "44677218-5bb1-4bce-922c-bd5649b3e63f.0"
            //               orderby ir.FileName
            //               select new {Ir = ir, FileName = ir.FileName, Content = ir.Body};
            //var e1 = request1.ToArray();

            //return;


            //var stream = new FileStream(@"D:\228-Свойства мастеров\GLTU.cf", FileMode.Open, FileAccess.Read, FileShare.Read);
            //Image image = ImageReader.ReadImageFrom(stream);

            ////Image subImage = image.Rows[1].Body as Image;
            ////var body = subImage.Rows[0].Body;

            ////var request = from ir in image.Rows
            ////              //where !(ir.Id.EndsWith(".0") || ir.Id.EndsWith(".1"))
            ////              //where !(ir.Id.EndsWith(".0") || ir.Id.EndsWith(".1")) && (ir.Body is String) && (ir.Body.ToString().Length > 2) //&& (ir.Body.ToString().Substring(1, 2) != "{1")
            ////              where ir.Body is byte[]
            ////              select new { a = ir.Id, b = ir.Body, c = ir.Body.ToString().Length, d = ir.Body.ToString().Substring(0,3)};

            ////var request = from ir in image.Rows.AsParallel()
            ////              where (ir.Body is String) && (ir.Body.ToString().StartsWith("{1"))
            ////              //where ir.Body is byte[]
            ////              select new { a = ir.Id, b = ir.Body, c = ir.Body.ToString().Length, d = ir.Body.ToString().Substring(0, 3) };

            ////var request1 = from ir in image.Rows
            ////              where ir.Id == "027d2b35-f55f-4360-a331-0d366034b2f0"
            ////              select ir.Body as string;
            ////var e1 = request1.FirstOrDefault();

            //var request2 = from ir in image.Rows
            //               where (ir.Id == "027d2b35-f55f-4360-a331-0d366034b2f0") || (ir.Id == "027d2b35-f55f-4360-a331-0d366034b2f0.0")
            //               select new { ir.Creation, ir.Modified };
            //var e2 = request2.ToArray();

            //var request3 = from ir in image.Rows
            //               where (ir.Id == "027d2b35-f55f-4360-a331-0d366034b2f0")
            //               select new { ir.Creation, ir.Modified };
            //var e3 = request3.FirstOrDefault();
            ////{ Creation = 633814309420000 0x0002407364c43be0, Modified = 633814309420000, dt = 04.01.0003 13:57:10 }
            ////{ Creation = 633814309400000 0x0002407364c3edc0, Modified = 633814309400000, dt = 04.01.0003 13:57:10 }

            ////19.12.2008 14:57:56
            ////19.12.2008 14:57:54
            ////Ticks	0x08cb30014e278500	long 
            ////633652954740000000
            ////633814309400000

            ////var dt = new DateTime((long)e3.Creation);
            //var dt1 = DateTime.Parse("19.12.2008 14:57:54");

            //Console.WriteLine("");

            //var request = from ir in image.Rows
            //              where ir.BodyType == ImageRowTypes.CompressedImage
            //              select ir;

            //var array = request.ToArray();
        }


        public static void CreateTemplates(List<ImageRowElement> metadataObjects, string pathDestination)
        {

            if (!Directory.Exists(pathDestination))
                Directory.CreateDirectory(pathDestination);

            var types = metadataObjects.GroupBy(m => m.GetType()).Select(m => m.Key).OrderBy(m=>m.FullName).ToArray();
            foreach (Type type in types)
            {
                if (type == typeof(Versions) || type == typeof(UnresolvedPart))
                    continue;

                var strings = metadataObjects.Where(m => m.GetType() == type).Select(m => m.ImageRow.Body).OfType<string>().ToArray();
                if (strings.Length < 2)
                    continue;

                DataSerializer ds = new DataSerializer();
                var collections = strings.Select(m => ds.Deserialize(m)).ToArray();

                Collection template = collections[0].Clone() as Collection;
                FindTemplate(template, collections, new int[] { });

                string stringTemplate = ds.Serialize(template);

                using (StreamWriter outfile =
                    new StreamWriter(pathDestination + type.FullName + ".txt"))
                {
                    outfile.Write(stringTemplate);
                }

            }
        }

        public static void FindTemplate(Collection template, Collection[] items, int[] indexes)
        {
            for (int i = 0; i < template.Count; i++)
            {
                int[] currentIndexes = indexes.Clone() as int[];
                Array.Resize(ref currentIndexes, currentIndexes.Length + 1);
                currentIndexes[currentIndexes.Length - 1] = i;

                string stringIndexes = String.Join("_", currentIndexes.Select(m=>m.ToString(CultureInfo.InvariantCulture)).ToArray());

                if (!ItemsHaveTheSameType(template[i].GetType(), items, currentIndexes))
                {
                    throw new InvalidOperationException();
                    //template[i] = "\"#ElisyBound:Data_" + stringIndexes + "\"";
                    //continue;
                }

                if (template[i] is Collection)
                {
                    if(CollectionLengthsAreEqual(((Collection)template[i]).Count, items, currentIndexes))
                        FindTemplate(template[i] as Collection, items, currentIndexes);
                    else
                    {
                        template[i] = "\"#ElisyBound:" + (IsLocalizable(items, currentIndexes) ? "Localizable_" : "List_") + stringIndexes + "\"";
                        continue;
                    }
                }

                //current element is not Collection so it's string
                if (!ItemsHaveTheSameValue(template[i].ToString(), items, currentIndexes))
                {
                    template[i] = "\"#ElisyBound:" + FindCommonInternalType(items, currentIndexes) + "_" + stringIndexes + "\"";
                    continue;
                }
            }
        }

        public static bool CollectionLengthsAreEqual(int length, Collection[] items, int[] indexes)
        {
            foreach (var item in items)
            {
                if (!(item is Collection))
                    throw new InvalidOperationException();

                if ((item.GetElement(indexes) as Collection).Count != length)
                    return false;
            }

            return true;
        }

        public static bool ItemsHaveTheSameType(Type type, Collection[] items, int[] indexes)
        {
            foreach (var item in items)
            {
                if (item.GetElement(indexes).GetType() != type)
                    return false;
            }

            return true;
        }

        public static bool ItemsHaveTheSameValue(String value, Collection[] items, int[] indexes)
        {
            foreach (var item in items)
            {
                if (item.GetElement(indexes).ToString() != value)
                    return false;
            }

            return true;
        }

        public static bool IsLocalizable(Collection[] items, int[] indexes)
        {
            foreach (var item in items)
            {
                try
                {
                    LocalStringType localizable = new LocalStringType(item.GetElement(indexes));
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public static string FindCommonInternalType(Collection[] items, int[] indexes)
        {
            List<string> types = new List<string>();
            foreach (var item in items)
            {
                object rawValue = item.GetElement(indexes);
                if (rawValue is Collection)
                {
                    types.Add("List");
                    continue;
                }

                string value = rawValue.ToString();
                Guid guid;
                if (Guid.TryParse(value, out guid))
                {
                    types.Add("Guid");
                    continue;
                }

                try
                {
                    int res = value.AsInt32();
                    types.Add("Int");
                    continue;
                }
                catch (Exception)
                {
                }

                try
                {
                    double res = value.AsDouble();
                    types.Add("Double");
                    continue;
                }
                catch (Exception)
                {
                }

                try
                {
                    DateTime res = value.AsDateTime();
                    types.Add("DateTime");
                    continue;
                }
                catch (Exception)
                {
                }

                types.Add("String");
            }

            var typeSet = types.GroupBy(m => m).Select(m => m.Key).ToArray();
            if (typeSet.Length == 1)
                return types[0];
            else if (typeSet.Length == 2 && typeSet.Contains("Double") && types.Contains("Int"))
                return "Double";
            else
                return "Object";
        }

        public static void UnloadCommonModules(MetadataPackage mp, string folder)
        {
            var requestModules = from o in mp.MetadataObjects.OfType<CommonModule>()
                                 select o;

            var requestContent = from module in requestModules
                                 join ir in mp.MetadataObjects on module.ImageRow.FileName + ".0" equals ir.ImageRow.FileName
                                 where ir.ImageRow.Body is Image
                                 orderby module.Name
                                 select new { FileName = module.Name, OriginalImage = ((Image)ir.ImageRow.Body), Module = ((Image)ir.ImageRow.Body).Rows.Where(i => i.FileName == "info").FirstOrDefault(), Image = ((Image)ir.ImageRow.Body).Rows.Where(i => i.FileName == "image").FirstOrDefault() };

            var files = requestContent.ToArray();

            foreach (var file in files)
            {
                if (file.Image == null)
                    continue;

                string fileName = file.FileName;
                string opCodeString = file.Image.Body.ToString();

                //if (!(fileName == "Инт_СистемаСервер" || fileName == "Инт_СистемаСерверКэшируемый"))
                //    continue;

                using (StreamWriter outfile =
                    new StreamWriter(folder + fileName + ".opcode"))
                {
                    outfile.Write(opCodeString);
                }

                CodeReader reader = new CodeReader(opCodeString, false);
                string decompiledString = reader.GetSourceCode();
                using (StreamWriter outfile =
                    new StreamWriter(folder + fileName + ".txt"))
                {
                    outfile.Write(decompiledString);
                }

            }
        }
    }
}
