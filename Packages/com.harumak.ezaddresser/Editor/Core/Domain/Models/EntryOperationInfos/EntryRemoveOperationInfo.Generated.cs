using System;
using System.Collections.Generic;
using UnityEngine;
using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Foundation.Observable.ObservableProperty;
using EZAddresser.Editor.Foundation.Observable.ObservableCollection;
using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Models.EntryOperationInfos
{
    [Serializable]
    public partial class EntryRemoveOperationInfo : ValueObject
    {
        [SerializeField] private string _assetPath;

        public EntryRemoveOperationInfo(string assetPath)
        {
            _assetPath = assetPath;
        }

        public string AssetPath => _assetPath;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AssetPath;
        }
    }
}
