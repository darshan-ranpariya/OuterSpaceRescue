using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public float score;
    [Space(5)]
    public GameObject effect;
    [Space(5)]
    [Space(5)]
    [Range(0f, 3f)]
    public float speed;
    Vector3 pos;

    private void Update()
    {
        pos = transform.position;
        pos.y -= speed * Time.deltaTime;
        transform.position = pos;
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -5.5f)
        {
            Destroy(gameObject);
        }
    }

    public void OnPlayerTouched()
    {
        UIManager.inst.UpdateScore(this);
        Instantiate(effect, transform.position, Quaternion.identity, transform.parent.parent);
        AudioPlayer.PlaySFX(itemType == ItemType.Enemy ? "Enemy" : "Collect");
        Destroy(gameObject);
    }
}

public enum ItemType
{
    Friendly,
    Energy,
    Enemy
}
