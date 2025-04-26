using UnityEngine;

namespace Genesis.GameWorld
{
    [CreateAssetMenu(fileName = "GameWorldConfig", menuName = "Genesis/GameWorldConfig")]
    public class GameWorldConfigAsset : ScriptableObject, IGameWorldConfig
    {
        [field: SerializeField]
        public int WorldWidth { get; set; } = 400;
        [field: SerializeField]
        public int WorldHeight { get; set; } = 300;
        [field: SerializeField]
        public int PixelSize { get; set; } = 2;
        [field: SerializeField, Min(1)]
        public float DesiredFramerate { get; set; } = 60f;
        [field: SerializeField, Min(1)]
        public int DrawEveryNthFrame { get; set; } = 1;
        public float TickPeriod => 1f / DesiredFramerate;
    }
}