using System;

namespace Entities
{
    [Flags]
    public enum EntityFlags : byte
    {
        None = 0,
        Foo,
        Bar
    }
}
