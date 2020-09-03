using Air.Mapper;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchmark
{
    public static class CollectionToArray
    {
        //public static T[] EnumerableToArray<T>(IEnumerable<T> enumerable)
        //    => enumerable.ToArray();

        //public static T[] ListToArray<T>(List<T> list)
        //    => list.ToArray();

        //public static T[] QueueToArray<T>(Queue<T> queue)
        //{
        //    if (queue == null)
        //        return null;

        //    T[] entries = new T[queue.Count];
        //    queue.CopyTo(entries, 0);
        //    return entries;
        //}

        //public static T[] HashSetCopyToArray<T>(HashSet<T> hashset)
        //{
        //    if (hashset == null)
        //        return null;

        //    T[] entries = new T[hashset.Count];
        //    hashset.CopyTo(entries, 0);
        //    return entries;
        //}

        //public static T[] HashSetToArray<T>(HashSet<T> hashset)
        //{
        //    if (hashset == null)
        //        return null;

        //    return hashset.ToArray();
        //}

        //public static int HashSet_LoopForEach<T>(HashSet<T> hashset)
        //{
        //    int returnValue = 0;

        //    foreach (T item in hashset)
        //        if (item != null)
        //            returnValue++;

        //    return returnValue;
        //}

        //public static int HashSet_CopyToLoopIndex<T>(HashSet<T> hashset)
        //{
        //    int returnValue = 0;
        //    T[] array = Air.Reflection.Emit.ILGenerator.Converters.ToArray(hashset);
            
        //    for (int i = 0; i < array.Length; i++)
        //        if (array[i] != null)
        //            returnValue++;

        //    return returnValue;
        //}

        //public static int HashSet_ToArrayLoopIndex<T>(HashSet<T> hashset)
        //{
        //    int returnValue = 0;
        //    T[] array = hashset.ToArray();

        //    for (int i = 0; i < array.Length; i++)
        //        if (array[i] != null)
        //            returnValue++;

        //    return returnValue;
        //}
    

        //public static KeyValuePair<int, T>[] DictionaryCopyToArray<T>(Dictionary<int, T> source)
        //{
        //    if (source == null)
        //        return null;

        //    KeyValuePair<int, T>[] entries = new KeyValuePair<int, T>[source.Count];
        //    ((ICollection<KeyValuePair<int, T>>)source).CopyTo(entries, 0);
        //    return entries;
        //}

        //public static KeyValuePair<int, T>[] DictionaryToArray<T>(Dictionary<int, T> source)
        //{
        //    if (source == null)
        //        return null;

        //    return source.ToArray();
        //}

        //public static int Dictionary_LoopForEach<T>(Dictionary<int, T> source)
        //{
        //    int returnValue = 0;

        //    foreach (var item in source)
        //        if (item.Key > 0)
        //            returnValue++;

        //    return returnValue;
        //}

        //public static int Dictionary_LoopEnumeratorMoveNext<T>(Dictionary<int, T> source)
        //{
        //    int returnValue = 0;
        //    var enumerator = source.GetEnumerator();

        //    while (enumerator.MoveNext())
        //        if (enumerator.Current.Key > 0)
        //            returnValue++;

        //    return returnValue;
        //}


        //public static int Dictionary_LoopEnumerableEnumeratorMoveNext<T>(Dictionary<int, T> source)
        //{
        //    int returnValue = 0;
        //    var enumerator = ((ICollection<KeyValuePair<int, T>>)source).GetEnumerator();

        //    while (enumerator.MoveNext())
        //        if (enumerator.Current.Key > 0)
        //            returnValue++;

        //    return returnValue;
        //}

        //public static int Dictionary_CopyToLoopIndex<T>(Dictionary<int, T> source)
        //{
        //    int returnValue = 0;
        //    KeyValuePair<int, T>[] array = Air.Reflection.Emit.ILGenerator.Converters.ToArray(source);
            
        //    for (int i = 0; i < array.Length; i++)
        //        if (array[i].Key > 0)
        //            returnValue++;

        //    return returnValue;
        //}


        //public static int Dictionary_ICollectionToArrayLoopIndex<T>(Dictionary<int, T> source)
        //{
        //    int returnValue = 0;
        //    KeyValuePair<int, T>[] array = Air.Reflection.Emit.ILGenerator.Converters.ToArray(source);

        //    for (int i = 0; i < array.Length; i++)
        //        if (array[i].Key > 0)
        //            returnValue++;

        //    return returnValue;
        //}

        //public static int Dictionary_ToArrayLoopIndex<T>(Dictionary<int, T> source)
        //{
        //    int returnValue = 0;
        //    KeyValuePair<int, T>[] array = source.ToArray();

        //    for (int i = 0; i < array.Length; i++)
        //        if (array[i].Key > 0)
        //            returnValue++;

        //    return returnValue;
        //}


        //public static int Queue_LoopForEach<T>(Queue<T> source)
        //{
        //    int returnValue = 0;

        //    foreach (var item in source)
        //        if (item != null)
        //            returnValue++;

        //    return returnValue;
        //}

        //public static int Queue_CopyToLoopIndex<T>(Queue<T> source)
        //{
        //    int returnValue = 0;
        //    T[] array = source.ToArray();

        //    for (int i = 0; i < array.Length; i++)
        //        if (array[i] != null)
        //            returnValue++;

        //    return returnValue;
        //}


        //public static int IEnumerable_IterateForLoop<T>(IEnumerable<T> list)
        //{
        //    int returnValue = 0;

        //    foreach (T item in list)
        //    {
        //        if (item != null)
        //            returnValue++;
        //    }

        //    return returnValue;
        //}

        //public static int IEnumerable_IterateForLoop_v2<T>(IEnumerable<T> list)
        //{
        //    int returnValue = 0;
        //    IEnumerator<T> enumerator = list.GetEnumerator();

        //    while (enumerator.MoveNext())
        //    {
        //        T item = enumerator.Current;
        //        if (item != null)
        //            returnValue++;
        //    }

        //    return returnValue;
        //}

        //public static int IEnumerable_ToArray_IterateForLoop<T>(IEnumerable<T> list)
        //{
        //    var source = new List<TC0_I0_Members>();

        //    List<TC0_I0_Members> destination;
        //    if (source != null)
        //    {
        //        int i = 0;
        //        int len = source.Count;
        //        destination = new List<TC0_I0_Members>(len);

        //        while (i < len)
        //        {
        //            destination.Add(Mapper<TC0_I0_Members, TC0_I0_Members>.Map(source[i]));
        //            i++;
        //        }
        //    }
        //    else
        //    {
        //        destination = null;
        //    }

        //    return destination.Count;

        //}

        //public static int IEnumerable_ToArray_IterateForLoop_v2<T>(IEnumerable<T> list)
        //{
        //    var source = new List<TC0_I0_Members>();

        //    List<TC0_I0_Members> destination;
        //    if (source != null)
        //    {
        //        int i = 0;
        //        int len = source.Count;
        //        destination = new List<TC0_I0_Members>(len);

        //        var mapper = Mapper<TC0_I0_Members, TC0_I0_Members>.CompiledFunc;

        //        while (i < len)
        //        {
        //            destination.Add(mapper(source[i]));
        //            i++;
        //        }
        //    }
        //    else
        //    {
        //        destination = null;
        //    }

        //    return destination.Count;

        //}


        //public static int IEnumerable_ToArray_IterateForLoop2<T>(IEnumerable<T> list)
        //{
        //    var source = new List<TC0_I0_Members>();

        //    TC0_I0_Members[] destination;
        //    if (source != null)
        //    {
        //        int i = 0;
        //        int len = source.Count;
        //        destination = new TC0_I0_Members[len];

        //        while (i < len)
        //        {
        //            destination[i] = Mapper<TC0_I0_Members, TC0_I0_Members>.Map(source[i]);
        //            i++;
        //        }
        //    }
        //    else
        //    {
        //        destination = null;
        //    }


        //    return destination.Length;
        //}


        //public static AccountDto ToAccountDto(Account account)
        //{
        //    AccountDto result = null;
        //    if (account == null)
        //        return result;
            
        //    result = new AccountDto
        //    {
        //        DeliveryAddresses = account.DeliveryAddresses == null ? null :
        //        new List<AddressDto>()
        //    };

        //    return result;
        //}


        //public static int List_IterateForLoop<T>(List<T> list)
        //{
        //    int returnValue = 0;

        //    for (int i = 0; i < list.Count; i++)
        //        if (list[i] != null)
        //            returnValue++;

        //    return returnValue;
        //}

        //public static int List_ToArrayIterateForLoop<T>(List<T> list)
        //{
        //    int returnValue = 0;
        //    T[] array = list.ToArray();

        //    for (int i = 0; i < array.Length; i++)
        //        if (array[i] != null)
        //            returnValue++;

        //    return returnValue;
        //}
    }
}
