using System;

namespace IHomer.GenericRepository.Attributes
{
    [AttributeUsageAttribute(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ParentAttribute : Attribute
    {
    }
}
