using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZAddresser.Editor.Foundation.DomainModel
{
    [Serializable]
    public class EntityId : ValueObject
    {
        [SerializeField] private string _value;

        public EntityId(string value)
        {
            _value = value;
        }

        public string Value => _value;

        public static EntityId Create()
        {
            return new EntityId(Guid.NewGuid().ToString());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}