using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RStaffsMod.Staffs.SapphireStaff
{
    public class Sapphires : ModProjectile
    {
        public int t,d;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.magic = true;
            projectile.height = 10;
            projectile.width = 10;
            drawOffsetX = -4;
            drawOriginOffsetY = -4;
            projectile.penetrate = -1;
        }
        public override void AI()
        {
            projectile.ai[0]++;
            t += 2;
            if(t > 90 && projectile.ai[0] < 528)
            {
                t = 90;
            }
            Lighting.AddLight(projectile.position, 0.090f, 0.576f, 0.917f);
            Player player = Main.player[projectile.owner];
            double deg = (double)projectile.ai[1]; 
            double rad = deg * (Math.PI / 180);
            double dist = t;
            projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width / 2;
            projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height / 2; 
            projectile.ai[1] += 2.2f;

            if (projectile.ai[0] >= 528) t += 6;
            if (projectile.ai[0] >= 588) projectile.alpha += 5;
            if (projectile.ai[0] >= 628)
            {
                projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            Main.PlaySound(SoundID.Item27, projectile.position);
            
            if(target.knockBackResist != 0f)
            {
                float xcom = ((projectile.Center.X - player.Center.X) * 0.05f) * target.knockBackResist;
                float ycom = ((projectile.Center.Y - player.Center.Y) * 0.05f) * target.knockBackResist;
                if (new Vector2(xcom, ycom) != Vector2.Zero)
                {
                    if(xcom > (4.8f * target.knockBackResist))
                    {
                        xcom = 4.8f * target.knockBackResist;
                    }
                    else if (xcom < (-4.8f * target.knockBackResist))
                    {
                        xcom = -4.8f * target.knockBackResist;
                    }
                    if(ycom > (4.8f * target.knockBackResist))
                    {
                        ycom = 4.8f * target.knockBackResist;
                    }
                    else if (ycom < (-4.8f * target.knockBackResist))
                    {
                        ycom = -4.8f * target.knockBackResist;
                    }
                    if (!crit) target.velocity = new Vector2(xcom, ycom);
                    else if (crit) target.velocity = new Vector2(xcom, ycom) * 1.5f;
                    
                    for (int i = 1; i <= 30; i++)
                    {
                        Dust dust;
                        dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SapphireBolt, xcom * 5, ycom * 5, 1)];
                        dust.noGravity = true;
                    }
                }
            }
            
        }
        public override void Kill(int timeLeft)
        {

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("RStaffsMod/Staffs/SapphireStaff/SapTrail");
            Vector2 value = new Vector2(projectile.width, projectile.height) / 2f;
            Color color = Color.White;
            color.A = 150;
            for (int k = projectile.oldPos.Length - 1; k > 0; k--)
            {
                Vector2 vector = projectile.oldPos[k] + value;
                if (!(vector == value))
                {
                    Vector2 vector2 = projectile.oldPos[k - 1] + value;
                    float rot = (vector2 - vector).ToRotation() - (float)Math.PI / 2f;
                    if (t < 72)
                    {
                        d = (int)Vector2.Distance(vector2, vector);
                    }
                    else if (t >= 72)
                    {
                        d = (int)Vector2.Distance(vector2, vector) + 8;
                    }
                    Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height + d);
                    Vector2 origin = source.Size() / 2;
                    float scale = 1f - (k * 0.04f);
                    Color color2 = color * (1f - (float)k / (float)projectile.oldPos.Length);
                    spriteBatch.Draw(texture, vector - Main.screenPosition, source, color2, rot, origin, scale, SpriteEffects.None,1f);
                }
            }
            return true;
        }
    }
}
