﻿using UnityEngine;

namespace EZAddresser.Editor.Foundation.Serialization
{
    /// <summary>
    ///     Json serializer implemented using <see cref="UnityEngine.JsonUtility" />.
    /// </summary>
    internal class UnityJsonSerializer : ISerializer<string>
    {
        public string Serialize(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public T Deserialize<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }

    /// <summary>
    ///     Json serializer implemented using <see cref="UnityEngine.JsonUtility" />.
    /// </summary>
    internal class UnityJsonSerializer<T> : ISerializer<T, string>
    {
        public bool PrettyPrint { get; set; }

        public string Serialize(T obj)
        {
            return JsonUtility.ToJson(obj, PrettyPrint);
        }

        public T Deserialize(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}