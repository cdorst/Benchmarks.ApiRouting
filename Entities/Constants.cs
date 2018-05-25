namespace Entities
{
    public static class Constants
    {
        public static readonly Entity EntityConstant = new Entity
        {
            Id = 5,
            ForeignKeyEntityOneId = 5000,
            ForeignKeyEntityTwoId = 5001,
            Flags = EntityFlags.Foo
        };
    }
}
