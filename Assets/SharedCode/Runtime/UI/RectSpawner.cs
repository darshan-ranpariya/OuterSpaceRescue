using UnityEngine;
using System.Collections;

public class RectSpawner : MonoBehaviour {
    public RectTransform spawnRectT;
    public OPool prefabsPool;
    public bool StartOnEnable;

    public int maxSpawns;
    public int preWarmSpawns;
    int totalSpawned;

    public int m_burstCountMin;
    int burstCountMin {
        get {
            if (m_burstCountMin < 0) m_burstCountMin = 0;
            return m_burstCountMin;
        }
    }

    public int m_burstCountMax;
    int burstCountMax
    {
        get
        {
            if (m_burstCountMax < burstCountMin) m_burstCountMax = burstCountMin;
            return m_burstCountMax;
        }
    }

    public float m_burstGapMin;
    float burstGapMin {
        get {
            if (m_burstGapMin <= 0) m_burstGapMin = 1;
            return m_burstGapMin;
        }
    }

    public float m_burstGapMax;
    float burstGapMax
    {
        get
        {
            if (m_burstGapMax < burstGapMin) m_burstGapMax = burstGapMin;
            return m_burstGapMax;
        }
    }

    float _x, _y, _z;
    Vector3 randomSpawnPos
    {
        get
        {
            _x = Random.Range(spawnRectT.position.x - spawnRectT.rect.width * spawnRectT.lossyScale.x / 2, spawnRectT.position.x + spawnRectT.rect.width * spawnRectT.lossyScale.x / 2);
            _y = Random.Range(spawnRectT.position.y - spawnRectT.rect.height * spawnRectT.lossyScale.y / 2, spawnRectT.position.y + spawnRectT.rect.height * spawnRectT.lossyScale.y / 2);
            _z = spawnRectT.position.z; 
            return new Vector3(_x, _y, _z); 
        }
    }

    void OnEnable() {
        print(string.Format("RectPos: {0}\nRectWidth: {1}\nRect Height: {2}\n{3}", spawnRectT.position, spawnRectT.rect.width, spawnRectT.rect.height, spawnRectT.localScale));
        prefabsPool.DespawnAll();
        if (StartOnEnable) StartCoroutine("Emission");
    }

    void OnDisable() {
        StopCoroutine("Emission");
    }

    IEnumerator Emission() {
        while (true)
        {
            Burst(); 
            yield return new WaitForSeconds(Random.Range(burstGapMin, burstGapMax));
        } 
    }

    void Burst() {
        int burstCount = Random.Range(burstCountMin, burstCountMax);
        string prefabKey = "";
        for (int i = 0; i < burstCount; i++)
        {
            prefabKey = prefabsPool.objects[Random.Range(0, prefabsPool.objects.Length)].key;
            Transform tx = prefabsPool.Spawn(prefabKey, randomSpawnPos); 
        }
    }
}
