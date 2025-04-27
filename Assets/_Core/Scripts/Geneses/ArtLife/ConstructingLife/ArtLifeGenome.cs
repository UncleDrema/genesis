namespace Geneses.ArtLife.ConstructingLife
{
    public enum ArtLifeGenome : byte
    {
        Photosynthesis = 0,
        AbsoluteRotate = 1,
        RelativeRotate = 2,
        AbsoluteMove = 3,
        RelativeMove = 4,
        AbsoluteLook = 5,
        RelativeLook = 6,
        AlignHorizontal = 7,
        AlignVertical = 8,
        AbsoluteShare = 9,
        RelativeShare = 10,
        AbsoluteGift = 11,
        RelativeGift = 12,
        AbsoluteEat = 13,
        RelativeEat = 14,
        ConvertMinerals = 15,
        Duplicate = 16,
        CheckEnergy = 17,
        CheckHeight = 18,
        CheckMinerals = 19,
        CheckSurrounded = 20,
        CheckPhotosynthesisFlow = 21,
        CheckMineralFlow = 22,
        Exit = 255
    }
}