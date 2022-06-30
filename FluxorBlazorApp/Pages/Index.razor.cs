public static class Math
{
    //Natural Transfomration
    public static Func<Func<C, D>, Func<C, D>> η_x<C,D>(Func<C,D> F, Func<C,D> G)
    {
        return F => G;
    }
}