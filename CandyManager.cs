using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyManager : MonoBehaviour
{
    const int DefaultCandyAmount = 30;
    const int RecoverySeconds = 10;

    //現在のストック
    public int candy = DefaultCandyAmount;

    //回復までの残り秒数
    int counter;

    public void ConsumeCandy()
    {
        if(candy>0) candy--;
    }

    public int GetCandyAmount()
    {
        return candy;
    }
    
    public void AddCandy(int amount)
    {
        candy += amount;
    }

    void OnGUI()
    {
        GUI.color = Color.black;

        //ストック数を表示
        string label = "Candy : " + candy;

        //カウントしている時だけ秒数を表示
        if(counter > 0) label = label + " (回復まで残り" + counter + "秒)";

        GUI.matrix = Matrix4x4.Scale(Vector3.one * 5); 
        GUI.Label(new Rect(50,50,200,100),label);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ストックがデフォルトより少なくカウントしていない時にカウントスタート
        if(candy < DefaultCandyAmount && counter <= 0)
        {
            StartCoroutine(RecoverCandy());
        }
    }

    IEnumerator RecoverCandy()
    {
        counter = RecoverySeconds;

        //1秒ずつカウントを進める
        while(counter>0)
        {
            yield return new WaitForSeconds(1.0f);
            counter--;
        }
        candy++;
    }
}
