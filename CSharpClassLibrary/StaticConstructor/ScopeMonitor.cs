namespace CSharpClassLibrary.StaticConstructor;

class ScopeMonitor
{
    static string firstPart = "http://www.example.com/";
    static string fullUrl;
    static string urlFragment = "foo/bar";

    static ScopeMonitor()
    {
        fullUrl = firstPart + urlFragment;

    }
}