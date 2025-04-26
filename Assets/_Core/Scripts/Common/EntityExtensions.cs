using Scellecs.Morpeh;

namespace Genesis.Common
{
    public static class EntityExtensions
    {
        public static ref T GetOrAddComponent<T>(this Entity entity) where T : struct, IComponent
        {
            if (entity.Has<T>())
            {
                return ref entity.GetComponent<T>();
            }

            return ref entity.AddComponent<T>();
        }
    }
}