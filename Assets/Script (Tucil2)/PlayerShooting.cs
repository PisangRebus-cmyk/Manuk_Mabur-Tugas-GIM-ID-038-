using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float fireRate = 0.25f;     // seconds between shots
    [SerializeField] private float spawnOffset = 0.6f;   // distance in front of the player

    private PlayerMovement movement;
    private float nextFireTime;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (WantsToShoot() && Time.time >= nextFireTime)
        {
            AudioManager.Play(Sfx.Shoot);
            Shoot();
        }
            
    }

    private bool WantsToShoot()
    {
        var kb = Keyboard.current;
        var mouse = Mouse.current;

        bool keyPressed = kb != null && (kb.jKey.isPressed || kb.fKey.isPressed);
        bool clicked = mouse != null && mouse.leftButton.isPressed;
        return keyPressed || clicked;
    }

    private void Shoot()
    {
        

        nextFireTime = Time.time + fireRate;

        Vector2 direction = new Vector2(movement.FacingDirection, 0f);
        Vector2 spawnPos = (Vector2)transform.position + direction * spawnOffset;

        Bullet bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        bullet.Launch(direction, gameObject.tag);
    }
}