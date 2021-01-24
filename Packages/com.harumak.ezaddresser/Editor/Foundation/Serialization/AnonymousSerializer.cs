using System;

namespace EZAddresser.Editor.Foundation.Serialization
{
    internal class AnonymousSerializer<TSerialized> : ISerializer<TSerialized>
    {
        private readonly Func<TSerialized, object> _deserialize;
        private readonly Func<object, TSerialized> _serialize;

        public AnonymousSerializer(Func<object, TSerialized> serialize, Func<TSerialized, object> deserialize)
        {
            _serialize = serialize;
            _deserialize = deserialize;
        }

        public TSerialized Serialize(object obj)
        {
            return _serialize.Invoke(obj);
        }

        public T Deserialize<T>(TSerialized serialized)
        {
            return (T) _deserialize.Invoke(serialized);
        }
    }

    internal class AnonymousSerializer<TDeserialized, TSerialized> : ISerializer<TDeserialized, TSerialized>
    {
        private readonly Func<TSerialized, TDeserialized> _deserialize;
        private readonly Func<TDeserialized, TSerialized> _serialize;

        public AnonymousSerializer(Func<TDeserialized, TSerialized> serialize,
            Func<TSerialized, TDeserialized> deserialize)
        {
            _serialize = serialize;
            _deserialize = deserialize;
        }

        public TSerialized Serialize(TDeserialized obj)
        {
            return _serialize.Invoke(obj);
        }

        public TDeserialized Deserialize(TSerialized serialized)
        {
            return _deserialize.Invoke(serialized);
        }
    }
}