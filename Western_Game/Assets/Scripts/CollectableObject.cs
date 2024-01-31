using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    public void Collect(SheriffCharacter character)
    {
        character.AddDynamite();
        Destroy(gameObject);
    }
}
