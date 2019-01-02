# MdInternals
## 1C Enterprise configuration analyzer and decompiler 

### Выгрузка форматов cf, cfu, epf, erf в xml-формат

```csharp
var cf = new CfPackage();
//или var cf = new EpfPackage();
//или var cf = new ErfPackage();
//или var cf = new CfuPackage();
cf.Open(@"D:\config.cf");
var project = new CfProject();
project.Save(epf, @"D:\Config\Xml\Config.cfproj", ProjectType.Xml);
```

### Создание файла из выгруженного xml-формата

```csharp
var project = new CfProject();
var mp = project.Load(@"D:\Config\Xml\Config.cfproj");
mp.Save(@"D:\config.cf");
```

### Чтение из MSSQL-таблицы
```csharp
var image = ImageReader.ReadImageFromConfig(@"data source=192.168.1.2\SQL2005;user=login;pwd=password;database=Database1C");
```

### Обращение к внутренним файлам
```csharp
var mp = new EpfPackage();
mp.Open(file);
var root = mp.MetadataObjects.Where(m => m.ImageRow.FileName == "root").FirstOrDefault();
var rp = new RootPointer(root.ImageRow);
var part = mp.MetadataObjects.Where(m => m.ImageRow.FileName == rp.MetadataPackageFileName.ToString()).FirstOrDefault();
```

### Декомпилирование ОП-кода 1С

```csharp
string opCodeString = System.IO.File.ReadAllText(@"D:\OpCode.txt");
CodeReader reader = new CodeReader(opCodeString, true);
string decompiledString = reader.GetSourceCode();
using (StreamWriter outfile = new StreamWriter(@"D:\OpCode-decompiled.txt"))
{
    outfile.Write(decompiledString);
}
```
