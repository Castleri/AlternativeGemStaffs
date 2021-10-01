using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace RStaffsMod.Staffs.SapphireStaff
{
    public class SapBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.magic = true;
            projectile.width = 16;
            projectile.height = 16;
            projectile.ignoreWater = false;
        }
        public override void AI()
        {

            projectile.rotation += 0.85f * projectile.direction;

            Lighting.AddLight(projectile.position, 0f, 0.48f, 0.896f);
            Dust dust;
            dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SapphireBolt)];
            dust.noGravity = true;


        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Sapphires>()] < 1)
            {
                float numproj = 8;
                float rotation = MathHelper.ToRadians(180);
                for (int a = 0; a < numproj; a++)
                {
                    Vector2 Speed = new Vector2(0.1f, 0.1f).RotatedBy(MathHelper.Lerp(-rotation, rotation, a / (numproj)));
                    Projectile.NewProjectile(projectile.Center, Speed, ModContent.ProjectileType<Sapphires>(), (int)(projectile.damage * 0.8f), 0, projectile.owner, 0, (float)(a * 45));
                }
                Main.PlaySound(SoundID.Item28);
                for (int i = 0; i < 20; i++)
                {
                    Dust dust;
                    dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, DustID.SapphireBolt)];
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
            return true;
        }
    }
}
