using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace RStaffsMod.Staffs.DiamondStaff
{
    public class DiaBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.magic = true;
            projectile.width = 6;
            projectile.height = 6;
            drawOffsetX = -2;
            drawOriginOffsetY = -2;
            projectile.penetrate = 3;
            projectile.tileCollide = false;
            projectile.timeLeft = 720;
        }
        public override void AI()
        {
            projectile.ai[0]++;
            projectile.ai[1]++;
            if(projectile.ai[1] >= 3)
            {
                Dust dust;
                dust = Dust.NewDustPerfect(projectile.Center, DustID.DiamondBolt, new Vector2(0, 0));
                dust.noGravity = true;
                projectile.ai[1] = 0;
            }
            projectile.rotation = projectile.velocity.ToRotation();

            //Wait 10 frames to start homing
            if(projectile.ai[0] >= 10)
            {
                float d = 128;
                bool targetfound = false;
                Vector2 targetcenter = projectile.position;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy())
                    {
                        float dpt = Vector2.Distance(projectile.Center, npc.Center);
                        bool inRange = dpt < d;
                        if ((inRange && !targetfound) || dpt < d)
                        {
                            d = dpt;
                            targetfound = true;
                            targetcenter = npc.Center;
                        }
                    }

                    if (targetfound)
                    {
                        projectile.velocity = ((Vector2.Normalize(targetcenter - projectile.Center)) + projectile.oldVelocity * 0.87f);
                        if (projectile.velocity.Y > 6.5f)
                        {
                            projectile.velocity.Y = 6.5f;
                        }
                        else if (projectile.velocity.Y < -6.5f)
                        {
                            projectile.velocity.Y = -6.5f;
                        }
                        if (projectile.velocity.X > 6.5f)
                        {
                            projectile.velocity.X = 6.5f;
                        }
                        else if (projectile.velocity.X < -6.5f)
                        {
                            projectile.velocity.X = -6.5f;
                        }
                    }

                    else
                    {
                        projectile.velocity = new Vector2(6.5f, 0).RotatedBy(projectile.rotation);
                    }
                }
            }
            
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("RStaffsMod/Staffs/DiamondStaff/DiaGlow");
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = source.Size() / 2f;
            Vector2 value = new Vector2(projectile.width, projectile.height) / 2;
            lightColor.A = 205;
            float a = ((float)Math.Sin(projectile.ai[0] / 15) + 1.15f) / 4f;
            spriteBatch.Draw(texture, projectile.position + value - Main.screenPosition, source, lightColor * a, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 4; i++)
            {
                Vector2 speed = new Vector2(0, -3).RotatedBy(MathHelper.PiOver2 * i);
                Dust dust;
                dust = Dust.NewDustPerfect(projectile.position, DustID.DiamondBolt, speed, 0, default, 1.4f);
                dust.noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.ai[0] = 0;
            target.immune[projectile.owner] = 10;
            if (target.life <= 0) projectile.Kill();
        }
    }
}
