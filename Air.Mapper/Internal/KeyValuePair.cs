﻿using System.Collections.Generic;

namespace Air.Mapper.Internal
{
    public struct KeyValue<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public KeyValuePair<TKey, TValue> ToKeyValuePair() =>
            new KeyValuePair<TKey, TValue>(Key, Value);
    }
}
