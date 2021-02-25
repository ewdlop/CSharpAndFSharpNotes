using System;

namespace CSharpClassLibrary.CSharp9.InterfaceChange
{
    //Virtual method in interfaces
    interface IBaseInterface // Base or Parent interface
    {
        public virtual void BaseInterfaceVirtualMethod() // Virtual method declared Explicitly by "virtual" keyword
        {
            Console.WriteLine("This is Base Interface - BaseInterfaceVirtualMethod()");
        }
        //. A protected internal member is accessible from the current assembly or from types that are derived from the containing class
        protected internal void BaseInterfaceDefaultVirtualMethod() // Default Virtual method without "virtual" keyword
        {
            Console.WriteLine("This is Base Interface : BaseInterfaceDefaultVirtualMethod()");
        }
        public void BaseInterfaceDefaultMethod()
        {
            Console.WriteLine("This is Base Interface : BaseInterfaceDefaultMethod()");
        }
    }
    interface IDerivedInterface : IBaseInterface // Derived or Child interface
    {
        void IBaseInterface.BaseInterfaceVirtualMethod() // Explicit virtual interface method overriding
        {
            Console.WriteLine("This is Derived Interface : BaseInterfaceVirtualMethod() overridden");
        }
        abstract void IBaseInterface.BaseInterfaceDefaultVirtualMethod(); // Re-Abstraction : Making virtual method as abstract in derived interface
        public new void BaseInterfaceDefaultMethod() // Base interface virtual method hiding
        {
            Console.WriteLine("This is Derived Interface : BaseInterfaceDefaultMethod() hidden");
        }
        virtual void DerivedInterfaecVirtualMethod()
        {
            Console.WriteLine("This is Derived Interface : DerivedInterfaecVirtualMethod()");
        }
    }

    class BaseClass : IBaseInterface // Inheriting class
    {
        void IBaseInterface.BaseInterfaceVirtualMethod() // Explicit virtual method overriding in class
        {
            Console.WriteLine("This is Base Class : BaseInterfaceVirtualMethod() overridden");
        }
    }
    class DerivedClass : IDerivedInterface // Inheriting class
    {
        void IBaseInterface.BaseInterfaceDefaultVirtualMethod() // Implementing abstract overriden member in the inheriting class
        {
            Console.WriteLine("This is Derived Class : abstract BaseInterfaceDefaultVirtualMethod() overridden");
        }

        public void DerivedInterfaecVirtualMethod() // This method will neither hiding nor overridding the "DerivedInterfaecVirtualMethod" virtual method of inheritted interface "IDerivedInterface"
        {
            Console.WriteLine("This is Derived Class : DerivedInterfaecVirtualMethod()");
        }
    }
}
