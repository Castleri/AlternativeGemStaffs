using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RStaffsMod
{
    public class GItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            int id = item.type;
            switch (id)
            {
                case 739:
                    item.damage = 8;
                    item.useTime = 16;
                    item.useAnimation = 16;
                    item.shoot = ModContent.ProjectileType<Staffs.AmethystStaff.AmeCrystal>();
                    item.mana = 4;
                    item.shootSpeed = 5.5f;
                    item.autoReuse = true;
                    item.UseSound = null;
                    item.rare = ItemRarityID.Blue;
                    break;
                case 740:
                    item.damage = 15;
                    item.channel = true;
                    item.mana = 4;
                    item.autoReuse = true;
                    item.useTime = 36;
                    item.useAnimation = 36;
                    item.shoot = ModContent.ProjectileType<Staffs.TopazStaff.TopBolt>();
                    item.rare = ItemRarityID.Blue;
                    break;
                case 741:
                    item.autoReuse = true;
                    item.shoot = ModContent.ProjectileType<Staffs.SapphireStaff.SapBolt>();
                    item.useTime = 24;
                    item.useAnimation = 24;
                    item.rare = ItemRarityID.Blue;
                    break;
                case 742:
                    item.damage = 20;
                    item.useTime = 9;
                    item.useAnimation = 27;
                    item.reuseDelay = 25;
                    item.rare = ItemRarityID.Blue;
                    item.shoot = ModContent.ProjectileType<Staffs.EmeraldStaff.EmBolt>();
                    item.shootSpeed = 9.75f;
                    item.knockBack = 4.5f;
                    break;
                case 743:
                    item.autoReuse = true;
                    item.damage = 25;
                    item.shootSpeed = 13.5f;
                    item.UseSound = SoundID.Item45;
                    item.useTime = 24;
                    item.useAnimation = 24;
                    item.shoot = ModContent.ProjectileType<Staffs.RubyStaff.RuBolt>();
                    item.rare = ItemRarityID.Blue;
                    item.knockBack = 6.25f;
                    break;
                case 744:
                    item.damage = 17;
                    item.useAnimation = 21;
                    item.useTime = 21;
                    item.shoot = ModContent.ProjectileType<Staffs.DiamondStaff.DiaBolt>();
                    item.shootSpeed = 6.5f;
                    item.autoReuse = true;
                    item.knockBack = 4f;
                    item.rare = ItemRarityID.Blue;
                    item.mana = 7;
                    break;
                case 3377:
                    item.shoot = ModContent.ProjectileType<Staffs.AmberStaff.Crawler>();
                    item.knockBack = 1.75f;
                    item.damage = 27;
                    item.shootSpeed = 9.75f;
                    item.mana = 8;
                    item.useTime = 24;
                    item.useAnimation = 24;
                    break;
            }
            base.SetDefaults(item);
        }
        public override bool AltFunctionUse(Item item, Player player)
        {
            int id = item.type;
            switch (id)
            {
                case 742:
                    return true;
            }
            return base.AltFunctionUse(item, player);
        }

        public override bool CanUseItem(Item item, Player player)
        {
            int id = item.type;
            switch (id)
            {
                case 740:
                    if (player.channel)
                    {
                        item.useAnimation -= 2;
                        item.useTime -= 2;
                        item.shootSpeed += 0.4f;
                        if (item.useTime == 20)
                        {
                            Main.PlaySound(SoundID.Item28);
                            for (int i = 0; i < 20; i++)
                            {
                                Dust.NewDust(player.position, player.width, player.height, DustID.TopazBolt);
                            }
                        }
                        if (item.useTime < 20) item.useTime = 20;
                        if (item.useAnimation < 20) item.useAnimation = 20;
                        if (item.shootSpeed > 9.2f) item.shootSpeed = 9.2f;
                    }
                    else if (!player.channel)
                    {
                        item.useAnimation = 36;
                        item.useTime = 36;
                        item.shootSpeed = 6.5f;
                    }
                    return true;
            }
            return base.CanUseItem(item, player);
        }
        
        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int id = item.type;
            switch (id)
            {
                case 739:
                    int numberProjectiles = 2;
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 OffsetAm = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
                        if (Collision.CanHit(position, 0, 0, position + OffsetAm, 0, 0))
                        {
                            position += OffsetAm;
                        }
                        Vector2 Speed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
                        Projectile.NewProjectile(position.X, position.Y, Speed.X, Speed.Y, type, damage, knockBack, player.whoAmI);
                    }
                    return false;


                case 740:
                    Vector2 OffsetTo = Vector2.Normalize(new Vector2(speedX, speedY)) * (item.useTime + 15);
                    if (Collision.CanHit(position, 0, 0, position + OffsetTo, 0, 0))
                    {
                        position += OffsetTo;
                    }
                    return true;


                case 741:
                    Vector2 OffsetSap = Vector2.Normalize(new Vector2(speedX, speedY)) * 60f;
                    if (Collision.CanHit(position, 0, 0, position + OffsetSap, 0, 0))
                    {
                        position += OffsetSap;
                    }
                    return true;


                case 742:
                    Vector2 OffsetEm = Vector2.Normalize(new Vector2(speedX, speedY)) * 60f;
                    if (Collision.CanHit(position, 0, 0, position + OffsetEm, 0, 0))
                    {
                        position += OffsetEm;
                    }
                    if (player.altFunctionUse == 2)
                    {
                        if (player.ownedProjectileCounts[ModContent.ProjectileType<Staffs.EmeraldStaff.FieldStarter>()] >= 1)
                        {
                            return false;
                        }
                        for(int i = -1; i < 2; i += 2)
                        {
                            if(i == -1)
                            {
                                type = ModContent.ProjectileType<Staffs.EmeraldStaff.FieldStarter>();
                            }
                            else if (i == 1)
                            {
                                type = ModContent.ProjectileType<Staffs.EmeraldStaff.FieldStarter1>();
                            }
                            Vector2 speed = new Vector2(speedX, speedY).RotatedBy(0.785 * i);
                            Projectile.NewProjectile(position, speed, type, 0, 0, player.whoAmI);
                        }

                        return false;
                    }
                    return true;


                case 743:
                    Vector2 OffsetRu = Vector2.Normalize(new Vector2(speedX, speedY)) * 60f;
                    if (Collision.CanHit(position, 0, 0, position + OffsetRu, 0, 0))
                    {
                        position += OffsetRu;
                    }
                    return true;
                case 744:
                    Vector2 OffsetDia = Vector2.Normalize(new Vector2(speedX, speedY)) * 55f;
                    if (Collision.CanHit(position, 0, 0, position + OffsetDia, 0, 0))
                    {
                        position += OffsetDia;
                    }
                    return true;


                case 3377:
                    Vector2 OffsetAmb = Vector2.Normalize(new Vector2(speedX, speedY)) * 35f;
                    if (Collision.CanHit(position, 0, 0, position + OffsetAmb, 0, 0))
                    {
                        position += OffsetAmb;
                    }
                    return true;


            }
            return base.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            int id = item.type;
            switch (id)
            {
                case 739:
                    var line739 = new TooltipLine(mod, "", "Shoots 2 Amethyst Crystals with low range that ignore 5 defense");
                    tooltips.Add(line739);
                    break;
                case 740:
                    var line740 = new TooltipLine(mod, "", "Gains more speed the more you keep using it");
                    tooltips.Add(line740);
                    break;
                case 741:
                    var line741 = new TooltipLine(mod, "", "Casts a spinning bolt that creates Sapphires around you\nto protect you when it hits an enemy");
                    tooltips.Add(line741);
                    break;
                case 742:
                    var line742 = new TooltipLine(mod, "", "Shoots emerald bolts with random curved paths\nRight Click to create a magic field that increases your magic powers");
                    tooltips.Add(line742);
                    break;
                case 743:
                    var line743 = new TooltipLine(mod, "", "Shoots a fast bolt with a small area of damage\nReleases explosive rubies when it kills an enemy");
                    tooltips.Add(line743);
                    break;
                case 744:
                    var line744 = new TooltipLine(mod, "", "Casts small bolts that homes to close enemies");
                    tooltips.Add(line744);
                    break;
                case 3377:
                    var line3377 = new TooltipLine(mod, "", "Summons Tomb Crawlers that bite enemies\nTomb Crawlers lose life constantly and when they hit enemies");
                    tooltips.Add(line3377);
                    break;
            }
            base.ModifyTooltips(item, tooltips);
        }
    }
}
