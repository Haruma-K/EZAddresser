using System;
using System.Collections.Generic;

namespace EZAddresser.Editor.Foundation.DomainModel
{
    internal sealed class CollectionItemConverter<TSrc, TDst>
    {
        private readonly Dictionary<TSrc, TDst> _cache = new Dictionary<TSrc, TDst>();
        private readonly Func<TSrc, TDst> _converter;

        public CollectionItemConverter(Func<TSrc, TDst> converter)
        {
            _converter = converter;
        }

        public TDst Invoke(TSrc source)
        {
            if (_cache.TryGetValue(source, out var dst))
            {
                return dst;
            }

            dst = _converter.Invoke(source);
            _cache.Add(source, dst);
            return dst;
        }
    }
}