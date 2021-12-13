using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    Vector3 startPosithion;

    public float amplitude; //移動量プロパティ
    public float speed; //移動速度プロパティ

    // Start is called before the first frame update
    void Start()
    {
        startPosithion = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //変位を計算
        float z = amplitude * Mathf.Sin(Time.time * speed);

        transform.localPosition = startPosithion + new Vector3(0,0,z);
    }
}
