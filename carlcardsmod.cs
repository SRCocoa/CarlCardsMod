using BepInEx;
using ModdingUtils.Extensions;
using ModdingUtils.MonoBehaviours;
using Photon.Pun.Simple;
using System.Dynamic;
using System.IO;
using System.Reflection;
using UnboundLib.Cards;
using UnityEngine;
using static ModdingUtils.Utils.SortingController;

[BepInPlugin("com.SRCocoa.CarlCardsModwithPhon", "CarlCards", "1.0.0")]
[BepInProcess("Rounds.exe")]

public static class AssetUtils
{
    public static Sprite LoadSpriteFromFile(string filePath)
    {
        // Check if the file exists
        if (!File.Exists(filePath))
        {
            Debug.LogError($"File {filePath} not found.");
            return null;
        }

        // Read the file as byte array
        byte[] fileData = File.ReadAllBytes(filePath);

        // Create a Texture2D and load the image data
        Texture2D texture = new Texture2D(2, 2); // Initial size doesn't matter; it will be overwritten
        texture.LoadImage(fileData); // LoadImage is an extension method in UnityEngine.ImageConversion

        // Create a Sprite from the Texture2D
        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f) // Pivot at the center
        );
    }
}
public class MetalPipe : CustomCard
{
    protected override string GetTitle()
    {
        return "Metal Pipe";
    }

    protected override string GetDescription()
    {
        return "hmm a metal pipe, oops i dropp- *explosion sfx*";
    }

    protected override GameObject GetCardArt()
    {
        // Construct the path to your image
        string filePath = Path.Combine(BepInEx.Paths.PluginPath, "CarlCardsModwithPhon/images/power_boost_card.png");

        // Load the sprite from the file
        Sprite cardSprite = AssetUtils.LoadSpriteFromFile(filePath);

        if (cardSprite != null)
        {
            // Create a GameObject to hold the sprite
            GameObject cardArt = new GameObject("CardArt");
            var spriteRenderer = cardArt.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = cardSprite;
            return cardArt;
        }

        // Return null if the sprite couldn't be loaded
        return null;
    }


    protected override CardInfo.Rarity GetRarity()
    {
        return CardInfo.Rarity.Rare;
    }

    protected override CardInfoStat[] GetStats()
    {
        return new CardInfoStat[]
        {
            new CardInfoStat()
            {
                positive = false,
                stat = "Gravity",
                amount = "+300%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotOf
            },
            new CardInfoStat()
            {
                positive = true,
                stat = "Damage",
                amount = "Some",
                simepleAmount = CardInfoStat.SimpleAmount.Some
            },
        };
    }

    public override void OnAddCard(
       Player player,
       Gun gun,
       GunAmmo gunAmmo,
       CharacterData data,
       HealthHandler health,
       Gravity gravity,
       Block block,
       CharacterStatModifiers characterStats)
    {
        // Increase player's gravity by 300%
        gravity.gravityForce *= 4.0f; // Default gravity is 1.0, so multiply by 4 for +300%
        gun.damage *= 1.5f;
    }

    public override void OnRemoveCard()
    {
        // Optional: Cleanup logic when the card is removed
    }

    protected override CardThemeColor.CardThemeColorType GetTheme()
    {
        return CardThemeColor.CardThemeColorType.TechWhite; // Example theme
    }

}
