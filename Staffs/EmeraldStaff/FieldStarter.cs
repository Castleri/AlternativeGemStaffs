using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace RStaffsMod.Staffs.EmeraldStaff
{
    public class FieldStarter : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly;
        public int starter1 = ModContent.ProjectileType<FieldStarter1>();
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
            projectile.velocity += new Vector2(0, 0.40f).RotatedBy(projectile.rotation);

            //Check colliding between the two projectiles
            
            var checkcollide = Main.projectile.Where(starter => starter.active && starter.type == starter1 && starter.active && (starter.Hitbox.Intersects(projectile.Hitbox)));
            if (checkcollide.Any() && projectile.ai[0] > 5)
            {
                foreach (var proj in checkcollide)
                {
                    proj.Kill();
                    Projectile.NewProjectile(projectile.position, Vector2.Zero, ModContent.ProjectileType<EmField>(), 0, 0, projectile.owner);
                    Main.PlaySound(SoundID.Item4);
                    for (int a = 0; a < 8; a++)
                    {
                        for (int b = 1; b < 5; b++)
                        {
                            Vector2 speed = new Vector2(0, -(3 + b)).RotatedBy(MathHelper.PiOver4 * a);
                            Dust dust;
                            dust = Dust.NewDustPerfect(projectile.position, DustID.EmeraldBolt, speed, 0, default, 1.4f);
                            dust.noGravity = true;
                        }
                    }
                    projectile.Kill();
                }
            }



            Dust trail;
            trail = Dust.NewDustPerfect(projectile.position, DustID.EmeraldBolt,null,0,default,1f);
            trail.noGravity = true;
        }
    }
}
