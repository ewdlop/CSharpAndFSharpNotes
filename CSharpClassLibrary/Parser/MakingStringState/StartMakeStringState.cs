namespace CSharpClassLibrary
{
    public class StartMakeStringState : IMakeStringState
    {
        public Token DoAction(MakeStringContent content, string s) {
            if (content.C == '"') {
                content.State = new DoubleQuotesMakeStringState();
            } else {
                content.State = new SingleQuotesMakeStringState();
            }
            return null;
        }
    }
}
