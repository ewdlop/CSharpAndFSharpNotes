namespace CSharpClassLibrary.Native
{
    unsafe struct Action
    {
        delegate*<void> _ptr;

        Action(delegate*<void> ptr) => _ptr = ptr;
        public void Invoke() => _ptr();
    }

}
