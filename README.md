# MdInternals
1C Enterprise configuration analyzer and decompiler 

Выгрузка форматов cf, cfu, epf, erf в xml-формат

var cf = new CfPackage();
//или var cf = new EpfPackage();
//или var cf = new ErfPackage();
//или var cf = new CfuPackage();
cf.Open(@"D:\config.cf");
var project = new CfProject();
project.Save(epf, @"D:\Config\Xml\Config.cfproj", ProjectType.Xml);

Создание файла из ранее выгруженного xml-формата

var project = new CfProject();
var mp = project.Load(@"D:\Config\Xml\Config.cfproj");
mp.Save(@"D:\config.cf");
