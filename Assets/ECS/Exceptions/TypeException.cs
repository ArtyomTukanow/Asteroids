using System;
using JetBrains.Annotations;

namespace ECS.Exceptions
{
    public class TypeException: Exception
    {
        public TypeException([NotNull]Type expected, [CanBeNull]Type actual) : base(
            $"Type error. Expected: {expected.Name}, Actual: {actual?.Name}")
        {
        }
    }
}