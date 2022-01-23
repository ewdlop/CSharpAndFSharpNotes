public partial class Person
{
    public class CarFactory
    {
        private Car reserved;
        private int createdCount;
        private bool LostPower;
        public Car CreateGoodCar()
        {
            createdCount++;
            if(LostPower)
            {
                return reserved;
            }
            return new Car(new GoodCarEngine());
        }
    }
}

