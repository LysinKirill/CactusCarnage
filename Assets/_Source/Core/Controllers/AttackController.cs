using Core.Enemies;
using Core.Player;
using ScriptableObjects.Items;
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
        private PlayerState _playerState;
        
        
        private bool _canAttack = true;
        private WeaponController _weaponController;
        private bool _rangedAttackReady;
        private Coroutine _preparationCoroutine;
        private void Awake()
        {
            _weaponController = GetComponent<WeaponController>();
            _playerState = GetComponent<PlayerState>();
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

            if (aimBox.activeInHierarchy)
                AimBoxFollowMouse();
        }

        private void AimBoxFollowMouse()
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newAimBoxPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, aimBox.transform.position.z);
            aimBox.transform.position = newAimBoxPosition;
        }

        private void HandleRightMouseButtonUp()
        {
            aimBox.SetActive(false);
            CancelWeaponPreparation();
        }

        private void CancelWeaponPreparation()
        {
            if(_preparationCoroutine == null)
                return;
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
            
            aimBox.SetActive(true);
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
            var currentWeapon = _weaponController.CurrentWeapon;

            if (currentWeapon == null)
            {
                BareHandedAttack();
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
            if (!_rangedAttackReady || !aimBox.activeInHierarchy)
                return;
            
            var projectile = Instantiate(rangedWeapon.ProjectilePrefab, transform.position, Quaternion.identity);
            if (projectile.TryGetComponent(out Projectile proj))
                proj.SetDamage(rangedWeapon.ProjectileDamage);

            if (projectile.TryGetComponent(out Rigidbody2D projectileBody))
            {
                var shootDirection = aimBox.transform.position - transform.position;
                projectileBody.velocity = shootDirection.normalized * rangedWeapon.ProjectileSpeed;
                
                float angle = Mathf.Atan2(projectileBody.velocity.y, projectileBody.velocity.x) * Mathf.Rad2Deg;
                projectileBody.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            
            StartCoroutine(AttackCooldown(rangedWeapon.AttackDelay));
        }

        private void AttackMelee(MeleeWeaponAsset weapon)
        {
            if (IsEnemyInAttackBox(weapon.AttackRange, out GameObject enemy))
                if(enemy.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    enemyHealth.TakeDamage(weapon.Damage);
                    _playerState.AddUltimateProgress(_playerState.UltimateGainOnDealDamage);
                }

            StartCoroutine(AttackCooldown(weapon.AttackDelay));
        }
        
        private void BareHandedAttack()
        {
            if (IsEnemyInAttackBox(baseReachDistance, out GameObject enemy))
                if(enemy.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    enemyHealth.TakeDamage(baseDamage);
                    _playerState.AddUltimateProgress(_playerState.UltimateGainOnDealDamage);
                }

            StartCoroutine(AttackCooldown(baseAttackCooldown));
        }

        private bool IsEnemyInAttackBox(float attackBoxSize, out GameObject enemy)
        {
            enemy = null;
            Vector3 direction = transform.localScale.x > 0 ? Vector3.right : Vector3.left;
            var center = transform.position + direction * attackBoxSize / 2f;
            var attackBox = new Vector3(attackBoxSize, transform.localScale.y, attackBoxSize);
            
            RaycastHit2D[] hits =
                Physics2D.BoxCastAll(center, attackBox, 0f, Vector2.zero, 0f, enemyLayerMask);
            
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.TryGetComponent(out EnemyHealth _))
                {
                    enemy = hit.collider.gameObject;
                    return true;
                }
            }
            
            return false;
        }

        private void OnDrawGizmos()
        {
            if(_weaponController == null)
                return;
            var weapon = _weaponController.CurrentWeapon;
            if (weapon == null)
                return;
            
            Vector3 direction = transform.localScale.x > 0 ? Vector3.right : Vector3.left;
            if(weapon is MeleeWeaponAsset meleeWeapon)
            {
                var attackBoxSize = meleeWeapon.AttackRange;
                var center = transform.position + direction * attackBoxSize / 2f;
                var attackBox = new Vector3(attackBoxSize, transform.localScale.y, attackBoxSize);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(center, attackBox);
            }
        }

        private IEnumerator PrepareWeapon(float preparationTime)
        {
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
