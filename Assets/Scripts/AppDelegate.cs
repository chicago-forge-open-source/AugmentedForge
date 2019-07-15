using UnityEngine;

public class AppDelegate
{
    public static Sprite GetMapSprite()
    {
        var spritePath = $"Sprites/{PlayerPrefs.GetString("location")}Map";
        var mapObject = (GameObject) Resources.Load(spritePath);
        return mapObject.GetComponent<SpriteRenderer>().sprite;
    }
}