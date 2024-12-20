

namespace CSharp13.X
{
    namespace Y
    {
        public static class ConditionalCompliationSampleClass
        {
            public static C1 Create0() => C1.Create0;
            public static C1 Create0O() => C1.Create0;

            public partial class C1
            {
                public const double P1 = Math.PI;
                public static double P10 => Math.PI;
                public static double P11() => Math.PI;

                public static C1 Create0 => new C1(2, 3, 4);
                public static C1 Create00() => new C1(2, 3, 4);
                public static C1 Create00O() => new C1(2, 3, 4);
                public static C1 Create000() => new C1(2, 3, 4);
            }
#if NET8_0_OR_GREATER
            public partial class C1(int P2, int P3, int P4)
            {
#if NET9_0_OR_GREATER
#if DEBUG
                //Field-backed property
                public int P2
                {
                    get
                    {
                        if (field == 0)
                        {
                            //field = 1;
                            return field;
                        }
                        return 1;
                    }
                } = P2;

                public int P3 { get => field; set; } = P3;
                public int P4 { get => field; set => field = value; } = P4;
#elif NET9_0_OR_GREATER

                private int _P2 = P2;
                private int _P3 = P3;
                private int _P4 = P4;
                public int P2
                {
                    get
                    {
                        if (_P2 == 0)
                        {
                            //_P2 = 1;
                            return _P2;
                        }
                        return _P2;
                    }
                }
                public int P3 { get; set; } = P3;
                public int P4 { get; set; } = P4;
            
#endif
#elif NET8_0_OR_GREATER
                private int _P2 = P2;
                private int _P3 = P3;
                private int _P4 = P4;
                public int P2
                {
                    get
                    {
                        if (_P2 == 0)
                        {
                            //_P2 = 1;
                            return _P2;
                        }
                        return _P2;
                    }
                }
                public int P3 { get => _P3; set => _P3 = value; }
                public int P4 { get => _P4; set => _P4 = value; }
#endif
            }
#elif NET7_0_OR_GREATER
            public partial class C1
            {
                private int _P2;
                private int _P3;
                private int _P4;

                public C1(int P2, int P3, int P4)
                {
                    _P2 = P2;
                    _P3 = P3;
                    _P4 = P4;
                }

                public int P2
                {
                    get
                    {
                        if (_P2 == 0)
                        {
                            //_P2 = 1;
                            return _P2;
                        }
                        return _P2;
                    }
                    set => _P2 = value;
                }
                public int P3 { get => _P3; set => _P3 = value; }
                public int P4 { get => _P4; set => _P4 = value; }
            }
#endif
        }
    }
}
