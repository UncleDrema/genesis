namespace Geneses.ArtLife.ConstructingLife
{
    public static class LifePresets
    {
        public static byte[] SimpleLife()
        {
            var builder = new LifeBuilder();
            
            // Клетка должна делиться, если не окружена, а если окружена, то поддерживать себя живой
            
            /*
l_start:
	CheckEnergy 4 l_main l_critical
l_critical:
	Photosynthesis
	Exit
l_main:
    CheckSurrounded l_wait l_try_dup
l_wait:
    Exit
l_try_dup:
	CheckEnergy 15 l_dup l_sun
l_sun:
	Photosynthesis
	Exit
l_dup:
	Duplicate
	Exit
             */

            return builder
                .CheckEnergy(4, "l_main", "l_critical")
                .DeclareLabel("l_critical")
                .Photosynthesis()
                .Exit()
                .DeclareLabel("l_main")
                .CheckSurrounded("l_wait", "l_try_dup")
                .DeclareLabel("l_wait")
                .Exit()
                .DeclareLabel("l_try_dup")
                .CheckEnergy(64, "l_dup", "l_sun")
                .DeclareLabel("l_sun")
                .Photosynthesis()
                .Exit()
                .DeclareLabel("l_dup")
                .Duplicate()
                .Exit()
                .Build();
        }
    }
}