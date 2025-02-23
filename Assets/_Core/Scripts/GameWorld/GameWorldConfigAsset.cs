using UnityEngine;

namespace Genesis.GameWorld
{
    [CreateAssetMenu(fileName = "GameWorldConfig", menuName = "Genesis/GameWorldConfig")]
    public class GameWorldConfigAsset : ScriptableObject, IGameWorldConfig
    {
        [field: SerializeField]
        public int WorldWidth { get; private set; } = 400;
        [field: SerializeField]
        public int WorldHeight { get; private set; } = 300;
        [field: SerializeField]
        public int PixelSize { get; private set; } = 2;
        [field: SerializeField, Min(1)]
        public float DesiredFramerate { get; private set; } = 60f;
        public float TickPeriod => 1f / DesiredFramerate;
    }
}