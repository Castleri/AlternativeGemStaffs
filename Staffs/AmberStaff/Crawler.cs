using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace RStaffsMod.Staffs.AmberStaff
{
    public class Crawler : ModProjectile
    {
        //Values to set interactions
        private int interaction = 0;
        private bool interact;
        
        private float maxSpeed = 5.25f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true;
        }
        public override void SetDefaults() 
        {
            projectile.magic = true;
            projectile.width = 20;
            projectile.height = 20;
            drawOffsetX = -8;
            drawOriginOffsetY = -10;
            projectile.alpha = 12;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.frame = 0;
            projectile.tileCollide = false;
            projectile.timeLeft = 600;
            projectile.hide = true;
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            projectile.ai[1]++;


            CheckCollideBlocks(); 

            if (interact)
            {
                InteractionProjectileNPC(ref interaction);
            }

        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindNPCsAndTiles.Add(index);        
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Manual Drawing for the crawlers to be centered
            Texture2D texture = Main.projectileTexture[projectile.type];
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = source.Size() / 2f;
            Vector2 value = new Vector2(projectile.width, projectile.height) / 2;
            spriteBatch.Draw(texture, projectile.position + value - Main.screenPosition, source, lightColor, projectile.rotation, origin, projectile.scale, SpriteEffects.None,0.1f);
            
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Manual Drawing for the "life bar"
            Texture2D texture = Main.hbTexture1;
            Texture2D texture2 = Main.hbTexture2;
            Rectangle source = new Rectangle(0, 0, texture.Width - (int)(projectile.ai[1] / 16.66f), texture.Height);
            Rectangle source2 = new Rectangle(0, 0, texture2.Width, texture2.Height);
            Vector2 origin = source.Size() / 2f;
            Vector2 origin2 = source2.Size() / 2f;
            Vector2 value = new Vector2(projectile.width, projectile.height) / 2;
            Color color = new Color(0f,0f,0f);
            
            //Changing color depending on the life
            if (texture.Width - (int)(projectile.ai[1] / 16.66f) > 28)
            {
                color.G = 150;
            }
            else if (texture.Width - (int)(projectile.ai[1] / 16.66f) > 10)
            {
                color.G = 170;
                color.R = 255;
            }
            else
            {
                color.R = 165;
            }
            color.A = 150;
            spriteBatch.Draw(texture2, projectile.position + value + new Vector2(0, 20) - Main.screenPosition, source2, color, 0, origin2, projectile.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, projectile.position + value + new Vector2(0 - (int)(projectile.ai[1] / 28.66f), 20) - Main.screenPosition, source, color, 0, origin, projectile.scale, SpriteEffects.None, 0f);

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.netUpdate = true;
            switch (target.type)
            {
                case 25:
                    interact = true;
                    interaction = 1;
                    projectile.ai[0]++;
                    break;
                case 30:
                    projectile.Kill();
                    break;
                case 33:
                    interact = true;
                    interaction = 2;
                    break;
                default:
                    if (crit)
                    {
                        float damage1 = damage * 1.5f;
                        projectile.ai[1] += (int)damage1;
                        projectile.timeLeft -= (int)damage1;
                        CombatText.NewText(new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height), Color.Red, (int)damage1);
                    }
                    else
                    {
                        float damage1 = damage * 3;
                        projectile.ai[1] += damage1;
                        projectile.timeLeft -= (int)damage1;
                        CombatText.NewText(new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height), Color.OrangeRed, (int)damage1);
                    }
                    break;
            }
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.NPCDeath1,projectile.position);
            Gore.NewGore(projectile.position,projectile.velocity,GoreID.TombCrawlerHead, 1f);
            Gore.NewGore(projectile.position, projectile.velocity, GoreID.TombCrawlerTail, 1f);
        }

        private void CheckCollideBlocks()
        {
            //Since the crawlers have tileCollide = false, manual check to see if the hitbox is colliding with tiles to see if the
            //search and targeting takes place
            if (Collision.SolidCollision(projectile.position,projectile.width,projectile.height))
            {
                projectile.ai[0]++;

                if(projectile.ai[0] >= 12)
                {
                    projectile.ai[0] = 0;
                    Main.PlaySound(SoundID.ForceRoar,(int)projectile.position.X,(int)projectile.position.Y,1,0.5f);
                }
                float d = 420;
                bool found = false;
                Vector2 centergo = projectile.position;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy())
                    {
                        float dpt = Vector2.Distance(projectile.Center, npc.Center);
                        bool inRange = dpt < d;
                        if ((inRange && !found) || dpt < d)
                        {
                            d = dpt;
                            found = true;
                            centergo = npc.Center;
                        }
                    }
                }
                if (found)
                {
                    projectile.velocity = Vector2.Normalize(centergo - projectile.Center) + (projectile.oldVelocity);

                }

                else
                {
                    Player player = Main.player[projectile.owner];
                    projectile.velocity = ((Vector2.Normalize(player.Center - projectile.Center) * 0.55f) + (projectile.oldVelocity));
                }


                if (projectile.velocity.Y > maxSpeed)
                {
                    projectile.velocity.Y = maxSpeed;
                }
                else if (projectile.velocity.Y < -maxSpeed)
                {
                    projectile.velocity.Y = -maxSpeed;
                }

                if (projectile.velocity.X > maxSpeed)
                {
                    projectile.velocity.X = maxSpeed;
                }
                else if (projectile.velocity.X < -(maxSpeed))
                {
                    projectile.velocity.X = -(maxSpeed);
                }
            }

            //If it is not colliding, gravity takes place
            else
            {
                projectile.ai[0] = 0;
                projectile.velocity.Y += 0.2f;
            }
        }

        private void InteractionProjectileNPC(ref int interactionType)
        {
            //Crawlers have special interactions when they hit some ProjectileNPCs, specifically Burning Sphere and Water Sphere.
            //This interactions are similar to debuffs

            switch (interactionType)
            {
                case 1:
                    //Lose life twice as fast
                    projectile.ai[1]++;
                    projectile.timeLeft--;
                    if(Main.rand.NextFloat() < 0.85f)
                    {
                        Dust dust;
                        dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 0, 0, 0, default, 2f)];
                        dust.noGravity = true;
                    }
                    break;
                case 2:
                    //Max Speed is reduced
                    maxSpeed = 3.5f;
                    if (Main.rand.NextFloat() < 0.75f)
                    {
                        Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Water, 0, 0, 0, default, 2f);
                    }
                    break;
            }
        }
    }
}
