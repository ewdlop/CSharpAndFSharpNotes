using NUnit.Framework.Internal;

namespace TestProject3
{
    internal class MyService
    {
        private MailComposer object1;
        private IRepository object2;
        private ILogger object3;

        public MyService(MailComposer object1, IRepository object2, ILogger object3)
        {
            this.object1 = object1;
            this.object2 = object2;
            this.object3 = object3;
        }
    }
}