using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCollectable : CollectableObject
{
    public float MoneyValue;

    public override void Collect(SheriffCharacter character)
    {
        character.Money += MoneyValue;
        character.MoneyCountText.text = character.Money.ToString();
        Destroy(gameObject);
    }
}
