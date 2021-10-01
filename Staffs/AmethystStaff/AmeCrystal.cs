using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RStaffsMod.Staffs.AmethystStaff
{
    public class AmeCrystal : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.height = 10;
            projectile.width = 10;
            drawOriginOffsetY = -2;
            projectile.magic = true;
            projectile.penetrate = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.extraUpdates = 2;
            projectile.alpha = 255;
        }
        public override void AI()
        {
            projectile.alpha -= 25;
            Lighting.AddLight(projectile.position, 0.64f, 0f, 0.92f);
            projectile.ai[0]++;
            if (projectile.ai[0] >= 40)
            {
                projectile.velocity.Y += 0.35f;
            }
            if (projectile.velocity.Y >= 14.5f)
            {
                projectile.velocity.Y = 14.5f;
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item27, projectile.position);
            for (int i = 1; i <= 7; i++)
            {
                float x = Main.rand.NextFloat(-1, 1);
                float y = Main.rand.NextFloat(-1, 1);
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.AmethystBolt, x, y, 1);
            }
            return true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.PlaySound(SoundID.Item27, projectile.position);
            for (int i = 1; i <= 7; i++)
            {
                float x = Main.rand.NextFloat(-1, 1);
                float y = Main.rand.NextFloat(-1, 1);
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.AmethystBolt, x, y, 1);
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.defense > 0)
            {
                if (target.defense <= 5) damage += target.defense;
                else if (target.defense > 5) damage += 5;
            }
        }
    }
}
