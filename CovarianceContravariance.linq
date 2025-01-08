/// <summary>
/// Demonstrates Covariance and Contravariance in C#
/// </summary>
public class CovarianceContravariance
{
	// Base class and derived classes for demonstration
	public class Animal { }
	public class Dog : Animal { }
	public class Cat : Animal { }

	// Covariance Demonstration (out keyword)
	public interface ICovariant<out T>
	{
		T GetItem();
	}

	// Contravariance Demonstration (in keyword)
	public interface IContravariant<in T>
	{
		void ProcessItem(T item);
	}

	// Implementations to show usage
	public class CovariantContainer<T> : ICovariant<T>
	{
		private T _item;

		public CovariantContainer(T item)
		{
			_item = item;
		}

		public T GetItem() => _item;
	}

	public class ContravariantProcessor<T> : IContravariant<T>
	{
		private Action<T> _processor;

		public ContravariantProcessor(Action<T> processor)
		{
			_processor = processor;
		}

		public void ProcessItem(T item) => _processor(item);
	}

	// Demonstration method
	public static void DemonstrateCovariance()
	{
		Console.WriteLine("Covariance Demonstration:");

		// Covariance allows using a more derived type
		// ICovariant<Dog> can be treated as ICovariant<Animal>
		ICovariant<Dog> dogContainer = new CovariantContainer<Dog>(new Dog());
		ICovariant<Animal> animalContainer = dogContainer;

		// Can get an Animal from a Dog container
		Animal retrievedAnimal = animalContainer.GetItem();
		Console.WriteLine($"Retrieved Animal from Dog container: {retrievedAnimal.GetType().Name}");
	}

	public static void DemonstrateContravariance()
	{
		Console.WriteLine("\nContravariance Demonstration:");

		// Contravariance allows using a less derived type
		// IContravariant<Animal> can be treated as IContravariant<Dog>
		Action<Animal> animalProcessor = (animal) =>
			Console.WriteLine($"Processing an animal: {animal.GetType().Name}");

		IContravariant<Animal> animalContravariant =
			new ContravariantProcessor<Animal>(animalProcessor);

		// Can pass a Dog to a method expecting an Animal
		IContravariant<Dog> dogContravariant = animalContravariant;
		dogContravariant.ProcessItem(new Dog());
	}

	// Delegate Covariance and Contravariance
	public static void DemonstrateDelegateVariance()
	{
		Console.WriteLine("\nDelegate Variance Demonstration:");

		// Covariant delegate return type
		Func<Dog> getDog = () => new Dog();
		Func<Animal> getAnimal = getDog;
		Console.WriteLine($"Covariant delegate returns: {getAnimal().GetType().Name}");

		// Contravariant delegate parameter
		Action<Animal> processAnimal = (a) =>
			Console.WriteLine($"Processing animal: {a.GetType().Name}");
		Action<Dog> processDog = processAnimal;
		processDog(new Dog());
	}

	public static void Main()
	{
		DemonstrateCovariance();
		DemonstrateContravariance();
		DemonstrateDelegateVariance();
	}
}
