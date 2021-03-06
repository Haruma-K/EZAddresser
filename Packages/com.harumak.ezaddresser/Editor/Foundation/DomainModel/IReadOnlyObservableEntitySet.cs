﻿using System;
using EZAddresser.Editor.Foundation.Observable;

namespace EZAddresser.Editor.Foundation.DomainModel
{
    public interface IReadOnlyObservableEntitySet<TItem> : IReadOnlyObservableEntitySet<TItem, Empty>
        where TItem : Entity
    {
    }

    public interface IReadOnlyObservableEntitySet<TItem, TEmpty> : IReadOnlyEntitySet<TItem>
        where TItem : Entity
    {
        /// <summary>
        ///     The observable that is called when a item was added.
        /// </summary>
        IObservable<EntitySetAddEvent<TItem>> ObservableAdd { get; }

        /// <summary>
        ///     The observable that is called when a item was removed.
        /// </summary>
        IObservable<EntitySetRemoveEvent<TItem>> ObservableRemove { get; }

        /// <summary>
        ///     The observable that is called when items was cleared.
        /// </summary>
        IObservable<TEmpty> ObservableClear { get; }
    }
}