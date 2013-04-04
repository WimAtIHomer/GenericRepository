using System;

namespace IHomer.GenericRepository.Attributes
{
    [AttributeUsageAttribute(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class UniqueAttribute : Attribute
    {
    }
}
