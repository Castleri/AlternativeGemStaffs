using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace RStaffsMod.Staffs.TopazStaff
{
    public class TopBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.magic = true;
            projectile.width = 12;
            projectile.height = 12;
            projectile.penetrate = 5;
            drawOffsetX = -4;
            drawOriginOffsetY = -4;
        }
        public override void AI()
        {
            Lighting.AddLight(projectile.position, 0.9f, 0.735f, 0f);
            projectile.rotation = projectile.velocity.ToRotation();
            for (int i = 0; i < 2; i++)
            {
                Dust dust;
                dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.TopazBolt, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                dust.noGravity = true;
            }
            if (++projectile.frameCounter >= 5)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.penetrate > 1) target.immune[projectile.owner] = 10;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
            return true;
        }
    }
}
