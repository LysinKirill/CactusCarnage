using UnityEngine;

namespace Core
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private int hp = 0;
        [SerializeField] private int maxHp = 0;

        public void AddHealth(int amount)
        {
            hp = Mathf.Min(hp + amount, maxHp);
        }

        public int GetHealthPoints()
        {
            return hp;
        }
    }
}
