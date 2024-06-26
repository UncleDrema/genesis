using UnityEngine;

namespace Genesis.GameStartup
{
    internal static class ApplicationConfiguration
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Configure()
        {
            Application.targetFrameRate = 300;
        }
    }
}