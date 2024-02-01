using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Static_ObjectPooling {

    public static void do_ObjectPooling(Weapon weapon,GameObject prefab,Transform bulletHierarchy,Transform bulletDirection,Queue<GameObject> bullets)
    {
        if (bullets.Count <= weapon.FullAmmo)
        {
            GameObject bullet = GameObject.Instantiate(prefab, bulletDirection);
            bullet.GetComponent<BulletScript>().damage = (int)weapon.Damage;
            bullet.transform.parent = bulletHierarchy; 
            bullets.Enqueue(bullet); 
        }
        else
        {
            GameObject bullet = bullets.Peek();
            if (!bullet.active)
            {
                bullet.transform.rotation = bulletDirection.rotation;
                bullet.transform.position = bulletDirection.position;
                bullet.GetComponent<BulletScript>().ResetVelo();
                bullet.SetActive(true);
            }
            else
            {
                do
                {
                    GameObject _oldBullet = bullets.Dequeue();
                    bullet = bullets.Peek();

                    _oldBullet.gameObject.GetComponent<BulletScript>().ReCall();
                    bullets.Enqueue(_oldBullet);

                    if (bullet.gameObject.active == false)
                    {
                        bullet.transform.rotation = bulletDirection.rotation;
                        bullet.transform.position = bulletDirection.position;
                        bullet.transform.GetComponent<BulletScript>().ResetVelo();
                        bullet.gameObject.SetActive(true);
                        break;
                    }
                }
                while (bullet.gameObject.active == true);
            }
        }
    }
    public static void do_ObjectPoolingEnemy(GameObject prefab, Transform bulletHierarchy, Transform bulletDirection, Queue<GameObject> bullets)
    {
        if (bullets.Count <= 30)
        {
            GameObject bullet = GameObject.Instantiate(prefab, bulletDirection);
            bullet.transform.parent = bulletHierarchy;
            bullets.Enqueue(bullet);
        }
        else
        {
            GameObject bullet = bullets.Peek();
            if (!bullet.active)
            {
                bullet.transform.rotation = bulletDirection.rotation;
                bullet.transform.position = bulletDirection.position;
                bullet.GetComponent<BulletScript>().ResetVelo();
                bullet.SetActive(true);
            }
            else
            {
                do
                {
                    GameObject _oldBullet = bullets.Dequeue();
                    bullet = bullets.Peek();

                    _oldBullet.gameObject.GetComponent<BulletScript>().ReCall();
                    bullets.Enqueue(_oldBullet);

                    if (bullet.gameObject.active == false)
                    {
                        bullet.transform.rotation = bulletDirection.rotation;
                        bullet.transform.position = bulletDirection.position;
                        bullet.transform.GetComponent<BulletScript>().ResetVelo();
                        bullet.gameObject.SetActive(true);
                        break;
                    }
                }
                while (bullet.gameObject.active == true);
            }
        }
    }


}
