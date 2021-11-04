using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace CSharpClassLibrary.Reflection
{
    public class Test
    {
        public int x { get; set; }
        public IList<Test2> y { get; set; }
        public IList<Test2> z { get; set; }
    }

    public class Test2
    {
        public int y { get; set; }
    }

    public static class ReflectionTest
    {
        public static void Test()
        {
            var test = new Test()
            {
                x = 4,
                y = new List<Test2>() { new Test2() { y = 5 }, new Test2() },
                z = new List<Test2>() { new Test2() { y = 10 }, new Test2() }
            };
            var test3 = new List<ExpandoObject>();
            //Expand(ref test3,test);
            Expand(test);
        }
        static void Expand(object test)
        {
            if (test.GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)//bitswise or
                        .Any(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)))
            {


                List<ExpandoObject> csvPage = new List<ExpandoObject>();

                var IListProperties = test.GetType()
                    .GetProperties()
                    .Where(p => p.PropertyType.IsGenericType &&
                           p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>));
                //IListProperties
                //    .CartesianProduct(test)
                //    .Select(x => string.Join("", x.)).
                //foreach (var test2 in IListProperties.CartesianProduct(test))
                //{
                //    foreach(var o in )
                //    {
                //        Console.WriteLine(o);
                //    }

                //}

                var nonIListProperties = test.GetType()
                    .GetProperties()
                    .Where(p => !(p.PropertyType.IsGenericType &&
                                  p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))).ToArray();

                var properties = test.GetType().GetProperties();
            }
        }

        //static IEnumerable<(string, object)> CartesianProduct(this IEnumerable<PropertyInfo> properties, object test)
        //{
        //    var IObjectEnumeratorTupleArray = properties
        //           .Select(property => ( Name: property.Name, Enumerator: ((IEnumerable<object>)property.GetValue(test)).GetEnumerator()))
        //           //.Where(IListPropertyEnumerator => IListPropertyEnumerator.MoveNext())
        //           .ToArray();

        //    while (true)
        //    {
        //        // yield current values
        //        yield return IObjectEnumeratorTupleArray
        //            .Select(IObjectEnumeratorTuple => (IObjectEnumeratorTuple.Name, IObjectEnumeratorTuple.Enumerator.Current));

        //        // increase enumerators
        //        foreach (var IObjectEnumeratorTuple in IObjectEnumeratorTupleArray)
        //        {
        //            // reset the slot if it couldn't move next
        //            if (!IObjectEnumeratorTuple.Enumerator.MoveNext())//move next has side effect!!!(it moves then check)
        //            {
        //                // stop when the last enumerator resets
        //                if (IObjectEnumeratorTuple == IObjectEnumeratorTupleArray.Last())
        //                {
        //                    yield break; //this exit the loop
        //                }
        //                IObjectEnumeratorTuple.Enumerator.Reset();
        //                IObjectEnumeratorTuple.Enumerator.MoveNext();
        //                // move to the next enumerator if this reseted
        //                continue;
        //            }
        //                // we could increase the current enumerator without reset so stop here
        //            break;
        //        }
        //    }
        //}

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

    }
}
