using System;
using static Common.Extensions.Memory.IntegerReadOnlyMemoryMapper;
using static Common.Extensions.Memory.IntegerReadOnlySpanMapper;

namespace Entities
{
    public class Entity
    {
        public int Id { get; set; }

        public ForeignKeyEntityOne ForeignKeyEntityOne { get; set; }
        public long ForeignKeyEntityOneId { get; set; }

        public ForeignKeyEntityTwo ForeignKeyEntityTwo { get; set; }
        public short ForeignKeyEntityTwoId { get; set; }

        public EntityFlags Flags { get; set; }

        public ReadOnlyMemory<byte> ToReadonlyMemory()
            => MapMemory(ForeignKeyEntityOneId, ForeignKeyEntityTwoId, (byte)Flags);

        public ReadOnlySpan<byte> ToReadonlySpan()
            => MapSpan(ForeignKeyEntityOneId, ForeignKeyEntityTwoId, (byte)Flags);
    }
}
