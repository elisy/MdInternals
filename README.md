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

### Создание файла из ранее выгруженного xml-формата

```csharp
var project = new CfProject();
var mp = project.Load(@"D:\Config\Xml\Config.cfproj");
mp.Save(@"D:\config.cf");
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
