using Fluxor;

namespace FluxorBlazorApp.Store
{
    [FeatureState]
    public class CounterState
    {
        public int key;
        public int ClickCount { get; }

        private CounterState() { } // Required for creating initial state

        public CounterState(int clickCount)
        {
            ClickCount = clickCount;
        }
    }
}