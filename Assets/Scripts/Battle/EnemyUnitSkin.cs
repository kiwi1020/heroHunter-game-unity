using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class EnemyUnitSkin : MonoBehaviour
{
    SpriteResolver[] resolvers;

    void Awake()
    {
        resolvers = GetComponentsInChildren<SpriteResolver>(); 
    }
    public void ChangeSkin(List<string> _skinNames)
    {
        foreach(SpriteResolver i in resolvers)
        {
            var categoryName = i.GetCategory();

            switch(categoryName)
            {
                case "Head":
                    i.SetCategoryAndLabel(categoryName, _skinNames[0]);
                    break;
                case "Body" or "Arm1F" or "Arm2F" or "Arm1B" or "Arm2B":
                    i.SetCategoryAndLabel(categoryName, _skinNames[1]);
                    break;
                case "HandF" or "HandB":
                    i.SetCategoryAndLabel(categoryName, _skinNames[2]);
                    break;
                case "Leg1F" or "Leg2F" or "Leg1B" or "Leg2B":
                    i.SetCategoryAndLabel(categoryName, _skinNames[3]);
                    break;
                case "FootF" or "FootB":
                    i.SetCategoryAndLabel(categoryName, _skinNames[4]);
                    break;
                case "Weapon":
                    i.SetCategoryAndLabel(categoryName, _skinNames[5]);
                    break;
            }

        }
    }
}
