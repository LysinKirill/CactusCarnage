using UnityEngine;

namespace Core
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private int coins = 0;
        [SerializeField] private int hp = 0;
        [SerializeField] private int maxHp = 0;
        public void AddCoins(int amount)
        {
            coins += amount;
        }
        
        public void AddHealth(int amount)
        {
            hp = Mathf.Min(hp + amount, maxHp);
        }

        public int GetCoins()
        {
            return coins;
        }

        public int GetHealthPoints()
        {
            return hp;
        }
    }
}
