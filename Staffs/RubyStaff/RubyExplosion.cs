using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RStaffsMod.Staffs.RubyStaff
{
    public class RubyExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            projectile.magic = true;
            projectile.friendly = true;
            projectile.width = 128;
            projectile.height = 128;
            projectile.timeLeft = 15;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 14;
            projectile.alpha = 55;
            projectile.scale = 0.7f;
            drawOffsetX = -19;
            drawOriginOffsetY = -16;
        }
        public override void AI()
        {
            projectile.ai[0]++;
            if (++projectile.frameCounter >= 3)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (projectile.ai[0] < 6)
            {
                return null;
            }
            else return false;
        }
    }
}
