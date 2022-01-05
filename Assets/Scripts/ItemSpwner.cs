using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemSpwner : MonoBehaviour
{
    public static ItemSpwner inst;
    public Transform itemParent;
    [Space(5)]
    public Item[] itemPrefabs;
    bool canSpwan;
    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
    }

    public void StartSpwan()
    {
        canSpwan = true;
        StartCoroutine(SpwanItems());
    }

    internal IEnumerator SpwanItems()
    {
        while (canSpwan && UIManager.inst.gameStarted)
        {
            Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length)], GetPosition(), Quaternion.identity, itemParent);
            yield return new WaitForSeconds(1f);
        }
    }


    public void RemoveAllItems()
    {
        canSpwan = false;
        StopCoroutine(SpwanItems());
        itemParent.ClearChildren();
    }

    Vector3 lastPos;
    public Vector3 GetPosition()
    {
        DR:
        Vector3 position = new Vector3(Random.Range(-2.9f, 2.9f), Random.Range(5.5f, 6.3f), 0f);
        if(Vector3.Distance(lastPos, position) < 1.5f)
        {
            goto DR;
        }
        lastPos = position;
        return lastPos;
    }

}
