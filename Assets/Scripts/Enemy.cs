using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextRPG;

public class Enemy : Character {

  

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        Debug.Log("this only happens but only on enemy! not onother characters");
    }

    public override void Die()
    {
        //base.Die();
        Debug.Log("Character died, was enemy");
    }

}
