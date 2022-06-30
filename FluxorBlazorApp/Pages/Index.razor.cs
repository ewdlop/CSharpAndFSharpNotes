public static class Math
{
    public static Func<C, D> NaturalTransformation<A,B,C,D>(Func<C,D> F, Func<Func<C, D>, Func<C, D>> eta)
    {
        return eta(F);
    }
}