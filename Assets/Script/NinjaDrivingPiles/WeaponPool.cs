using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPool : MonoBehaviour
{
    public static WeaponPool instance;
    [SerializeField] int poolSize = 10;
    [SerializeField] GameObject[] weaponPrefabs;
    Queue<GameObject> pool = new Queue<GameObject>();
    float pullTimer;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject weapon = Instantiate(weaponPrefabs[Random.Range(0, weaponPrefabs.Length)], transform);
            weapon.SetActive(false);
            pool.Enqueue(weapon);
        }
        pullTimer = Random.Range(1f, 5f);
        StartCoroutine(Pull(pullTimer));
    }

    void PullWeapon()
    {
        GameObject weapon = null;
        if (pool.Count > 0)
        {
            weapon = pool.Dequeue();
        }
        else
        {
            weapon = Instantiate(weaponPrefabs[Random.Range(0, weaponPrefabs.Length)], transform);
        }

        weapon.SetActive(true);

        //随机屏幕两边位置
        float posX = Random.Range(0, 2) == 1 ? Screen.width * 0.9f : Screen.width * 0.1f;
        float posY = Random.Range(Screen.height * 0.4f, Screen.height * 0.9f);
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(posX, posY, 0));
        pos.z = 0;
        weapon.transform.position = pos;
        float forceX = posX < Screen.width * 0.5f ? 10f : -10f;
        //float forceY = Random.Range(10f, 20f);
        weapon.GetComponent<Rigidbody2D>().velocity = new Vector2(forceX, 0);

        
    }
    public void RecycleToPool(GameObject weapon)
    {
        weapon.SetActive(false);
        pool.Enqueue(weapon);
    }

    IEnumerator Pull(float _pullTimer)
    {
        yield return new WaitForSeconds(_pullTimer);
        pullTimer = Random.Range(1f, 5f);
        PullWeapon();
        StartCoroutine(Pull(pullTimer));
    }
}
