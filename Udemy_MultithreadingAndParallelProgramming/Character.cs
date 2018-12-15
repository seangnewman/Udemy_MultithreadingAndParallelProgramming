using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Udemy_MultithreadingAndParallelProgramming
{
    class Character
    {

        private int _health = 100;

        public int Health
        {
            get => _health;
            private set => _health = value;
        } 


        private int _armor;

        public int Armor
        {
            get => _armor;
            private set => _armor = value;
        }


        public void Hit(int damage)
        {
            //Health -= -damage - Armor;
           int actualDamage =  Interlocked.Add(ref damage, -Armor);
            Interlocked.Add(ref _health,  -actualDamage);

        }

        public void Heal(int health)
        {
           // Health += health;
            Interlocked.Add(ref _health, health);
        }

        public void CastArmorSpell(bool isPositive)
        {
            if (isPositive)
            {
                Interlocked.Increment(ref _armor);
                //Armor++;
            }
            else
            {
                Interlocked.Decrement(ref _armor);
                //Armor--;
            }
        }
    }
}
