using System.Collections.Generic;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        public static SortedDictionary<DK, D> ToSortedDictionary<SK, DK>(IEnumerable<KeyValuePair<SK, S>> source)
        {
            SortedDictionary<DK, D> returnValue = null;
            if (source == null)
                return returnValue;

            returnValue = new SortedDictionary<DK, D>();

            foreach (KeyValuePair<SK, S> sourceEntry in source)
                returnValue.Add(
                    Mapper<SK, DK>.Map(sourceEntry.Key),
                    CompiledFunc(sourceEntry.Value));

            return returnValue;
        }
    }
}
