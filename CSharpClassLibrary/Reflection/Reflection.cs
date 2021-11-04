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
        public int x { get; set; }
        public IList<ScoreSet> dd { get; set; }
        public IList<ScoreSet> z { get; set; }
        public IList<RankSet> n { get; set; }
    }

    public class ScoreSet
    {
        public int y { get; set; }
    }

    public class RankSet
    {
        public int s { get; set; }
        public int ss { get; set; }
        public IList<Test4> w { get; set; }
    }

    public class Test4
    {
        public int bb { get; set; }
        public int? test { get; set; }
        
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
                x = 4,
                dd = new List<ScoreSet>() 
                    { 
                        new ScoreSet() { y = 5 },
                        new ScoreSet(), 
                        new ScoreSet() { y = 3 },
                        new ScoreSet() { y = 10 }
                    },
                n = new List<RankSet>() 
                    { 
                        new RankSet() { s = 12, ss = 1 ,
                            w = new List<Test4>() { 
                                    new Test4() { bb = 100 }, 
                                    new Test4() { bb = 335 } 
                            } 
                        }, 
                        new RankSet() { s = 12, ss = 2,
                            w = new List<Test4> { 
                                    new Test4() { bb = 5 },
                                    new Test4() { bb = 123 } 
                            } 
                        },
                        new RankSet() { s = 23, ss = 22,
                            w = new List<Test4> { 
                                    new Test4() { bb = 15 }, 
                                    new Test4() { bb = 3435 }
                            }
                        }
                    },
                z = new List<ScoreSet>()
                {

                }
            };

            //var test2 = new RankSet() { w = new Test4() };

            List<ExpandoObject> csvPage = new ();
            csvPage.AttachFlatableObject(test);
            Console.WriteLine("test");
        }

        static void AttachFlatableObject(this IList<ExpandoObject> csvPage, object layeredObject, IDictionary<string, object> csvRow = null)
        {
            if (layeredObject.GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)//bitswise or
                        .Any(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)))
            {
                var IListProperties = layeredObject.GetType()
                                                .GetProperties()
                                                .Where(p => p.PropertyType.IsGenericType &&
                                                       p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>));

                foreach (var product in IListProperties.OfCartesianProductOf(layeredObject))
                {
                    if (csvRow == null)
                    {
                        csvRow = new ExpandoObject();
                        csvPage.Add(csvRow as ExpandoObject);
                    }
                    foreach (var (EnumerablePropertyInfo, Current) in product as IEnumerable<(PropertyInfo, object)>)
                    {
                        
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
