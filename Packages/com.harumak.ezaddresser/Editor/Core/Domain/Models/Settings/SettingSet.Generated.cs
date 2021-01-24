using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Models.Settings
{
    public partial interface ISettingSet : IObservableEntitySet<Setting>
    {
    }

    public partial interface IReadOnlySettingSet : IReadOnlyObservableEntitySet<Setting>
    {
    }
    
    /// <summary>
    /// Set of the <see cref="Setting"/>
    /// </summary>
    [Serializable]
    public partial class SettingSet : ObservableEntitySet<Setting>, ISettingSet, IReadOnlySettingSet
    {
        public SettingSet() : base()
        {
        }

        public SettingSet(IEnumerable<Setting> entities) : base(entities)
        {
        }
    }
}
