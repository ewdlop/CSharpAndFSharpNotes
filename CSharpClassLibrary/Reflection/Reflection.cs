using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSharpClassLibrary.Reflection
{
    public class Player
    {
        public string? Name { get; set; }
        public int Health { get; set; }
        public int Gold { get; set; }
        public IList<MiscItem>? MiscItems { get; set; }
        public IList<EquipmentSet>? EquipmentSets { get; set; }

    }

    public class MiscItem
    { 
        public string? MiscItemName { get; set; }
        public int MiscItemWorthValue { get; set; }
    }

    public class EquipmentSet
    {
        public string? EquipmentSetName { get; set; }
        public int? Priceless { get; set; }
        public IList<Equipment>? Equipments { get; set; }
    }

    public class Equipment
    {
        public string? Name { get; set; }
        public int EquipmentWorthValue { get; set; }
        public IList<Stat>? Stats { get; set; }
    }

    public class Stat
    {
        public string? EquipmentStatName { get; set; }
        public int EquipmentStatBonus { get; set; }
    }

    public static class Reflection
    {
        public static void Test()
        {
            var player = new Player()
            {
                Name = "Random Dude",
                Health = 4,
                MiscItems = new List<MiscItem>()
                {
                    new MiscItem() { 
                        MiscItemName = "Random item",
                        MiscItemWorthValue = 5
                    },
                    new MiscItem() {
                        MiscItemName = "Empty Wallet",
                        MiscItemWorthValue = 5
                    },
                    new MiscItem() { 
                        MiscItemName = "Unknown",
                        MiscItemWorthValue = -99
                    },
                    new MiscItem() { 
                        MiscItemName = "Apple",
                        MiscItemWorthValue = 33333
                    }
                },
                EquipmentSets = new List<EquipmentSet>()
                {
                    new EquipmentSet() { 
                        EquipmentSetName = "SpeedRunSet",
                        Equipments = new List<Equipment>() {
                            new Equipment() {
                                Name ="IFrame",
                                EquipmentWorthValue = 100,
                                Stats = new List<Stat>()
                                {
                                    new Stat() { EquipmentStatName ="DamageTaken", EquipmentStatBonus = 0 },
                                }
                            },
                            new Equipment() {
                                Name = "Bunny Hop Physics",
                                EquipmentWorthValue = 335,
                                Stats = new List<Stat>()
                                {
                                    new Stat() { EquipmentStatName ="Weight", EquipmentStatBonus = -999 },
                                    new Stat() { EquipmentStatName ="SpeedBonus", EquipmentStatBonus = 9999 },
                                }
                            }
                        }
                    },
                    new EquipmentSet() { 
                        EquipmentSetName = "Very Basic",
                        Priceless = -1,
                        Equipments = new List<Equipment> {
                            new Equipment() {
                                Name = "Sword",
                                EquipmentWorthValue = 1234,
                                Stats = new List<Stat>()
                                {
                                    new Stat() { 
                                        EquipmentStatName ="Damage",
                                        EquipmentStatBonus = 1 
                                    }
                                }
                            }
                        }
                    },
                    new EquipmentSet() { 
                        EquipmentSetName = "Me Poor Man's Dream",
                        Equipments = new List<Equipment> {
                            new Equipment() {
                                Name = "Sports Car",
                                EquipmentWorthValue = 99999,
                                Stats = new List<Stat>()
                                {
                                    new Stat() {
                                        EquipmentStatName ="Speed",
                                        EquipmentStatBonus = 10000
                                    },
                                    new Stat() {
                                        EquipmentStatName ="Cool",
                                        EquipmentStatBonus = -500
                                    }
                                }
                            },
                            new Equipment() {
                                Name = "Big Mansion",
                                EquipmentWorthValue = 99999,
                                Stats = new List<Stat>()
                                {
                                    new Stat() {
                                        EquipmentStatName ="Size",
                                        EquipmentStatBonus = 10000 
                                    },
                                    new Stat() { 
                                        EquipmentStatName = "Price",
                                        EquipmentStatBonus = 999999
                                    }
                                }
                            },
                            new Equipment() {
                                Name = "Pretty Lady",
                                EquipmentWorthValue = 99999,
                                Stats = new List<Stat>()
                                {
                                    new Stat() { 
                                        EquipmentStatName = "Pretty",
                                        EquipmentStatBonus = 9999
                                    },
                                    new Stat() {
                                        EquipmentStatName = "FaithFul",
                                        EquipmentStatBonus = 9999
                                    }
                                }
                            },
                             new Equipment() {
                                Name = "Debt Free",
                                EquipmentWorthValue = 5555555,
                                Stats = new List<Stat>()
                                {
                                    new Stat() {
                                        EquipmentStatName = "Debt",
                                        EquipmentStatBonus = 0
                                    }
                                }
                            }
                        }
                    }
                },
                Gold = 0
            };
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                        //.WriteTo.File("log.txt",
                        //    rollingInterval: RollingInterval.Minute,
                        //    rollOnFileSizeLimit: true)
                        .CreateLogger();

            List<ExpandoObject> csvPage = new();
            //List<Dictionary<PropertyInfo,object>> csvPage = new();
            //csvPage.AttachFlatableObject(test);

            //int count = 0;
            //IDictionary<string, int> Dict = new Dictionary<string, int>();

            //var test = GetDecomposedPropertyTuple(player);

            csvPage.AppendObjectAsDecomposed2(player);
            //Console.WriteLine("============");
            //Console.WriteLine($"Count: {count}");
            //foreach (var keyValuePair in Dict)
            //{
            //    Console.WriteLine($"{keyValuePair.Key}: {keyValuePair.Value}");
            //}
            Log.Information("============================");

            foreach (var expandoObject in csvPage)
            {
                Log.Information("---\n");
                foreach (var pair in expandoObject)
                {
                    //Console.WriteLine($"{pair.Key.Name}: {pair.Value}");
                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                }
                Log.Information("---");
            }
            Log.CloseAndFlush();
        }
        #region attemp1
        public static void AppendObjectAsDecomposed(this IList<Dictionary<PropertyInfo, object>> page,
                                                    object composedObject,
                                                    Dictionary<PropertyInfo, object>? row = null,
                                                    Dictionary<PropertyInfo, object>? tempRow = null,
                                                    int level = 0)
        {
            if(true)
            {
                foreach (var decomposedObjects in composedObject.GetDecomposed())
                {
                    if (row is null)
                    {
                        if (tempRow is not null)
                        {
                            Console.WriteLine("===========End Of Row===========");
                            Console.WriteLine("===========New Row===========");
                            Console.WriteLine("===========Copying from tempRow===========");
                            row = new Dictionary<PropertyInfo, object>();
                            page.Add(row);
                            foreach (var keyValuePair in tempRow)
                            {
                                row.Add(keyValuePair.Key, keyValuePair.Value);
                            }
                        }
                        else
                        {
                            Console.WriteLine("===========New Row===========");
                            row = new Dictionary<PropertyInfo, object>();
                            page.Add(row);
                        }
                    }   
                    var spaceBuilder = new StringBuilder("");
                    for(int i = 0; i<=level; i++)
                    {
                        spaceBuilder.Append('=');
                    }
                    Console.WriteLine($"{spaceBuilder}===========Level {level} Modificiton===========");
                    Console.WriteLine($"{spaceBuilder}{{");
                    foreach (var (propertyInfo, value) in decomposedObjects as IEnumerable<(PropertyInfo propertyInfo, object value)>)
                    {
                        if (propertyInfo.PropertyType.IsValueType
                            || propertyInfo.PropertyType == typeof(string))//string is a reference type
                        {
                            //not work for struct
                            Console.WriteLine($"{spaceBuilder}{propertyInfo.Name}({propertyInfo.PropertyType.Name}): {value}, ");
                            //if(!row.TryAdd(propertyInfo.Name, value))
                            //{
                            //    row[propertyInfo.Name] = value;
                            //    string ToBeInsertedPropertyNameKey = propertyInfo.Name;
                            //    int count = 1;
                            //    do
                            //    {
                            //        count++;
                            //        ToBeInsertedPropertyNameKey = $"{propertyInfo.Name}{count}";
                            //    }
                            //    while (row.ContainsKey(ToBeInsertedPropertyNameKey));
                            //    row[ToBeInsertedPropertyNameKey] = value;
                            //}
                            if (!row.TryAdd(propertyInfo, value))
                            {
                                row[propertyInfo] = value;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{spaceBuilder}==========={propertyInfo.Name}({propertyInfo.PropertyType.Name}):===========");
                            Console.WriteLine($"{spaceBuilder}[");
                            page.AppendObjectAsDecomposed(value, row, tempRow, level + 1);
                            Console.WriteLine($"{spaceBuilder}]");
                        }
                    }
                    Console.WriteLine($"{spaceBuilder}}}");
                    Console.WriteLine($"{spaceBuilder}===========End of the Level {level} Modificiton===========");
                    Console.WriteLine($"{spaceBuilder}===========Overwrite tempRow===========");
                    tempRow = row;
                    row = null;
                }
            }
        }  
        public static IEnumerable<object> GetDecomposed(this object composedObject)
        {
            //CartesainProduct
            var propertiesInfo = composedObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var IObjectEnumeratorTupleArray = propertiesInfo
                  //.OrderByDescending(propertyInfo => propertyInfo.GetType().IsValueType ? 0 : 1)
                  .Select(propertyInfo => (PropertyInfo: propertyInfo, Enumerator: GetEnumeratorOfAObjectValueOfAProperty(propertyInfo, composedObject)))
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
                    //move next has side effect!!!(it moves then check)
                    //wonder why there is no HasNext()?
                    if (!IObjectEnumeratorTuple.Enumerator.MoveNext())
                    {
                        // stop when the last enumerator resets
                        if (IObjectEnumeratorTuple == IObjectEnumeratorTupleArray.Last())
                        {
                            yield break; //this break the loop for IEnumerable
                        }
                        IObjectEnumeratorTuple.Enumerator.Reset();
                        IObjectEnumeratorTuple.Enumerator.MoveNext();
                        // move to the next enumerator if this reseted
                        // this has to be done because some enuermator has a longer "cycle"
                        continue;
                    }
                    // we could increase the current enumerator without reset so stop here
                    break;
                }
            }
        }

        private static IEnumerator GetEnumeratorOfAObjectValueOfAProperty(PropertyInfo propertyInfo,
                                                                          object original)
        {
            if (propertyInfo.GetValue(original) is null
                && Nullable.GetUnderlyingType(propertyInfo.PropertyType) is null) //for checking nullable which are struct/valuetype
            {
                if (propertyInfo.PropertyType.IsGenericType 
                    && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    //let's pretend that the datatype supply is like IList<T> and not IList<T1,T2,T3>...
                    //so we the T and not T1..T2....
                    var theOneGenericArugmentType = propertyInfo.PropertyType.GetGenericArguments()[0];//expecting T
                    //var listWiththeOneGenericArugment = typeof(List<>).MakeGenericType(theOneGenericArugmentType);//expecting List<T>
                    var placeholderObject = Activator.CreateInstance(theOneGenericArugmentType);
                    var placeholderObjectArray = new object[] { placeholderObject };
                    return placeholderObjectArray.AsEnumerable().GetEnumerator();
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    var placeholderObject = Activator.CreateInstance("".GetType(), Array.Empty<char>());
                    var placeholderObjectArray = new object[] { placeholderObject };
                    return placeholderObjectArray.AsEnumerable().GetEnumerator();
                }
                else if (propertyInfo.PropertyType.IsInterface)
                {
                    return null;
                    //make a provider for this later
                    //switch (propertyInfo.PropertyType.Name)
                    //{
                    //    case "x":
                    //        var CalculatedScore = Activator.CreateInstance(typeof(x));
                    //        var CalculatedScoreObjectArray = new object[] { CalculatedScore };
                    //        return CalculatedScoreObjectArray.AsEnumerable().GetEnumerator();
                    //    case "x":
                    //        var CalculatedDecile = Activator.CreateInstance(typeof(x));
                    //        var CalculatedDecileObjectArray = new object[] { CalculatedDecile };
                    //        return CalculatedDecileObjectArray.AsEnumerable().GetEnumerator();
                    //    case "x":
                    //        var CalculatedRank = Activator.CreateInstance(typeof(x));
                    //        var CalculatedRankArray = new object[] { CalculatedRank };
                    //        return CalculatedRankArray.AsEnumerable().GetEnumerator();
                    //    default:
                    //        throw new System.Exception("Not Mapped Interface");
                    //}
                }
                else
                {
                    var placeholderObject = Activator.CreateInstance(propertyInfo.PropertyType);
                    var placeholderObjectArray = new object[] { placeholderObject };
                    return placeholderObjectArray.AsEnumerable().GetEnumerator();
                }

            }
            else if (propertyInfo.GetValue(original) is not IEnumerable<object>)
            {
                var objectArray = new object[] { propertyInfo.GetValue(original) };
                return objectArray.AsEnumerable().GetEnumerator();
            }
            else
            {
                return (propertyInfo.GetValue(original) as IEnumerable<object>).GetEnumerator();
            }

        }
        #endregion
        #region attempt2
        public static void AppendObjectAsDecomposed2(this IList<ExpandoObject> page,
                                                     object composedObject,
                                                     int level = 0)
        {
            if (true)
            {
                IDictionary<string,object> row = new Dictionary<string,object>();
                foreach (var decomposedObjects in composedObject.GetDecomposed2())
                {
                    //Log.Warning("===========New Row===========");
                    page.Add(row as ExpandoObject);
                    var spaceBuilder = new StringBuilder("");
                    for (int i = 0; i <= level; i++)
                    {
                        spaceBuilder.Append('=');
                    }
                    //Log.Information($"{spaceBuilder}===========Level {level} Modificiton===========");
                    //Log.Information($"{spaceBuilder}{{");
                    foreach (var (propertyInfo, value) in decomposedObjects)
                    {
                        //Log.Information($"{spaceBuilder}{propertyInfo.Name}({propertyInfo.PropertyType.Name}): {value}, ");
                        if (!row.TryAdd(propertyInfo.Name, value))
                        {
                            //row[propertyInfo.Name] = value;
                            string ToBeInsertedPropertyNameKey;
                            int count = 1;
                            do
                            {
                                count++;
                                ToBeInsertedPropertyNameKey = $"{propertyInfo.Name}{count}";
                            }
                            while (row.ContainsKey(ToBeInsertedPropertyNameKey));
                            row[ToBeInsertedPropertyNameKey] = value;
                        }
                    }
                    //Log.Information($"{spaceBuilder}}}");
                    //Log.Information($"{spaceBuilder}===========End of the Level {level} Modificiton===========");
                    //Log.Warning("===========End Of Row===========");
                }
            }
        }
        public static IEnumerable<IEnumerable<(PropertyInfo,object)>> GetDecomposed2(this object composedObject)
        {
            //CartesainProduct
            var IObjectEnumerableDictonaryArray = composedObject.GetDecomposedPropertyTuple()
                .Select(IObjectEnumeratorDictonary => (Property: IObjectEnumeratorDictonary.Key, Enumerator: IObjectEnumeratorDictonary.Value.GetEnumerator()))
                .Where(IObjectEnumeratorDictonary => 
                       (IObjectEnumeratorDictonary.Property.PropertyType.IsValueType ||
                       IObjectEnumeratorDictonary.Property.PropertyType == typeof(string) ||
                       Nullable.GetUnderlyingType(IObjectEnumeratorDictonary.Property.PropertyType) != null) &&
                       IObjectEnumeratorDictonary.Enumerator.MoveNext())
                .ToArray();
            
            while(true)
            {
                yield return IObjectEnumerableDictonaryArray
                   .Select(IObjectEnumeratorDictonary => (IObjectEnumeratorDictonary.Property, IObjectEnumeratorDictonary.Enumerator.Current));

                // increase enumerators
                foreach ((PropertyInfo Property, IEnumerator<object> Enumerator) in IObjectEnumerableDictonaryArray)
                {
                    if(Property.Name.CompareTo("PlayerName")==0)
                    {
                        Log.Warning(Property.Name);
                    }
                    Log.Warning(Property.Name);
                    // reset the slot if it couldn't move next
                    //move next has side effect!!!(it moves then check)
                    //wonder why there is no HasNext()?
                    if (!Enumerator.MoveNext())
                    {
                        // stop when the last enumerator resets
                        if (Property == IObjectEnumerableDictonaryArray.Last().Property)
                        {
                           yield break; //this break the loop for IEnumerable
                        }
                        Enumerator.Reset();
                        Enumerator.MoveNext();
                        // move to the next enumerator if this reseted
                        // this has to be done because some enuermator has a longer "cycle"
                        continue;
                    }
                    // we could increase the current enumerator without reset so stop here
                    break;
                }
            }
        }
        public static Dictionary<PropertyInfo, IEnumerable<object>> GetDecomposedPropertyTuple(this object composedObject)
        {
            //List<PropertyInfo> decomposedProperties = new();
            Dictionary<PropertyInfo, IEnumerable<object>> visitedTuple = new();//no duplicate
            //HashSet<PropertyInfo> visitedProperties = new();//no duplicate
            Stack<(PropertyInfo, object)> tupleStack = new();
            var propertiesInfo = composedObject
                .GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for(int i = 0; i < propertiesInfo.Length; i++)
            {
                tupleStack.Push((propertiesInfo[i], composedObject));
            }
            while(tupleStack.Count > 0)
            {
                var (propertyInfo, value) = tupleStack.Pop();

                if (propertyInfo.PropertyType.IsValueType ||
                    propertyInfo.PropertyType == typeof(string) ||
                    Nullable.GetUnderlyingType(propertyInfo.PropertyType) is not null)
                {
                    var peekTupleEnumerable = new List<object> { propertyInfo.GetValue(value) };
                    if (visitedTuple.TryGetValue(propertyInfo, out IEnumerable<object>? enumerable))
                    {
                        var combinedEnumerable = new List<object>()
;                       combinedEnumerable.AddRange(enumerable);
                        combinedEnumerable.AddRange(peekTupleEnumerable);
                        visitedTuple[propertyInfo] = combinedEnumerable;
                    }
                    else
                    {
                        visitedTuple.Add(propertyInfo, peekTupleEnumerable);
                    }
                }
                else
                {
                    var peekTupleEnumerable = GetEnumerableOfAObjectValueOfAProperty2(propertyInfo, propertyInfo.GetValue(value));
                    var enumerator = peekTupleEnumerable.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        var deeperProperties = current.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        for (int i = 0; i < deeperProperties.Length; i++)
                        {
                            var deeperTuple = (deeperProperties[i], current);
                            tupleStack.Push(deeperTuple);
                        }
                    }
                    enumerator.Reset();
                }
            }
            return visitedTuple;
        }
        private static IEnumerable<object>? GetEnumerableOfAObjectValueOfAProperty2(PropertyInfo propertyInfo,
                                                                                   object original)
        {
            if (original is null
                && Nullable.GetUnderlyingType(propertyInfo.PropertyType) is null) //for checking nullable which are struct/valuetype
            {
                if (propertyInfo.PropertyType.IsGenericType
                    && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    //let's pretend that the datatype supply is like IList<T> and not IList<T1,T2,T3>...
                    //so we the T and not T1..T2....
                    var theOneGenericArugmentType = propertyInfo.PropertyType.GetGenericArguments()[0];//expecting T
                    //var listWiththeOneGenericArugment = typeof(List<>).MakeGenericType(theOneGenericArugmentType);//expecting List<T>
                    var placeholderObject = Activator.CreateInstance(theOneGenericArugmentType);
                    return new object[] { placeholderObject }.AsEnumerable();
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    var placeholderObject = Activator.CreateInstance("".GetType(), Array.Empty<char>());
                    return new object[] { placeholderObject }.AsEnumerable();
                }
                else if (propertyInfo.PropertyType.IsInterface)
                {
                    return null;
                    //make a provider for this later
                    //switch (propertyInfo.PropertyType.Name)
                    //{
                    //    case "x":
                    //        var CalculatedScore = Activator.CreateInstance(typeof(x));
                    //        var CalculatedScoreObjectArray = new object[] { CalculatedScore };
                    //        return CalculatedScoreObjectArray.AsEnumerable().GetEnumerator();
                    //    case "x":
                    //        var CalculatedDecile = Activator.CreateInstance(typeof(x));
                    //        var CalculatedDecileObjectArray = new object[] { CalculatedDecile };
                    //        return CalculatedDecileObjectArray.AsEnumerable().GetEnumerator();
                    //    case "x":
                    //        var CalculatedRank = Activator.CreateInstance(typeof(x));
                    //        var CalculatedRankArray = new object[] { CalculatedRank };
                    //        return CalculatedRankArray.AsEnumerable().GetEnumerator();
                    //    default:
                    //        throw new System.Exception("Not Mapped Interface");
                    //}
                }
                else
                {
                    var placeholderObject = Activator.CreateInstance(propertyInfo.PropertyType);
                    return new object[] { placeholderObject }.AsEnumerable();
                }
            }
            else if (original is not IEnumerable<object>)
            {
                return new object[] { original }.AsEnumerable();
            }
            else
            {
                return original as IEnumerable<object>;
            }

        }
        #endregion
        #region different
        static void Show(object test, ref int count, IDictionary<string, int> Dict)
        {
            foreach (var results in Decompose(test))
            {
                foreach (var (propertyInfo, value) in results as IEnumerable<(PropertyInfo, object)>)
                {
                    if (propertyInfo.PropertyType.IsValueType)
                    {
                        Console.WriteLine($"{propertyInfo.Name}({propertyInfo.GetType()}): {value}, ");
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
        private static void AddPropertiesColumn(this IList<ExpandoObject> csvPage, PropertyInfo composedObjectPropertyInfo,
           IDictionary<string, object>? csvRow = null,
           IDictionary<string, object>? tempRow = null)
        {
            //if (csvRow is null)
            //{
            //    if (tempRow is not null)
            //    {
            //        csvRow = new ExpandoObject();
            //        csvPage.Add(csvRow as ExpandoObject);
            //        foreach (var keyValuePair in tempRow)
            //        {
            //            csvRow.Add(keyValuePair.Key, keyValuePair.Value);
            //        }
            //    }
            //    else
            //    {
            //        csvRow = new ExpandoObject();
            //        csvPage.Add(csvRow as ExpandoObject);
            //    }
            //}
            var properties = composedObjectPropertyInfo.PropertyType.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].PropertyType.IsValueType || (properties[i].PropertyType == typeof(string)))
                {
                    //not work for struct
                    if (!csvRow.TryAdd(properties[i].Name, null))
                    {
                        csvRow[properties[i].Name] = null;
                    }
                }
                else
                {
                    csvPage.AddPropertiesColumn(properties[i], csvRow, tempRow);
                }
            }
            //tempRow = csvRow;
            //csvRow = null;
        }
        static void AttachFlatableObject2(this IList<ExpandoObject> csvPage,
           object test, ref int count,
           IDictionary<string, int> Dict,
           IDictionary<string, object>? csvRow = null)
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
        static void AttachFlatableObject(this IList<ExpandoObject> csvPage, object layeredObject, IDictionary<string, object>? csvRow = null)
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
        public static IEnumerable<IEnumerable<T>>? CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
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
                yield return element is IEnumerable candidate ? Flatten(candidate) : element;
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
        #endregion
    }
}
