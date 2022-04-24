using System;

namespace CSharpClassLibrary.Misc
{

    class Person
    {
        // Settable property.
        public Memory<char> FirstName { get; set; }

        // alternatively, equivalent "setter" method
        public void SetFirstName(Memory<char> value)
        {
            FirstName = value;
        }

        // alternatively, a public settable field
        //public Memory<char> FirstName
    }
}
