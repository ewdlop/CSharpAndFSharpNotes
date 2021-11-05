using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace CSharpClassLibrary.Reflection
{
    public class ScoreCard
    {
        public int a { get; set; }
        public IList<ScoreSet> scoreSets { get; set; }
        public IList<RankSet> rankSets { get; set; }
    }

    public class ScoreSet
    {
        public int b { get; set; }
    }

    public class RankSet
    {
        public int c { get; set; }
        public int d { get; set; }
        public IList<Test> test { get; set; }
    }

    public class Test
    {
        public int e { get; set; }
        public int? f { get; set; }
        
    }

    public struct GG
    {

    }

    public static class Reflection
    {
        public static void Test()
        {
            var test = new ScoreCard()
            {
                a = 4,
                scoreSets = new List<ScoreSet>() 
                    { 
                        new ScoreSet() { b = 5 },
                        new ScoreSet(), 
                        new ScoreSet() { b = 3 },
                        new ScoreSet() { b = 10 }
                    },
                rankSets = new List<RankSet>() 
                    { 
                        new RankSet() { c = 12, d = 1 ,
                            test = new List<Test>() { 
                                        new Test() { e = 100 }, 
                                        new Test() { e = 335 } 
                            } 
                        }, 
                        new RankSet() { c = 12, d = 2,
                            test = new List<Test> { 
                                        new Test() { e = 5 },
                                        new Test() { e = 123 } 
                            } 
                        },
                        new RankSet() { c = 23, d = 22,
                            test = new List<Test> { 
                                        new Test() { e = 15 }, 
                                        new Test() { e = 3435 }
                            }
                        }
                    }
            };

            //var test2 = new RankSet() { w = new Test4() };
            int count = 0;
            IDictionary<string, int> Dict = new Dictionary<string, int>();
            Show(test, ref count, Dict);
            Console.WriteLine("============");
            Console.WriteLine(count);
            foreach (var pair in Dict)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
            //List<ExpandoObject> csvPage = new ();
            //csvPage.AttachFlatableObject(test);
            //Console.WriteLine("test");
        }

        static void Show(object test, ref int count, IDictionary<string, int> Dict)
        {
            foreach (var results in CartesianProduct(test))
            {
                foreach (var (propertyInfo, value) in results as IEnumerable<(PropertyInfo, object)>)
                {
                    if (propertyInfo.PropertyType.IsValueType)
                    {
                        Console.WriteLine($"{propertyInfo.Name}: {value}, ");
                        count++;
                        if (Dict.TryGetValue(propertyInfo.Name, out int propertyInfoCount))
                        {
                            Dict[propertyInfo.Name] = ++propertyInfoCount;
                        }
                        else
                        {
                            Dict.Add(propertyInfo.Name, 1);
                        }
                    }
                    else
                    {
                        Show(value, ref count, Dict);
                    }
                }
            }
        }

        static void AttachFlatableObject(this IList<ExpandoObject> csvPage, object layeredObject, IDictionary<string, object> csvRow = null)
        {
            if (layeredObject.GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)//bitswise or
                        .Any(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)))
            {
                var IListProperties = layeredObject.GetType()
                                                 .GetProperties(BindingFlags.Public | BindingFlags.Instance);
                                                //.Where(p => p.PropertyType.IsGenericType &&
                                                //       p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>));

                foreach (var product in IListProperties.OfCartesianProductOf(layeredObject))
                {
                    if (csvRow == null)
                    {
                        csvRow = new ExpandoObject();
                        csvPage.Add(csvRow as ExpandoObject);
                    }
                    foreach (var (EnumerablePropertyInfo, Current) in product as IEnumerable<(PropertyInfo, object)>)
                    {
                        Console.WriteLine(EnumerablePropertyInfo.Name);
                        if (Current.GetType()
                            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Any(p => p.PropertyType.IsGenericType &&
                                      p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)))
                        {
                            csvPage.AttachFlatableObject(Current, csvRow);
                        }
                        else
                        {
                            var CurrentNonIListProperties = Current.GetType()
                                .GetProperties()
                                .Where(p => !(p.PropertyType.IsGenericType &&
                                              p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))).ToArray();
                            for (int i = 0; i < CurrentNonIListProperties.Length; i++)
                            {
                                if(CurrentNonIListProperties[i].PropertyType.IsValueType)
                                {
                                    Console.WriteLine(CurrentNonIListProperties[i].Name);
                                    csvRow.Add(CurrentNonIListProperties[i].Name, CurrentNonIListProperties[i].GetValue(Current));
                                }
                                else
                                {
                                    csvPage.AttachFlatableObject(CurrentNonIListProperties[i].GetValue(Current), csvRow);
                                }
                            }
                        }

                    }
                    var nonIListProperties = layeredObject.GetType()
                        .GetProperties()
                        .Where(p => !(p.PropertyType.IsGenericType &&
                                  p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))).ToArray();
                    for (int i = 0; i < nonIListProperties.Length; i++)
                    {
                        if (nonIListProperties[i].PropertyType.IsValueType)
                        {
                            Console.WriteLine(nonIListProperties[i].Name);
                            csvRow.Add(nonIListProperties[i].Name, nonIListProperties[i].GetValue(layeredObject));
                        }
                        else
                        {
                            csvPage.AttachFlatableObject(nonIListProperties[i].GetValue(layeredObject), csvRow);
                        }
                    }
                    csvRow = null;
                }
            }
            //else if()
            //{
            //    if (csvRow == null)
            //    {
            //        csvRow = new ExpandoObject();
            //        csvPage.Add(csvRow as ExpandoObject);
            //    }
            //}
            else
            {
                if (csvRow == null)
                {
                    csvRow = new ExpandoObject();
                    csvPage.Add(csvRow as ExpandoObject);
                }
                var nonIListProperties = layeredObject.GetType()
                    .GetProperties()
                    .Where(p => !(p.PropertyType.IsGenericType &&
                                  p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))).ToArray();
                for (int i = 0; i < nonIListProperties.Length; i++)
                {
                    if (nonIListProperties[i].PropertyType.IsValueType)
                    {
                        csvRow.Add(nonIListProperties[i].Name, nonIListProperties[i].GetValue(layeredObject));
                    }
                    else
                    {
                        csvPage.AttachFlatableObject(nonIListProperties[i].GetValue(layeredObject), csvRow);
                    }
                }
                csvRow = null;
            }
        }
        static IEnumerator GetObjectEnumerator(PropertyInfo propertyInfo, object original)
        {
            if(propertyInfo.GetValue(original) as IEnumerable<object> is null)
            {
                var test = new object[] { propertyInfo.GetValue(original) };
                return test.AsEnumerable().GetEnumerator();
            }
            return (propertyInfo.GetValue(original) as IEnumerable<object>).GetEnumerator();
        }
        static IEnumerable<object> CartesianProduct(object original)
        {
            var propertiesInfo = original.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var IObjectEnumeratorTupleArray = propertiesInfo
                  .Select(propertyInfo => (PropertyInfo: propertyInfo, Enumerator: GetObjectEnumerator(propertyInfo, original)))
                  .Where(IObjectEnumeratorTuple => IObjectEnumeratorTuple.Enumerator.MoveNext())
                  .ToArray();
            while (true)
            {
                // yield current values
                yield return IObjectEnumeratorTupleArray
                    .Select(IObjectEnumeratorTuple => (IObjectEnumeratorTuple.PropertyInfo, IObjectEnumeratorTuple.Enumerator.Current));

                // increase enumerators
                foreach (var IObjectEnumeratorTuple in IObjectEnumeratorTupleArray)
                {
                    // reset the slot if it couldn't move next
                    if (!IObjectEnumeratorTuple.Enumerator.MoveNext())//move next has side effect!!!(it moves then check)
                    {
                        // stop when the last enumerator resets
                        if (IObjectEnumeratorTuple == IObjectEnumeratorTupleArray.Last())
                        {
                            yield break; //this exit the loop
                        }
                        IObjectEnumeratorTuple.Enumerator.Reset();
                        IObjectEnumeratorTuple.Enumerator.MoveNext();
                        // move to the next enumerator if this reseted
                        continue;
                    }
                    // we could increase the current enumerator without reset so stop here
                    break;
                }
            }
        }

        static IEnumerable<object> OfCartesianProductOf(this IEnumerable<PropertyInfo> propertiesInfo, object original)
        {
            var IObjectEnumeratorTupleArray = propertiesInfo
                   .Select(propertyInfo => ( PropertyInfo: propertyInfo, Enumerator: (propertyInfo.GetValue(original) as IEnumerable<object>).GetEnumerator()))
                   .Where(IObjectEnumeratorTuple => IObjectEnumeratorTuple.Enumerator.MoveNext())
                   .ToArray();

            while (true)
            {
                // yield current values
                yield return IObjectEnumeratorTupleArray
                    .Select(IObjectEnumeratorTuple => (IObjectEnumeratorTuple.PropertyInfo, IObjectEnumeratorTuple.Enumerator.Current));

                // increase enumerators
                foreach (var IObjectEnumeratorTuple in IObjectEnumeratorTupleArray)
                {
                    // reset the slot if it couldn't move next
                    if (!IObjectEnumeratorTuple.Enumerator.MoveNext())//move next has side effect!!!(it moves then check)
                    {
                        // stop when the last enumerator resets
                        if (IObjectEnumeratorTuple == IObjectEnumeratorTupleArray.Last())
                        {
                            yield break; //this exit the loop
                        }
                        IObjectEnumeratorTuple.Enumerator.Reset();
                        IObjectEnumeratorTuple.Enumerator.MoveNext();
                        // move to the next enumerator if this reseted
                        continue;
                    }
                        // we could increase the current enumerator without reset so stop here
                    break;
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            if (sequences == null)
            {
                return null;
            }

            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };

            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) => accumulator.SelectMany(
                    accseq => sequence,
                    (accseq, item) => accseq.Concat(new[] { item })));
        }

        public static IEnumerable Flatten(params object[] objects)
        {
            // Can't easily get varargs behaviour with IEnumerable
            return Flatten((IEnumerable)objects);
        }

        public static IEnumerable Flatten(IEnumerable enumerable)
        {
            foreach (object element in enumerable)
            {
                IEnumerable candidate = element as IEnumerable;
                if (candidate != null)
                {
                    yield return Flatten(candidate);
                }
                else
                {
                    yield return element;
                }
            }
        }

    }
}
