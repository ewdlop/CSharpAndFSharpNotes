

using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace TestProject3
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            // Arrange
            var someDto = new ClientDto
            {
                Id = 291293,
                Name = "John Doe",
                Age = 42
            };
            var mailComposerMock = new Mock<MailComposer>();
            mailComposerMock
              .Setup(c => c.Compose(It.IsAny<ClientDto>()))
              .Returns(new EmptyMail());
            mailComposerMock
              .Setup(c => c.Compose2(It.IsAny<string>(), It.IsAny<string>()))
              .ReturnsAsync(new EmptyMail());

            mailComposerMock
              .Setup(c => c.Compose2(It.Is<string>(s=>s.Equals("")), It.IsAny<string>()))
              .ReturnsAsync(new EmptyMail());
            var repositoryMock = new Mock<IRepository>();
            var loggerMock = new Mock<ILogger>();
            var sut = new MyService(
              mailComposerMock.Object,
              repositoryMock.Object,
              loggerMock.Object
            );

        }

        [Test]
        public void Test2()
        {
            var x = new Mock<Test2>();
            x.Setup(x => x.ToString()).Returns("test").Verifiable();
            var result =  x.Object.ToString();
            x.Verify();
        }
    }

    public class Test2
    {
        public override string ToString()
        {
            return base.ToString();
        }
    }

    [TestFixtureSource(typeof(MyFixtureData), nameof(MyFixtureData.FixtureParams))]
    public class ParameterizedTestFixture
    {
        private readonly string _eq1;
        private readonly string _eq2;
        private readonly string? _neq;

        public ParameterizedTestFixture(string eq1, string eq2, string? neq)
        {
            _eq1 = eq1;
            _eq2 = eq2;
            _neq = neq;
        }

        public ParameterizedTestFixture(string eq1, string eq2)
            : this(eq1, eq2, null) { }

        public ParameterizedTestFixture(int eq1, int eq2, int neq)
        {
            _eq1 = eq1.ToString();
            _eq2 = eq2.ToString();
            _neq = neq.ToString();
        }

        [Test]
        public void TestEquality()
        {
            Assert.That(_eq2, Is.EqualTo(_eq1));
            Assert.That(_eq2.GetHashCode(), Is.EqualTo(_eq1.GetHashCode()));
        }

        [Test]
        public void TestInequality()
        {
            Assert.That(_neq, Is.Not.EqualTo(_eq1));
            if (_neq != null)
            {
                Assert.That(_neq.GetHashCode(), Is.Not.EqualTo(_eq1.GetHashCode()));
            }
        }
    }

    public class MyFixtureData
    {
        public static IEnumerable<TestFixtureData> FixtureParams
        {
            get
            {
                yield return new TestFixtureData("hello", "hello", "goodbye");
                yield return new TestFixtureData("zip", "zip");
                yield return new TestFixtureData(42, 42, 99);
                yield return new TestFixtureData(new object?[] { 1, 2, 3 });
            }
        }
    }

    public class TestBase
    {
        protected List<int> _list;

        [SetUp]
        public void Setup()
        {
            _list = new List<int>() { 1, 2, 3 };
        }
    }

    [TestFixture]
    public class TestClass : TestBase
    {
        [TestCase(1)]
        public void Test(int i)
        {
            Assert.That(_list, Is.Not.Null);
            Assert.That(_list, Is.Not.Empty);
            Assert.That(_list, Has.Count.EqualTo(3));
        }
    }
}