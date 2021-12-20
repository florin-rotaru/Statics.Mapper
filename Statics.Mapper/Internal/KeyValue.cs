using System.Collections.Generic;

namespace Statics.Mapper.Internal
{
    internal struct KeyValue<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public KeyValuePair<TKey, TValue> ToKeyValuePair() =>
            new(Key, Value);
    }
}
