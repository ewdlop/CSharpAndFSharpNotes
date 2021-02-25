using System;
using System.Collections.Generic;

namespace CSharpClassLibrary.CSharp9.PatternMatching
{
    public enum LifeStage
    {
        Prenatal,
        Infant,
        Toddler,
        EarlyChild,
        MiddleChild,
        Adolescent,
        EarlyAdult,
        MiddleAdult,
        LateAdult
    }
    static class Extension
    {
        //Disjunctive Pattern, Conjunctive Pattern
        public static bool IsLetterOrSeparator(this char c) => c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or '.' or ',';
    }
    static class Helper
    {
        public static void M(object o1, object o2)
        {
            var t = (o1, o2);
            if (t is (int, string)) { }
            switch (o1)
            {
                case int: break; // test if o1 is an int
                case string: break; // test if o1 is a string
            }
            switch (o1,o2)
            {
                case (0, int x):
                    break;
                case (int x, 0):
                    Console.WriteLine(x);
                    break;
            }
        }

        //Relational Patterns
        public static LifeStage LifeStageAtAge(int age) => age switch
        {
            < 0 => LifeStage.Prenatal,
            < 2 => LifeStage.Infant,
            < 4 => LifeStage.Toddler,
            < 6 => LifeStage.EarlyChild,
            < 12 => LifeStage.MiddleChild,
            < 20 => LifeStage.Adolescent,
            < 40 => LifeStage.EarlyAdult,
            < 65 => LifeStage.MiddleAdult,
            _ => LifeStage.LateAdult,
        };

        public static bool IsValidPercentage(object x) => x is
            >= 0 and <= 100 or    // integer tests
            >= 0F and <= 100F or  // float tests
            >= 0D and <= 100D;    // double tests

        public static bool IsSmallByte(object o) => o is byte and < 100;

        //Diagnostics, subsumption, and exhaustiveness
        public static void DiceChecker(int roll)
        {
            switch(roll)
            {
                case 1 or 2:
                    break;
                case 3 or 4 or 5 or 6:
                    break;
                case > 1:
                    break;
            }
        }
    }
}
