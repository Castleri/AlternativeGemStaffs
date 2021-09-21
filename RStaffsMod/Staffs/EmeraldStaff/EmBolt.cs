using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using System;

namespace RStaffsMod.Staffs.EmeraldStaff
{
    public class EmBolt : ModProjectile
    {

        private bool resetbatch;
        private float dev;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            projectile.magic = true;
            projectile.friendly = true;
            projectile.width = 14;
            projectile.height = 14;
            drawOffsetX = -4;
            drawOriginOffsetY = -4;
            projectile.penetrate = 1;
            projectile.usesLocalNPCImmunity = true;
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            Lighting.AddLight(projectile.position, 0.56f, 1f, 0.32f);
            projectile.ai[0]++;
            projectile.ai[1]++;

            if (projectile.ai[1] == 1) dev = Main.rand.NextFloat(-0.37f, 0.37f);
            else if (projectile.ai[1] > 5 && projectile.ai[1] <= 25) projectile.velocity += new Vector2(0, dev).RotatedBy(projectile.rotation);

            if (projectile.ai[0] >= 480)
            {
                projectile.alpha += 5;
                if(projectile.alpha > 255)
                {
                    projectile.alpha = 255;
                    projectile.Kill();
                }
            }
            for (int i = 0; i < 2; i++)
            {
                Dust dust;
                dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.EmeraldBolt)];
                dust.noGravity = true;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("RStaffsMod/Staffs/EmeraldStaff/EmBoltTrail");
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = source.Size() / 2f;
            Vector2 value = new Vector2(projectile.width, projectile.height) / 2;
            Color color = projectile.GetAlpha(Color.GhostWhite);
            color.A = 205;
            for (int k = projectile.oldPos.Length - 1; k > 0; k--)
            {
                Vector2 vector = projectile.oldPos[k] + value;
                if (!(vector == value))
                {
                    Vector2 vector2 = projectile.oldPos[k - 1] + value;
                    float rot = (vector2 - vector).ToRotation() - MathHelper.Pi / 2;
                    Color color1 = color * (0.9f - (float)k / (float)projectile.oldPos.Length);
                    spriteBatch.Draw(texture, vector - Main.screenPosition, source, color1, rot, origin, projectile.scale, SpriteEffects.None, 0f);
                }
            }
            if (Main.netMode != NetmodeID.Server)
            {
                resetbatch = true;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                GameShaders.Misc["EmShader"].Apply();
            }
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (resetbatch)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                resetbatch = false;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = new Vector2(5.5f, 0).RotatedByRandom(180);
                Dust dust;
                dust = Main.dust[Dust.NewDust(projectile.Center, 1, 1, DustID.EmeraldBolt, speed.X, speed.Y, 0, default, 1.3f)];
                dust.noGravity = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            for (int i = 0; i < 15; i++)
            {
                Vector2 speed = new Vector2(5.5f, 0).RotatedByRandom(180);
                Dust dust;
                dust = Main.dust[Dust.NewDust(projectile.Center, 1, 1, DustID.EmeraldBolt, speed.X, speed.Y, 0, default, 1.3f)];
                dust.noGravity = true;
            }
            return true;
        }
    }
}
