namespace TestProject3
{
    internal class MailComposer
    {
        public EmptyMail Compose(ClientDto clientDto)
        {
            return new EmptyMail();
        }

        public EmptyMail Compose(string a, string b)
        {
            return new EmptyMail();
        }

        public Task<EmptyMail> Compose2(string a, string b)
        {
            return Task.FromResult<EmptyMail>(new EmptyMail());
        }
    }
}