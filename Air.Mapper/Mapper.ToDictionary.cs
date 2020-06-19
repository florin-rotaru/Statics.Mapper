using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Air.Mapper
{
    public static partial class Mapper<S, D>
    {
        public static Dictionary<DK, D> ToDictionary<SK, DK>(IDictionary<SK, S> source)
        {
            Dictionary<DK, D> returnValue = null;
            if (source == null)
                return returnValue;

            returnValue = new Dictionary<DK, D>();

            foreach (KeyValuePair<SK, S> sourceEntry in source)
                returnValue.Add(
                    Mapper<SK, DK>.Map(sourceEntry.Key),
                    CompiledFunc(sourceEntry.Value));

            return returnValue;
        }

        public static SortedDictionary<DK, D> ToSortedDictionary<SK, DK>(IDictionary<SK, S> source)
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

        public static SortedList<DK, D> ToSortedList<SK, DK>(IDictionary<SK, S> source)
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

        public static ConcurrentDictionary<DK, D> ToConcurrentDictionary<SK, DK>(IDictionary<SK, S> source)
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
