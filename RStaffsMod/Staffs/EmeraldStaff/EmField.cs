using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using System;
using System.Linq;

namespace RStaffsMod.Staffs.EmeraldStaff
{
    public class EmField : ModProjectile
    {
        public float t,n;
        public bool can = true;
        public bool field = true;
        private bool resetbatch;
        public int fstarter = ModContent.ProjectileType<FieldStarter>();
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
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 4000;
        }
        public override void AI()
        {
            Lighting.AddLight(projectile.position, 0.56f, 1f, 0.32f);
            projectile.ai[1]++;
            t++;


            Player player = Main.player[projectile.owner];
            Player player2 = Main.player[Main.myPlayer];
            if (t >= 20 && player.ownedProjectileCounts[projectile.type] > 1)
            {
                projectile.Kill();
            }
            if (t >= 20)
            {

                projectile.ai[0]++;
                projectile.rotation += 0.025f;
                can = false;
                projectile.penetrate = -1;

                if(projectile.ai[0] >= 30)
                {
                    for (int i = 1; i < 360; i += 2)
                    {
                        double deg = i * 2;
                        double rad = deg * (Math.PI / 180);
                        double dist = 640;

                        float posX = projectile.Center.X - (int)(Math.Cos(rad) * dist);
                        float posY = projectile.Center.Y - (int)(Math.Sin(rad) * dist);

                        Dust dust2;
                        Vector2 position = new Vector2(posX, posY);
                        dust2 = Dust.NewDustPerfect(position, 89, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 1f);
                        dust2.noGravity = true;

                    }
                    projectile.ai[0] = 0;
                }
                float dpp = Vector2.Distance(player2.Center, projectile.Center);
                if (dpp <= 640)
                {
                    if (!player2.HasBuff(ModContent.BuffType<Buffs.EmBuff>()))
                    {
                        player2.AddBuff(ModContent.BuffType<Buffs.EmBuff>(), 18000);
                    }
                }
                else
                {
                    player2.ClearBuff(ModContent.BuffType<Buffs.EmBuff>());
                }
            }
            var check = Main.projectile.Where(starter => starter.active && starter.type == fstarter && starter.active);
            if (player.altFunctionUse == 2 && player.HeldItem.type == ItemID.EmeraldStaff && check.Any())
            {
                projectile.timeLeft = 279;
            }

            if (projectile.timeLeft <= 280)
            {
                projectile.alpha += 15;
                if (projectile.alpha >= 255) projectile.Kill();
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                resetbatch = true;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix); // SpriteSortMode needs to be set to Immediate for shaders to work.

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
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
    }
}
