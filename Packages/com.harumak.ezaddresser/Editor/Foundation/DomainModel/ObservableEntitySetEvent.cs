namespace EZAddresser.Editor.Foundation.DomainModel
{
    public readonly struct EntitySetAddEvent<TValue>
    {
        public readonly TValue Value;

        public EntitySetAddEvent(TValue value)
        {
            Value = value;
        }
    }

    public readonly struct EntitySetRemoveEvent<TValue>
    {
        public readonly TValue Value;

        public EntitySetRemoveEvent(TValue value)
        {
            Value = value;
        }
    }
}