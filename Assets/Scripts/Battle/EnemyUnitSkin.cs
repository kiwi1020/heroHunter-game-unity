using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class EnemyUnitSkin : MonoBehaviour
{
    public IEnumerator ChangeSkin(List<string> _skinNames)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();


        print("in?");
        foreach (SpriteResolver i in GetComponentsInChildren<SpriteResolver>())
        {
            switch (i.GetCategory())
            {
                case "head":
                    i.SetCategoryAndLabel("head", _skinNames[0]);//
                    break;
                case "Body":
                    i.SetCategoryAndLabel("Body", _skinNames[1]);
                    break;
                case "Core":
                    i.SetCategoryAndLabel("Core", _skinNames[1]);
                    break;
                case "Arm1F":
                    i.SetCategoryAndLabel("Arm1F", _skinNames[1]);
                    break;
            }
        }

        foreach (SpriteResolver i in GetComponentsInChildren<SpriteResolver>())
        {
            switch (i.GetCategory())
            {
                case "Arm2F":
                    i.SetCategoryAndLabel("Arm2F", _skinNames[1]);
                    break;
                case "Arm1B":
                    i.SetCategoryAndLabel("Arm1B", _skinNames[1]);
                    break;
                case "Arm2B":
                    i.SetCategoryAndLabel("Arm2B", _skinNames[1]);
                    break;
            }
        }

        foreach (SpriteResolver i in GetComponentsInChildren<SpriteResolver>())
        {
            switch (i.GetCategory())
            {
                case "HandF":
                    i.SetCategoryAndLabel("HandF", _skinNames[2]);
                    break;
                case "HandB":
                    i.SetCategoryAndLabel("HandB", _skinNames[2]);
                    break;
                case "Leg1F":
                    i.SetCategoryAndLabel("Leg1F", _skinNames[3]);
                    break;
            }
        }

        foreach (SpriteResolver i in GetComponentsInChildren<SpriteResolver>())
        {
            switch (i.GetCategory())
            {
                case "Leg2F":
                    i.SetCategoryAndLabel("Leg2F", _skinNames[3]);
                    break;
                case "FootF":
                    i.SetCategoryAndLabel("FootF", _skinNames[4]);
                    break;
                case "Leg1B":
                    i.SetCategoryAndLabel("Leg1B", _skinNames[3]);
                    break;
            }
        }

        foreach (SpriteResolver i in GetComponentsInChildren<SpriteResolver>())
        {
            switch (i.GetCategory())
            {
                case "Leg2B":
                    i.SetCategoryAndLabel("Leg2B", _skinNames[3]);
                    break;
                case "FootB":
                    i.SetCategoryAndLabel("FootB", _skinNames[4]);
                    break;
                case "Weapon":
                    i.SetCategoryAndLabel("Weapon", _skinNames[5]);
                    break;
            }
        }

    }

}


/*
var categoryName = i.GetCategory();
if(categoryName == "Head")
{
    print(categoryName);
    print(_skinNames[0]);
    i.SetCategoryAndLabel(categoryName, _skinNames[0]);
}
if (categoryName == "Body") i.SetCategoryAndLabel(categoryName, _skinNames[1]);
if (categoryName == "Arm1F") i.SetCategoryAndLabel(categoryName, _skinNames[1]);
if (categoryName == "Arm1B") i.SetCategoryAndLabel(categoryName, _skinNames[1]);
if (categoryName == "Arm2F") i.SetCategoryAndLabel(categoryName, _skinNames[1]);
if (categoryName == "Arm2B") i.SetCategoryAndLabel(categoryName, _skinNames[1]);

else if ( categoryName == "HandF" || categoryName == "HandB") i.SetCategoryAndLabel(categoryName, _skinNames[2]);
else if (categoryName == "Leg1F" || categoryName == "Leg1B"
    || categoryName == "Leg2F" || categoryName == "Leg2B") i.SetCategoryAndLabel(categoryName, _skinNames[3]);
else if (categoryName == "FootF" || categoryName == "FootB") i.SetCategoryAndLabel(categoryName, _skinNames[4]);
else if (categoryName == "Weapon") i.SetCategoryAndLabel(categoryName, _skinNames[5]);

*/