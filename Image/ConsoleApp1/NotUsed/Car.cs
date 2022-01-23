public partial class Person
{
    public class Car : ICar
    {
        private readonly IEngine _engine;
        private object _someInTheTrunk;
        public Car(GoodCarEngine engine)
        {
            _engine = engine;
        }
        public Car(BadCarEngine engine)
        {
            _engine = engine;
        }
        public void LookingIntoTHeTrunK()
        {

        }
        public void Run()
        {
            _engine.Run();
        }

        public int GetTopSpeed()
        {
            return _engine.TopSpeed;
        }

        public IEngine GetEngine()
        {
            return _engine;
        }
    }
}

