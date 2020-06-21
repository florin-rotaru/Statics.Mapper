using System.Collections.Generic;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        public static SortedList<DK, D> ToSortedList<SK, DK>(IEnumerable<KeyValuePair<SK, S>> source)
        {
            SortedList<DK, D> returnValue = null;
            if (source == null)
                return returnValue;

            returnValue = new SortedList<DK, D>();

            foreach (KeyValuePair<SK, S> sourceEntry in source)
                returnValue.Add(
                    Mapper<SK, DK>.Map(sourceEntry.Key),
                    CompiledFunc(sourceEntry.Value));

            return returnValue;
        }
    }
}
