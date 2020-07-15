namespace CSharpClassLibrary
{
    public class MakeStringContent
    {
        public IMakeStringState State { get; set; }
        public char C { get; set; }

        public MakeStringContent(IMakeStringState startState) {
            State = startState;
        }

        public string Concat(string s)
        {
            return s + C;
        }
    }
}
