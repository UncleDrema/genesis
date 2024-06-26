using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace Genesis.GameWorld.Requests.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class InitializeGameWorldRequestProvider : MonoProvider<InitializeGameWorldRequest> { }
}