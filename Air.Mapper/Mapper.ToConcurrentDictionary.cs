using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        public static ConcurrentDictionary<DK, D> ToConcurrentDictionary<SK, DK>(IEnumerable<KeyValuePair<SK, S>> source)
        {
            ConcurrentDictionary<DK, D> returnValue = null;
            if (source == null)
                return returnValue;

            returnValue = new ConcurrentDictionary<DK, D>();

            foreach (KeyValuePair<SK, S> sourceEntry in source)
                returnValue.TryAdd(
                    Mapper<SK, DK>.Map(sourceEntry.Key),
                    CompiledFunc(sourceEntry.Value));

            return returnValue;
        }
    }
}
