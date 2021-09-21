using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace RStaffsMod.Staffs.RubyStaff
{
    public class Rubies : ModProjectile
    {
        public override string Texture => "Terraria/Item_178";
        public override void SetDefaults()
        {
            projectile.magic = true;
            projectile.friendly = true;
            projectile.width = 10;
            projectile.height = 10;
            projectile.penetrate = 1;
            drawOffsetX = -2;
            drawOriginOffsetY = -4;
        }
        public override void AI()
        {
            projectile.ai[0]++;
            if(projectile.ai[0] >= 9)
            {
                projectile.velocity.Y += 0.4f;
                if (projectile.velocity.Y > 16f)
                {
                    projectile.velocity.Y = 16f;
                }
            }
            projectile.rotation += 0.2f * projectile.direction;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(projectile.ai[0] >= 2)
            {
                return null;
            }
            else
            {
                return false;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.NewProjectile(projectile.position, Vector2.Zero, ModContent.ProjectileType<RubyExplosion>(), projectile.damage, 2, projectile.owner);
            target.immune[projectile.owner] = 8;
            Main.PlaySound(SoundID.Item14, projectile.position);
            base.OnHitNPC(target, damage, knockback, crit);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.NewProjectile(projectile.position, Vector2.Zero, ModContent.ProjectileType<RubyExplosion>(), projectile.damage, 2, projectile.owner);

            Main.PlaySound(SoundID.Item14, projectile.position);
            return base.OnTileCollide(oldVelocity);
        }
    }
}
