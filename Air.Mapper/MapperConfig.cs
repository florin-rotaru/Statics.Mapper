using System;
using System.Collections.Generic;
using System.Linq;

namespace Air.Mapper
{
    public class MapperConfig<S, D>
    {
        private static List<IMapOption> Options = null;

        public static bool IsConfigured => Options != null;

        public static void SetOptions(Action<MapOptions<S, D>> options, bool overwrite = false)
        {
            if (IsConfigured && !overwrite)
                throw new InvalidOperationException($"{nameof(Options)} for {typeof(S).Name} and {typeof(D).Name} already set!");

            MapOptions<S, D> mapOptions = new MapOptions<S, D>();
            options.Invoke(mapOptions);
            Options = mapOptions.Get().ToList();
        }

        public static IEnumerable<IMapOption> GetOptions() => Options;
    }
}
