using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elisy.MdInternals
{
    [AttributeUsageAttribute(AttributeTargets.Field | AttributeTargets.Class, Inherited = false)]
    public sealed class GuidAttribute : Attribute
    {
        public string Value { get; private set; }
        public GuidAttribute(string guid)
        {
            this.Value = guid;
        }
    }

    //[AttributeUsageAttribute(AttributeTargets.Field, Inherited = false)]
    //public sealed class MetadataPartAttribute : Attribute
    //{
    //    public Type Type { get; private set; }
    //    public MetadataPartAttribute(Type elementType)
    //    {
    //        Type = elementType;
    //    }
    //}


    public enum MetadataPartType : int
    {
        //[MetadataPartAttribute(typeof(UnresolvedPart))]
        //Unresolved = -1,
        //[Guid("09736b02-9cac-4e3f-b4f7-d3e9576ab948")]
        //Role,
        //[Guid("0c89c792-16c3-11d5-b96b-0050bae0a95d")]
        //CommonTemplate,
        //[Guid("0fe48980-252d-11d6-a3c7-0050bae0a776"), MetadataPartAttribute(typeof(CommonModule))]
        //CommonModule,
        //[Guid("11bdaf85-d5ad-4d91-bb24-aa0eee139052")]
        //ScheduledJob,
        //[Guid("24c43748-c938-45d0-8d14-01424a72b11e")]
        //SessionParameter,
        //[Guid("3e5404af-6ef8-4c73-ad11-91bd2dfac4c8")]
        //Style,
        //[Guid("3e7bfcc0-067d-11d6-a3c7-0050bae0a776")]
        //Filter,
        //[Guid("4e828da6-0f44-4b5b-b1c0-a2b3cfe7bdcc")]
        //EventSubscription,
        //[Guid("58848766-36ea-4076-8800-e91eb49590d7")]
        //StyleElement,
        //[Guid("7dcd43d9-aca5-4926-b549-1842e6a4e8cf")]
        //CommonPicture,
        //[Guid("857c4a91-e5f4-4fac-86ec-787626f1c108")]
        //ExchangePlan,
        //[Guid("8657032e-7740-4e1d-a3ba-5dd6e8afb78f")]
        //WebService,
        //[Guid("9cd510ce-abfc-11d4-9434-004095e12fc7")]
        //Language,
        //[Guid("cc9df798-7c94-4616-97d2-7aa0b7bc515e")]
        //XdtoPackage,
        //[Guid("d26096fb-7a5d-4df9-af63-47d04771fa9b")]
        //WsReference,

        //[Guid("0195e80c-b157-11d4-9435-004095e12fc7")]
        //Constant,
        //[Guid("061d872a-5787-460e-95ac-ed74ea3a3e84")]
        //Document,
        //[Guid("07ee8426-87f1-11d5-b99c-0050bae0a95d")]
        //CommonForm,
        //[Guid("13134201-f60b-11d5-a3c7-0050bae0a776")]
        //InformationRegister,
        //[Guid("36a8e346-9aaa-4af9-bdbd-83be3c177977")]
        //DocumentNumerator,
        //[Guid("4612bd75-71b7-4a5c-8cc5-2b0b65f9fa0d")]
        //DocumentJournal,
        //[Guid("631b75a0-29e2-11d6-a3c7-0050bae0a776")]
        //Report,
        //[Guid("82a1b659-b220-4d94-a9bd-14d757b95a48")]
        //ChartOfCharacteristicTypes,
        //[Guid("b64d9a40-1642-11d6-a3c7-0050bae0a776")]
        //AccumulationRegister,
        //[Guid("bc587f20-35d9-11d6-a3c7-0050bae0a776")]
        //Sequence,
        //[Guid("bf845118-327b-4682-b5c6-285d2a0eb296")]
        //DataProcessor,
        //[Guid("cf4abea6-37b2-11d4-940f-008048da11f9")]
        //Catalog,
        //[Guid("f6a80749-5ad7-400b-8519-39dc5dff2542")]
        //Enum,
        //[Guid("238e7e88-3c5f-48b2-8a3b-81ebbecb20ed")]
        //ChartOfAccounts,
        //[Guid("2deed9b8-0056-4ffe-a473-c20a6c32a0bc")]
        //AccountingRegister,
        [Guid("30b100d6-b29f-47ac-aec7-cb8ca8a54767")]
        ChartOfCalculationTypes,
        [Guid("f2de87a8-64e5-45eb-a22d-b3aedab050e7")]
        CalculationRegister,
        [Guid("3e63355c-1378-4953-be9b-1deb5fb6bec5")]
        Task,
        [Guid("fcd3404e-1523-48ce-9bc0-ecdb822684a1")]
        BusinessProcess
    }
}
