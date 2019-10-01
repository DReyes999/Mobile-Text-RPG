using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextRPG
{
    /* Implementing an interface.
     * Here we define the interface "Baddie"
     * All things that are an enemy will inherit the characteristics of baddie
     */

    // Make the interface public if we are to access it outside of this class
    public interface IBaddie
    {
        /* Here we define things that must be required by anything
         * that implements IBaddie. For example anything that has the IBaddie
         * interface attached to it must also have a battlecry method
         */
        void Cry();
        string Description { get; set; }
    }
    public class Enemy : Character, IBaddie
    {
        // Since IBaddie contains Description, this is where we make it agree
        public string Description { get; set; }

        public override void TakeDamage(int amount)
        {
            /* By using "base" we are implementing the original method found on the Character class
             * And then adding to it special things just for Enemy
             */
            Debug.Log(string.Format("Enemy takes {0} damage", amount));
            base.TakeDamage(amount);
            UIController.OnEnemyUpdate(this);
            if (Energy <= 0)
            {
                //Debug.Log("Energy was <= 0, calling Die()");
                this.Energy = 0;
                Die();
            }
        }

        // Here we validate the IBaddie interface and make it agree by implementing Cry()
        public void Cry()
        {

        }

        public override void Die()
        {
            //Debug.Log("Enemy died. Calling encounter.OnenemyDie");
            Encounter.OnEnemyDie();
            //Energy = MaxEnergy;
           
        }

    }
}
