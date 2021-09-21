using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RStaffsMod.Staffs.RubyStaff
{
    public class RuBolt : ModProjectile
    {
        public Vector2 pos;
        public int style;
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            projectile.magic = true;
            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = true;
            drawOffsetX = -4;
            drawOriginOffsetY = -4;
            projectile.penetrate = 1;
        }
        public override void AI()
        {
            Lighting.AddLight(projectile.position, 0.8f, 0.088f, 0.088f);
            projectile.ai[0]++;
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (++projectile.frameCounter >= 7)
            {
                projectile.frame++;
                if (projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
                projectile.frameCounter = 0;
            }
            Dust dust;
            dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.RubyBolt, -projectile.velocity.X * 0.4f, -projectile.velocity.Y * 0.4f)];
            dust.noGravity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {  
            Main.PlaySound(SoundID.Item14, projectile.position);
            for (int i = 0; i < 25; i++)
            {
                Vector2 speed = new Vector2(5.5f, 0).RotatedByRandom(180);
                Dust dust;
                dust = Main.dust[Dust.NewDust(projectile.Center, 1, 1, DustID.RubyBolt, speed.X, speed.Y, 0, default, 1.6f)];
                dust.noGravity = true; 
            }

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
            target.immune[projectile.owner] = 5;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item14, projectile.position);
            for (int i = 0; i < 25; i++)
            {
                Vector2 speed = new Vector2(5.5f, 0).RotatedByRandom(180);
                Dust dust;
                dust = Main.dust[Dust.NewDust(projectile.Center, 1, 1, DustID.RubyBolt, speed.X, speed.Y, 0, default, 1.3f)];
                dust.noGravity = true;
            }
            
            return true;
        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<BoltEx>(), (int)(projectile.damage * 0.7f), projectile.knockBack, projectile.owner);
        }
    }
}
