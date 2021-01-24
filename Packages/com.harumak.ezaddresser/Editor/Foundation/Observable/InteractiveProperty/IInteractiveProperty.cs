using System;

namespace EZAddresser.Editor.Foundation.Observable.InteractiveProperty
{
    public interface ISetOnlyInteractiveProperty<T> : IObservable<T>
    {
        /// <summary>
        ///     Current value.
        /// </summary>
        T Value { get; }

        /// <summary>
        ///     Set a value and not notify.
        /// </summary>
        /// <param name="value"></param>
        void SetValueAndNotNotify(T value);
    }

    public interface IInteractiveProperty<T> : ISetOnlyInteractiveProperty<T>
    {
        /// <summary>
        ///     Current value.
        /// </summary>
        new T Value { get; set; }

        /// <summary>
        ///     Set a value and force notify.
        /// </summary>
        /// <param name="value"></param>
        void SetValueAndNotify(T value);
    }
}