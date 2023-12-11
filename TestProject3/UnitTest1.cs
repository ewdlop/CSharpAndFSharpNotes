

using Moq;
using NUnit.Framework;

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
}