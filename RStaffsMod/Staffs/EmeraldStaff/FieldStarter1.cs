using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace RStaffsMod.Staffs.EmeraldStaff
{
    public class FieldStarter1 : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly;
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.magic = true;
            projectile.width = 8;
            projectile.height = 8;
        }
        public override void AI()
        {
            projectile.ai[0]++;
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.velocity += new Vector2(0, -0.40f).RotatedBy(projectile.rotation);
            Dust trail;
            trail = Dust.NewDustPerfect(projectile.position, DustID.EmeraldBolt, null, 0, default, 1f);
            trail.noGravity = true;
        }
    }
}
