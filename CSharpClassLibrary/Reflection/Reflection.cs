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
        public IList<Rank> rank { get; set; }
    }

    public class Rank
    {
        public int e { get; set; }
        public int? f { get; set; }
        public IList<Price> price { get; set; }

    }

    public class Price
    {
        public int g { get; set; }

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
                            rank = new List<Rank>() {
                                        new Rank() {
                                            e = 100,
                                            price = new List<Price>()
                                            {
                                                new Price(),
                                                new Price(),
                                                new Price()
                                            }
                                        }, 
                                        new Rank() { 
                                            e = 335,
                                            price = new List<Price>()
                                            {
                                                new Price(),
                                                new Price()
                                            }
                                        } 
                            } 
                        }, 
                        new RankSet() { c = 12, d = 2,
                            rank = new List<Rank> { 
                                        new Rank() { 
                                            e = 123,
                                            price = new List<Price>()
                                            {
                                                new Price() { g = -24 },
                                                new Price()
                                            }
                                        } 
                            } 
                        },
                        new RankSet() { c = 23, d = 22,
                            rank = new List<Rank> { 
                                        new Rank() { 
                                            e = 15,
                                            price = new List<Price>()
                                            {
                                                new Price(),
                                                new Price()
                                            }
                                        }, 
                                        new Rank() { 
                                            e = 3435,
                                            price = new List<Price>()
                                            {
                                                new Price() { g = 10000 },
                                                new Price() { g = -500 }
                                            }
                                        }
                            }
                        }
                    }
            };

            var test2 = new RankSet() 
            { 
                c = 2, 
                d = 3, 
                rank = new List<Rank> {
                    new Rank() {
                        price = new List<Price>()
                            {
                                new Price() ,
                                new Price()
                            }
                        }
                }
            };

            List<ExpandoObject> csvPage = new();
            //csvPage.AttachFlatableObject(test);

            int count = 0;
            IDictionary<string, int> Dict = new Dictionary<string, int>();
            csvPage.AttachObjectAsDecomposed(test);
            //Console.WriteLine("============");
            //Console.WriteLine($"Count: {count}");
            //foreach (var keyValuePair in Dict)
            //{
            //    Console.WriteLine($"{keyValuePair.Key}: {keyValuePair.Value}");
            //}
            Console.WriteLine($"============================");
            foreach (var expandoObject in csvPage)
            {
                Console.Write($"---\n");
                foreach (var pair in expandoObject as IDictionary<string, object>)
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                }
                Console.Write($"---");
            }
        }

        static void Show(object test, ref int count, IDictionary<string, int> Dict)
        {
            foreach (var results in Decompose(test))
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

        static void AttachObjectAsDecomposed(this IList<ExpandoObject> csvPage, 
            object composedObject, 
            //ref int count*/, 
            //IDictionary<string, int> Dict, 
            IDictionary<string, object> csvRow = null,
            IDictionary<string, object> tempRow = null)
        {
            if (composedObject.GetType().GetInterfaces().Any(
                i => i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IList<>)))
            {
                IList<object> enumerableObject = (IList<object>)composedObject;
                for (int i =0; i < enumerableObject.Count; i++)
                {
                    csvPage.AttachObjectAsDecomposed(enumerableObject[i]);
                }
            }
            else
            {
                foreach (var decomposedObjects in composedObject.Decompose())
                {
                    if (csvRow == null)
                    {
                        if (tempRow != null)
                        {
                            csvRow = new ExpandoObject();
                            csvPage.Add(csvRow as ExpandoObject);
                            foreach (var keyValuePair in tempRow)
                            {
                                csvRow.Add(keyValuePair.Key, keyValuePair.Value);
                            }
                        }
                        else
                        {
                            csvRow = new ExpandoObject();
                            csvPage.Add(csvRow as ExpandoObject);
                        }
                    }
                    foreach (var (propertyInfo, value) in decomposedObjects as IEnumerable<(PropertyInfo propertyInfo, object value)>)
                    {
                        if (propertyInfo.PropertyType.IsValueType)
                        {
                            Console.WriteLine($"{propertyInfo.Name}: {value}, ");
                            if (!csvRow.TryAdd(propertyInfo.Name, value))
                            {
                                csvRow[propertyInfo.Name] = value;
                            }
                            //count++;
                            //if (Dict.TryGetValue(propertyInfo.Name, out int propertyInfoCount))
                            //{
                            //    Dict[propertyInfo.Name] = ++propertyInfoCount;
                            //}
                            //else
                            //{
                            //    Dict.Add(propertyInfo.Name, 1);
                            //}
                        }
                        else
                        {
                            csvPage.AttachObjectAsDecomposed(value, /*ref count, Dict,*/ csvRow, tempRow);
                        }
                    }
                    tempRow = csvRow;
                    csvRow = null;
                }
            }
            
        }

        static void AttachFlatableObject2(this IList<ExpandoObject> csvPage,
           object test, ref int count,
           IDictionary<string, int> Dict,
           IDictionary<string, object> csvRow = null)
        {
            foreach (var results in Decompose(test))
            {
                if (csvRow == null)
                {
                    csvRow = new ExpandoObject();
                    csvPage.Add(csvRow as ExpandoObject);
                }
                foreach (var (propertyInfo, value) in results as IEnumerable<(PropertyInfo, object)>)
                {
                    if (propertyInfo.PropertyType.IsValueType)
                    {
                        Console.WriteLine($"{propertyInfo.Name}: {value}, ");
                        if (!csvRow.TryAdd(propertyInfo.Name, value))
                        {

                        }
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
                        csvPage.AttachFlatableObject2(value, ref count, Dict, csvRow);
                    }
                }
                csvRow = null;
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
        static IEnumerable<object> Decompose(this object composedObject)
        {
            //CartesainProduct
            var propertiesInfo = composedObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var IObjectEnumeratorTupleArray = propertiesInfo
                  .Select(propertyInfo => (PropertyInfo: propertyInfo, Enumerator: GetObjectEnumerator(propertyInfo, composedObject)))
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
        public static IEnumerable<T> Flatten<T>(
        this IEnumerable<T> items,
        Func<T, IEnumerable<T>> getChildren)
        {
            var stack = new Stack<T>();
            foreach (var item in items)
                stack.Push(item);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;

                var children = getChildren(current);
                if (children == null) continue;

                foreach (var child in children)
                    stack.Push(child);
            }
        }
    }
}
