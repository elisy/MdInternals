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


    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = false)]
    public sealed class StringTemplateAttribute : Attribute
    {
        public string Name { get; private set; }
        public Type ResourceType { get; private set; }
        public StringTemplateAttribute(string name, Type resourceType)
        {
            this.Name = name;
            this.ResourceType = resourceType;
        }
    }


    public enum MetadataPartType : int
    {
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
