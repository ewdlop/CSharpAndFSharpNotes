namespace CSharpClassLibrary.RandomComplier
{
    public class MakeStringContent
    {
        public IMakeStringState State { get; set; }
        public char C { get; set; }

        public MakeStringContent(IMakeStringState startState) {
            State = startState;
        }

        public string Concat(string s) => $"{s}{C}";
    }
}
