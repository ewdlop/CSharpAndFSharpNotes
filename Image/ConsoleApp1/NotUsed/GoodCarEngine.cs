﻿public partial class Person
{
    public class GoodCarEngine : IEngine
    {
        private object piston;
        public int TopSpeed => 100;

        public int CurrentSpeed { get; set; }

        public void Run()
        {
            CurrentSpeed = 50;
        }
    }
}

