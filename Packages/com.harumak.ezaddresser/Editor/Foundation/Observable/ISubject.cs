using System;

namespace EZAddresser.Editor.Foundation.Observable
{
    internal interface ISubject<T> : IObserver<T>, IObservable<T>
    {
    }
}