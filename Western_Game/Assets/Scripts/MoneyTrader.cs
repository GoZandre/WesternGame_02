using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyTrader : CollectableObject
{
    public float moneyNeeded;

    public UnityEvent NotEnoughMoney = new UnityEvent();
    public UnityEvent HasEnoughMoney = new UnityEvent();

    public bool DoOnce;

    public override void Collect(SheriffCharacter character)
    {
        if(character.Money >= moneyNeeded)
        {
            character.Money -= moneyNeeded;

            HasEnoughMoney.Invoke();

            if (DoOnce)
            {
                this.enabled = false;
            }
        }
        else
        {
            NotEnoughMoney.Invoke();
        }
    }
}
