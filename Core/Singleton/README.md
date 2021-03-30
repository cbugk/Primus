### Primus.Core.Singleton
Generic Singleton implementation taken from [this blog](http://www.unitygeek.com/unity_c_singleton/).

Usage:

        public class SampleSingleton : GenericSingleton<SampleSingleton>
        {
        }

Note that use of singleton is better suited for composition than inheritance, although the latter is as much possible.