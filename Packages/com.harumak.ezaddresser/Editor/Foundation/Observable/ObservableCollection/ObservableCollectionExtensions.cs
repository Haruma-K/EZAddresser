namespace EZAddresser.Editor.Foundation.Observable.ObservableCollection
{
    public static class ObservableCollectionExtensions
    {
        public static IReadOnlyObservableList<TValue> ToReadOnly<TValue>(this IObservableList<TValue> self)
        {
            return (IReadOnlyObservableList<TValue>) self;
        }

        public static IReadOnlyObservableDictionary<TKey, TValue> ToReadOnly<TKey, TValue>
            (this IObservableDictionary<TKey, TValue> self)
        {
            return (IReadOnlyObservableDictionary<TKey, TValue>) self;
        }
    }
}