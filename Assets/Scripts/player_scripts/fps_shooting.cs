using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class fps_shooting : MonoBehaviour
{
    public enum WeaponType {ashe_normal, ashe_q, ashe_w, ashe_e, ashe_r, pistol, machineGun, shotgun }
    public WeaponType current_weapon;

    public GameObject bullet_prefab_ashe_normal;
    public GameObject bullet_prefab_ashe_Q;
    public GameObject bullet_prefab_ashe_W;
    public GameObject bullet_prefab_ashe_E;
    public GameObject bullet_prefab_ashe_R;
    public GameObject bullet_prefab_pistol;
    public GameObject bullet_prefab_machinegun;
    public GameObject bullet_prefab_shotgun;

    public Transform fire_point;
    public Transform[] fire_points;
    private float next_fire_rate;

    public float recoil_amount = 1.5f;
    public float recoil_duration = 0.1f;

    private int q_counter;

    
    // Q Ability helpers
    public TextMeshProUGUI q_timer;
    private float q_cooldown;
    private bool q_available;

    // W Ability helpers
    public TextMeshProUGUI w_timer;
    private float w_cooldown;
    private bool w_available;

    // E Ability helpers
    public TextMeshProUGUI e_timer;
    private float e_cooldown;
    private bool e_available;

    // R Ability helpers
    public TextMeshProUGUI r_timer;
    private float r_cooldown;
    private bool r_available;

    private void Start()
    {
        current_weapon = WeaponType.ashe_normal;
        next_fire_rate = 0.0f;

        recoil_amount = 1.5f;
        recoil_duration = 0.1f;

        // Initialize Abilities
        q_available = true;
        w_available = true;
        e_available = true;
        r_available = true;
    }

    private void Update()
    {
        get_weapon_change();
        shoot();
        update_timers();
    }

    private void update_timers()
    {
        if (!q_available)
        {
            if(q_cooldown > 1)
            {
                q_cooldown -= Time.deltaTime;
                actualizar_timer("q");
            }
            else
            {
                q_available = true;
            }
        }
        if (!w_available)
        {
            if (w_cooldown > 1)
            {
                w_cooldown -= Time.deltaTime;
                actualizar_timer("w");
            }
            else
            {
                w_available = true;
            }
        }
        if (!e_available)
        {
            if (e_cooldown > 1)
            {
                e_cooldown -= Time.deltaTime;
                actualizar_timer("e");
            }
            else
            {
                e_available = true;
            }
        }

        if (!r_available)
        {
            if (r_cooldown > 1)
            {
                r_cooldown -= Time.deltaTime;
                actualizar_timer("r");
            }
            else
            {
                r_available = true;
            }
        }
    }

    public void shoot()
    {
        if (Input.GetButton("Fire1") && Time.time >= next_fire_rate)
        {
            basic_weapon_shoot();
            next_fire_rate = Time.time + 1f / get_actual_fire_rate();
        }
        if (current_weapon == WeaponType.ashe_normal)
        {
            // Ashe's Q
            if (Input.GetKeyDown(KeyCode.Q))
            {
                q_shoot();
                next_fire_rate = Time.time + 1f / get_actual_fire_rate();
            }
            // Ashe's W
            else if (Input.GetKeyDown(KeyCode.W))
            {
                w_shoot();
                next_fire_rate = Time.time + 1f / get_actual_fire_rate();
            }
            // Ashe's E
            else if (Input.GetKeyDown(KeyCode.E))
            {
                e_shoot();
                next_fire_rate = Time.time + 1f / get_actual_fire_rate();
            }
            // Ashe's R
            else if (Input.GetKeyDown(KeyCode.R))
            {
                r_shoot();
                next_fire_rate = Time.time + 1f / get_actual_fire_rate();
            }
        }
    }

    private void basic_weapon_shoot()
    {
        int num_bullets = get_actual_number_bullets();

        for (int i = 0; i < num_bullets; i++)
        {
            GameObject bullet_prefab = get_actual_bullet_prefab();

            if (num_bullets == 1)
            {
                GameObject bullet = Instantiate(bullet_prefab, fire_point.position, fire_point.rotation);
                Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();

                bullet_physics(bulletRB);
            }

            else
            {
                int randomFirePointIndex = Random.Range(0, fire_points.Length);
                Transform selectedFirePoint = fire_points[randomFirePointIndex];

                GameObject bullet = Instantiate(bullet_prefab, selectedFirePoint.position, selectedFirePoint.rotation);
                Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();

                bullet_physics(bulletRB);
            }
        }
    }

    private void q_shoot()
    {
        if (q_logic())
        {
            current_weapon = WeaponType.ashe_q;
            int num_bullets = get_actual_number_bullets();
            for (int i = num_bullets; i > 0; i--)
            {
                q_counter = i;
                Invoke("shoot_wait", i * 0.1f);
            }
            current_weapon = WeaponType.ashe_normal;
        }
    }

    private void w_shoot()
    {
        if (w_logic())
        {
            current_weapon = WeaponType.ashe_w;
            multiple_angluar_shoot(0.7f);
            multiple_angluar_shoot(-0.7f);
            multiple_angluar_shoot(0.3f);
            multiple_angluar_shoot(-0.3f);
            multiple_angluar_shoot(0.0f);
            current_weapon = WeaponType.ashe_normal;
        }
    }
    private void e_shoot()
    {
        if (e_logic())
        {
            current_weapon = WeaponType.ashe_e;
            int num_bullets = get_actual_number_bullets();
            shoot_wait();
            current_weapon = WeaponType.ashe_normal;
        }
    }
    private void r_shoot()
    {
        if (r_logic())
        {
            current_weapon = WeaponType.ashe_r;
            int num_bullets = get_actual_number_bullets();
            shoot_wait();
            current_weapon = WeaponType.ashe_normal;
        }
    }

    private bool q_logic()
    {
        if (q_available)
        {
            q_cooldown = 8.0f;
            q_available = false;
            return true;
        }
        return false;
    }

    private bool w_logic()
    {
        if (w_available)
        {
            w_cooldown = 3.0f;
            w_available = false;
            return true;
        }
        return false;
    }

    private bool e_logic()
    {
        if (e_available)
        {
            e_cooldown = 32.0f;
            e_available = false;
            return true;
        }
        return false;
    }

    private bool r_logic()
    {
        if (r_available)
        {
            r_cooldown = 19.0f;
            r_available = false;
            return true;
        }
        return false;
    }

    public void actualizar_timer(string habilidad)
    {
        float segundos;
        switch (habilidad)
        {
            case "q":
                segundos = Mathf.FloorToInt(q_cooldown % 60);
                q_timer.SetText(string.Format("{0:00}", segundos));
                break;
            case "w":
                segundos = Mathf.FloorToInt(w_cooldown % 60);
                w_timer.SetText(string.Format("{0:00}", segundos));
                break;
            case "e":
                segundos = Mathf.FloorToInt(e_cooldown % 60);
                e_timer.SetText(string.Format("{0:00}", segundos));
                break;
            case "r":
                segundos = Mathf.FloorToInt(r_cooldown % 60);
                r_timer.SetText(string.Format("{0:00}", segundos));
                break;
        }

    }

    private void shoot_wait()
    {
        single_linear_shoot();
    }

    private void single_linear_shoot()
    {
        GameObject bullet_prefab = get_actual_bullet_prefab();
        GameObject bullet = Instantiate(bullet_prefab, fire_point.position, fire_point.rotation);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();

        bullet_physics(bulletRB);
    }

    private void multiple_angluar_shoot(float angle)
    {
        GameObject bullet_prefab = get_actual_bullet_prefab();
        GameObject bullet = Instantiate(bullet_prefab, fire_point.position, fire_point.rotation);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();

        rotated_bullet_physics(bulletRB, angle);
    }

    public void rotated_bullet_physics(Rigidbody rb, float angle)
    {
        if (rb != null)
        {
            float bullet_speed = get_actual_bullet_speed();
            //rb.velocity = apply_gravity(bullet_speed * (fire_point.forward + new Vector3(angle, angle, angle)), Time.deltaTime);
            rb.velocity = (bullet_speed * fire_point.forward);
            //rb.rotation = Quaternion.AngleAxis(-45, Vector3.left);
        }
    }

    public void bullet_physics(Rigidbody rb)
    {
        if (rb != null)
        {
            float bullet_speed = get_actual_bullet_speed();
            rb.velocity = apply_gravity(fire_point.forward * bullet_speed, Time.deltaTime);
        }
    }

    private Vector3 apply_gravity(Vector3 velocity, float time)
    {
        float gravity = Physics.gravity.y;
        velocity.y += gravity * time;

        return velocity;
    }

    public void get_weapon_change()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            current_weapon = WeaponType.ashe_normal;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            current_weapon = WeaponType.pistol;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            current_weapon = WeaponType.machineGun;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            current_weapon = WeaponType.shotgun;
        }
    }


    private GameObject get_actual_bullet_prefab()
    {
        switch (current_weapon)
        {
            case WeaponType.ashe_normal:
                return bullet_prefab_ashe_normal;

            case WeaponType.ashe_q:
                return bullet_prefab_ashe_Q;

            case WeaponType.ashe_w:
                return bullet_prefab_ashe_W;

            case WeaponType.ashe_e:
                return bullet_prefab_ashe_E;

            case WeaponType.ashe_r:
                return bullet_prefab_ashe_R;

            case WeaponType.pistol:
                return bullet_prefab_pistol;

            case WeaponType.machineGun:
                return bullet_prefab_machinegun;

            case WeaponType.shotgun:
                return bullet_prefab_shotgun;

            default:
                return null;
        }
    }

    private float get_actual_fire_rate()
    {
        switch (current_weapon)
        {
            case WeaponType.ashe_normal:
                return 1.0f;

            case WeaponType.ashe_q:
                return 2.0f;

            case WeaponType.ashe_w:
                return 0.3f;

            case WeaponType.ashe_e:
                return 0.1f;

            case WeaponType.ashe_r:
                return 0.1f;

            case WeaponType.pistol:
                return 1.0f;

            case WeaponType.machineGun:
                return 5.0f;

            case WeaponType.shotgun:
                return 0.7f;

            default:
                return 0.2f;
        }
    }

    private int get_actual_number_bullets()
    {
        switch (current_weapon)
        {
            case WeaponType.ashe_normal:
                return 1;

            case WeaponType.ashe_q:
                return 4;

            case WeaponType.ashe_w:
                return 5;

            case WeaponType.ashe_e:
                return 1;

            case WeaponType.ashe_r:
                return 1;

            case WeaponType.pistol:
                return 1;

            case WeaponType.machineGun:
                return 1;

            case WeaponType.shotgun:
                return 7;

            default:
                return 1;
        }
    }

    private float get_actual_bullet_speed()
    {
        switch (current_weapon)
        {
            case WeaponType.ashe_normal:
                return 50.0f;

            case WeaponType.ashe_q:
                return 75.0f;

            case WeaponType.ashe_w:
                return 30.0f;

            case WeaponType.ashe_e:
                return 30.0f;

            case WeaponType.ashe_r:
                return 40.0f;

            case WeaponType.pistol:
                return 50.0f;

            case WeaponType.machineGun:
                return 85.0f;

            case WeaponType.shotgun:
                return 30.0f;

            default:
                return 30.0f;
        }
    }
}
