using UnityEngine;

namespace Geneses.ArtLife.ConstructingLife
{
    public static class LifePresets
    {
        public static byte[] SimpleLife()
        {
            var builder = new LifeBuilder();
            
            // Клетка должна делиться, если не окружена, а если окружена, то поддерживать себя живой

            var genomeBuilder = builder
                .CheckEnergy(16, "l_main", "l_critical")
                .DeclareLabel("l_critical")
                .Photosynthesis()
                .Exit()
                .DeclareLabel("l_main")
                .CheckSurrounded(140, "l_wait", "l_try_dup")
                .DeclareLabel("l_wait")
                .Exit()
                .DeclareLabel("l_try_dup")
                .CheckEnergy(64, "l_dup", "l_sun")
                .DeclareLabel("l_sun")
                .Photosynthesis()
                .Exit()
                .DeclareLabel("l_dup")
                .Duplicate()
                .Exit();

            var tokens = genomeBuilder.GetRawTokens();
            var genome = genomeBuilder.Build();
            return genome;
        }

        public static byte[] PredatorLife()
        {
            var builder = new LifeBuilder();
            // Напишем код клетки-хищника, которая будет есть другие клетки

            return builder
                .DeclareLabel("l_start")
                .CheckEnergy(4, "l_main", "l_critical")
                .DeclareLabel("l_critical")
                .CheckMineralsFlow(1, "l_convertMinerals", "l_photosynthesis")
                .DeclareLabel("l_convertMinerals")
                .ConvertMinerals()
                .Exit()
                .DeclareLabel("l_photosynthesis")
                .Photosynthesis()
                .Exit()
                .DeclareLabel("l_main")
                .CheckSurrounded(255, "l_rotate_and_eat", "l_try_dup_then_eat")
                .DeclareLabel("l_try_dup_then_eat")
                .CheckEnergy(64, "l_dup", "l_eat_or_sun")
                .DeclareLabel("l_dup")
                .Duplicate()
                .DeclareLabel("l_eat_or_sun")
                .CheckEnergy(15, "l_eat", "l_sun")
                .DeclareLabel("l_sun")
                .Photosynthesis()
                .Exit()
                .DeclareLabel("l_rotate_and_eat")
                .Rotate(3, false)
                .DeclareLabel("l_eat")
                .Eat(RelativeDirection.Forward.Value(), false, "l_go_forward", "l_rotate", "l_rotate", "l_go_forward")
                .DeclareLabel("l_rotate")
                .Rotate(3, false)
                .Exit()
                .DeclareLabel("l_go_forward")
                .Move(RelativeDirection.Forward.Value(), false, "l_stop", "l_stop", "l_stop", "l_stop")
                .DeclareLabel("l_stop")
                .Exit()
                .Build();
        }
    }
}