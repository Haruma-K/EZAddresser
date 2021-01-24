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
    public partial class EntryOperationInfo : ValueObject
    {
        [SerializeField] private EntryCreateOrMoveOperationInfo _createOrMoveInfo;
        [SerializeField] private EntryRemoveOperationInfo _removeInfo;

        public EntryOperationInfo(EntryCreateOrMoveOperationInfo createOrMoveInfo, EntryRemoveOperationInfo removeInfo)
        {
            _createOrMoveInfo = createOrMoveInfo;
            _removeInfo = removeInfo;
        }

        public EntryCreateOrMoveOperationInfo CreateOrMoveInfo => _createOrMoveInfo;

        public EntryRemoveOperationInfo RemoveInfo => _removeInfo;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CreateOrMoveInfo;
            yield return RemoveInfo;
        }
    }
}
