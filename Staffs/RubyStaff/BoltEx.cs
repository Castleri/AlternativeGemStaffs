using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RStaffsMod.Staffs.RubyStaff
{
    public class BoltEx : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly;
        public override void SetDefaults()
        {
            projectile.magic = true;
            projectile.width = 45;
            projectile.height = 45;
            projectile.friendly = true;
            drawOffsetX = -11;
            drawOriginOffsetY = -11;
            projectile.penetrate = -1;
            projectile.timeLeft = 3;
            projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0 && !NPCID.Sets.ProjectileNPC[target.type])
            {
                for (int i = 0; i < 5; i++)
                {
                    float speedX = -3 + Main.rand.Next(0, 6);
                    float speedY = -9 + Main.rand.Next(0, 7);
                    Vector2 speed = new Vector2(speedX, speedY);
                    Projectile.NewProjectile(projectile.position, speed, ModContent.ProjectileType<Rubies>(), (int)(projectile.damage * 0.3f), 0, projectile.owner);
                }
            }
        }
    }
}
