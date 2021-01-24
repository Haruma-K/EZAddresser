using System;

namespace EZAddresser.Editor.Foundation.Observable
{
    public static class DisposableExtensions
    {
        internal static void DisposeWith(this IDisposable self, CompositeDisposable compositeDisposable)
        {
            compositeDisposable.Add(self);
        }
    }
}