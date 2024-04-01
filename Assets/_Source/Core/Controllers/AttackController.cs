using Core.Enemies;
using ScriptableObjects.Items;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.Controllers
{
    public class AttackController : MonoBehaviour
    {
        [SerializeField] private GameObject aimBox;
        [SerializeField] private float baseDamage;
        [SerializeField] private float baseReachDistance;
        [SerializeField] private float baseAttackCooldown;
        [SerializeField] private LayerMask enemyLayerMask;
        [SerializeField] private GameObject inventory;
        
        private bool _canAttack = true;
        private WeaponController _weaponController;
        private bool _rangedAttackReady;
        private Coroutine _preparationCoroutine;
        private void Awake()
        {
            _weaponController = GetComponent<WeaponController>();
        }

        private void Start()
        {
            aimBox.SetActive(false);
        }

        private void Update()
        {
            if (inventory.activeInHierarchy)
                return;
            
            if (Input.GetMouseButtonDown((int)MouseButton.RightMouse))
                HandleRightMouseButtonDown();
            
            if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
                HandleLeftMouseButtonDown();
            
            if(Input.GetMouseButtonUp((int)MouseButton.RightMouse))
                HandleRightMouseButtonUp();
                
        }

        private void HandleRightMouseButtonUp()
        {
            CancelWeaponPreparation();
        }

        private void CancelWeaponPreparation()
        {
            if(_preparationCoroutine == null)
                return;
            Debug.Log("Cancel weapon preparation");
            StopCoroutine(_preparationCoroutine);
            _rangedAttackReady = false;
            aimBox.SetActive(false);
            _preparationCoroutine = null;
        }

        private void HandleRightMouseButtonDown()
        {
            var currentWeapon = _weaponController.CurrentWeapon;

            var rangedWeapon = currentWeapon as RangedWeaponAsset;
            if (rangedWeapon == null)
                return;

            _preparationCoroutine = StartCoroutine(PrepareWeapon(rangedWeapon.PreparationTime));
        }



        private void HandleLeftMouseButtonDown()
        {
            if (!_canAttack)
                return;
            Attack();
        }


        private void Attack()
        {
            Debug.Log("Attack");
            var currentWeapon = _weaponController.CurrentWeapon;

            if (currentWeapon == null)
            {
                BareHandedAttack(baseDamage);
                return;
            }
            
            if (currentWeapon is MeleeWeaponAsset meleeWeapon)
            {
                AttackMelee(meleeWeapon);
                return;
            }

            if (currentWeapon is RangedWeaponAsset rangedWeapon)
            {
                AttackRanged(rangedWeapon);
            }
        }

        private void AttackRanged(RangedWeaponAsset rangedWeapon)
        {
            Debug.Log("Ranged attack");
            if (!_rangedAttackReady || !aimBox.activeInHierarchy)
                return;
            
            var projectile = Instantiate(rangedWeapon.ProjectilePrefab);
            if (projectile.TryGetComponent(out Rigidbody2D projectileBody))
            {
                var shootDirection = aimBox.transform.position - transform.position;
                projectileBody.velocity = shootDirection.normalized * rangedWeapon.ProjectileSpeed;
            }
            _weaponController.AddProjectile(projectile);
            StartCoroutine(AttackCooldown(rangedWeapon.AttackDelay));
        }

        private void AttackMelee(MeleeWeaponAsset weapon)
        {
            Debug.Log("Melee attack");
            if (IsEnemyInAttackBox(weapon.AttackRange, out GameObject enemy))
            {
                if(enemy.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    enemyHealth.TakeDamage(weapon.Damage);
                    Debug.Log($"Deal {weapon.Damage} damage to enemy");
                }
                //throw new NotImplementedException();
                // enemy.TakeDamage(weapon.Damage);
            }

            StartCoroutine(AttackCooldown(weapon.AttackDelay));
        }
        
        private void BareHandedAttack(float damage)
        {
            Debug.Log("Bare-handed attack");
            if (IsEnemyInAttackBox(baseReachDistance, out GameObject enemy))
            {
                if(enemy.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    enemyHealth.TakeDamage(baseDamage);
                    Debug.Log($"Deal {baseDamage} damage to enemy with hands");
                }
            }

            StartCoroutine(AttackCooldown(baseAttackCooldown));
        }

        private bool IsEnemyInAttackBox(float attackBoxSize, out GameObject enemy)
        {
            enemy = null;
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            var center = new Vector2(transform.position.x, transform.position.y);
            var attackBox = new Vector3(attackBoxSize, attackBoxSize, attackBoxSize);
            
            
            RaycastHit2D hit =
                Physics2D.BoxCast(center, attackBox, 0, direction, 0, enemyLayerMask);
            if (hit.collider == null)
                return false;

            enemy = hit.collider.gameObject;
            return true;
        }

        private void OnDrawGizmos()
        {
            if(_weaponController == null)
                return;
            var weapon = _weaponController.CurrentWeapon;
            if (weapon == null)
                return;
            
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            var center = new Vector2(transform.position.x, transform.position.y);
            
            
            
            
            if(weapon is MeleeWeaponAsset meleeWeapon)
            {
                var attackBoxSize = meleeWeapon.AttackRange;
                var attackBox = new Vector3(attackBoxSize, attackBoxSize, attackBoxSize);
                
                Gizmos.DrawWireCube(center, attackBox);
            }
            
        }

        private IEnumerator PrepareWeapon(float preparationTime)
        {
            Debug.Log("Start preparing weapon");
            _rangedAttackReady = false;
            yield return new WaitForSecondsRealtime(preparationTime);
            // Play preparation animation
            _rangedAttackReady = true;
        }

        private IEnumerator AttackCooldown(float cooldownTime)
        {
            _canAttack = false;
            yield return new WaitForSecondsRealtime(cooldownTime);
            _canAttack = true;
        }
    }
}
