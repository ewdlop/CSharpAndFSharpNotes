using System;

namespace CSharpClassLibrary.CSharp8
{
    class InterfaceChanges
    {
    }
    interface IX
    {
        void Method1();
        public void Method2();
        public void Method3()
        {
            Method4();
            Test1();
        }
        private void Method4()
        {
            Test1();
        }
        protected void Method5(); //not a good idea?
        protected void Method6()
        {
            Method5();
            //accessible by IY not X
        }
        protected void Method7()
        {
            //accessible by IY not X
        }
        private static void Test1() {
            M = 2;
            //N = 2; not statics
        }
        protected static void Test2() { }
        static int M { get; set; }
        int N { get; set; }
        public int O { get; set; }
        abstract void Methosd8();
        protected abstract void Method9();
        virtual void Method10()
        {

        }
    }
    interface IY :IX
    {
        new void Method1()
        {
            (this as IX).Method1();
            IX.M = 2; //N is only accessiable by class that implments IX, or interface that extends IX
            //IX.Test1(); //private
            IX.Test2(); //Test is only accessiable by class that implments IX, or interface that extends IX
        }

        void IX.Method1()
        {
            Method5();
            Method6();
            Method7();
        }
        void IX.Method5()
        {
        }
        new void Method5()
        {
        }
        void IX.Method7()
        {

        }
        void IX.Method9()
        {

        }
        void IX.Method10()
        {

        }
        new void Method10()
        {

        }
    }
    public abstract class X : IX
    {
        public int N { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IX.N { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int O { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IX.O { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        void IX.Method1()
        {
            //use new to override
            (this as IX).Method3();
            this.Method1();
            //(this as IX).Method6();
        }

        void IX.Method2()
        {
            IX.M = 3;
            this.Method1();
        }
        public void Method1()
        {
        }
        public void Method2()
        {
            IX.Test2();
        }
        void IX.Method5()
        {
            Method8();
            (this as IX).Methosd8();
            Methosd8();
        }
        private void Method8()
        {
        }

        void IX.Method9()
        {
            throw new NotImplementedException();
        }

        public virtual void Method10()
        {
        }
        public void Methosd8()
        {
            throw new NotImplementedException();
        }
        void IX.Methosd8()
        {
            Method1();
        }
        public abstract void Method11();
    }
    public class Y : X, IY
    {
        new public int N { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IX.N { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IX.O { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public new static void Method2()
        {
        }

        void IY.Method1()
        {

        }
        void IX.Method1()
        {

        }
        //void IX.Method2()
        //{

        //}
        void IX.Method5()
        {
            //IX.Method5() called under IX.Method1() under IY will be ovverriden by this?
        }

        void IX.Method9()
        {
            throw new NotImplementedException();
        }

        public override void Method10()
        {
            base.Method10();
            Method2();
        }
        public override void Method11()
        {
            (this as IY).Method1();
            (this as IY).Method5();
        }
    }
}