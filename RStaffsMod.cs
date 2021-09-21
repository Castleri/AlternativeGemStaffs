using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RStaffsMod
{
	public class RStaffsMod : Mod
	{
        private Texture2D ogAmStaff,ogTopStaff,ogSapStaff,ogEmStaff,ogRuStaff,ogDiaStaff;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Ref<Effect> emboltref = new Ref<Effect>(GetEffect("Effects/EmShader"));

                GameShaders.Misc["EmShader"] = new MiscShaderData(emboltref, "TGambleShaders");

                ogAmStaff = Main.itemTexture[ItemID.AmethystStaff];
                ogTopStaff = Main.itemTexture[ItemID.TopazStaff];
                ogSapStaff = Main.itemTexture[ItemID.SapphireStaff];
                ogEmStaff = Main.itemTexture[ItemID.EmeraldStaff];
                ogRuStaff = Main.itemTexture[ItemID.RubyStaff];
                ogDiaStaff = Main.itemTexture[ItemID.DiamondStaff];
                Main.itemTexture[ItemID.AmethystStaff] = GetTexture("Resprites/AmStaff");
                Main.itemTexture[ItemID.TopazStaff] = GetTexture("Resprites/TopStaff");
                Main.itemTexture[ItemID.SapphireStaff] = GetTexture("Resprites/SapStaff");
                Main.itemTexture[ItemID.EmeraldStaff] = GetTexture("Resprites/EmStaff");
                Main.itemTexture[ItemID.RubyStaff] = GetTexture("Resprites/RuStaff");
                Main.itemTexture[ItemID.DiamondStaff] = GetTexture("Resprites/DiaStaff"); 
            }
        }
        public override void Unload()
        {
            if (!Main.dedServ)
            {
                Main.itemTexture[ItemID.AmethystStaff] = ogAmStaff;
                Main.itemTexture[ItemID.TopazStaff] = ogTopStaff;
                Main.itemTexture[ItemID.SapphireStaff] = ogSapStaff;
                Main.itemTexture[ItemID.EmeraldStaff] = ogEmStaff;
                Main.itemTexture[ItemID.RubyStaff] = ogRuStaff;
                Main.itemTexture[ItemID.DiamondStaff] = ogDiaStaff;
            }
        }
    }
}