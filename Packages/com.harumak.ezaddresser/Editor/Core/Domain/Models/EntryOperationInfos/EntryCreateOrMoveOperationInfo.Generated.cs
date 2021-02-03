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
    public partial class EntryCreateOrMoveOperationInfo : ValueObject
    {
        [SerializeField] private string _assetPath;
        [SerializeField] private string _address;
        [SerializeField] private string _groupName;
        [SerializeField] private string _groupTemplateGuid;
        [SerializeField] private string[] _labels;

        public EntryCreateOrMoveOperationInfo(string assetPath, string address, string groupName,
            string groupTemplateGuid, string[] labels)
        {
            _assetPath = assetPath;
            _address = address;
            _groupName = groupName;
            _groupTemplateGuid = groupTemplateGuid;
            _labels = labels;
        }

        public string AssetPath => _assetPath;

        public string Address => _address;

        public string GroupName => _groupName;

        public string GroupTemplateGuid => _groupTemplateGuid;

        public string[] Labels => _labels;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AssetPath;
            yield return Address;
            yield return GroupName;
            yield return GroupTemplateGuid;
            yield return Labels;
        }
    }
}
