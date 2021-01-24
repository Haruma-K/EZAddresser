using System;
using System.Collections.Generic;
using EZAddresser.Editor.Foundation.Observable.ObservableProperty;
using UnityEngine;

namespace EZAddresser.Editor.Foundation.Observable.InteractiveProperty
{
    /// <summary>
    ///     ObservableProperty which allows interactive control by exposing only not notifiable setters to the outside.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class InteractiveProperty<T> : IInteractiveProperty<T>, IObservableProperty<T>, IDisposable
    {
        private readonly Subject<T> _onValueChangedSubject = new Subject<T>();

        [SerializeField] private T _value;

        public T Value
        {
            get => _value;
            set
            {
                var hasChanged = !EqualityComparer<T>.Default.Equals(value, _value);
                _value = value;
                if (hasChanged)
                {
                    _onValueChangedSubject.OnNext(_value);
                }
            }
        }

        public void SetValueAndNotify(T value)
        {
            _value = value;
            _onValueChangedSubject.OnNext(Value);
        }

        public void SetValueAndNotNotify(T packingMode)
        {
            _value = packingMode;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            observer.OnNext(_value);
            return _onValueChangedSubject.Subscribe(observer);
        }

        public void Dispose()
        {
            _onValueChangedSubject.Dispose();
        }
    }
}