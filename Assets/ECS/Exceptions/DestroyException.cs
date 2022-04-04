using System;
using System.Reflection;

namespace ECS.Exceptions
{
    public class DestroyException : Exception
    {
        public DestroyException(MemberInfo classType) : base($"{classType.Name} already destroyed!")
        {
        }
    }
}