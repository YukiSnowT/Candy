using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject[] candyPrefabs; //candyプレハブのプロパティの配列→インスペクターにも複数登録可能
    public Transform candyParentTransform;
    public CandyManager candyManager;
    public float shotForce;
    public float shotTorque;
    public float baseWidth;

    void Update()
    {
        if(Input.GetButtonDown("Fire1")) Shot();
    }

    //キャンディのプレハブからランダムに1つ選ぶ
    GameObject SampleCandy()
    {
        int index = Random.Range(0, candyPrefabs.Length);
        return candyPrefabs[index];
    }

    Vector3 GetInstantiatePosition() //位置の計算
    {
        //画面のサイズとInputの割合からキャンディ生成
        //Input.mousePosition.xでタップ位置、Screen.widthで横幅を取得
        float x = baseWidth * (Input.mousePosition.x / Screen.width) - (baseWidth / 2);
        return transform.position + new Vector3(x,0,0);
    }

    public void Shot()
    {
        //キャンディを生成できないならShotしない
        if(candyManager.GetCandyAmount() <= 0) return;

        //プレハブからオブジェクトを生成
        GameObject candy = (GameObject)Instantiate(
            SampleCandy(), //candyプレハブからランダム生成
            GetInstantiatePosition(),//位置
            Quaternion.identity //向き(回転なし)
        );

        //生成したCandyオブジェクトの親をcandyParentTransformに設定
        candy.transform.parent = candyParentTransform; //親オブジェクトを設定

        //CandyオブジェクトのRigidbodyに力と回転を加える
        Rigidbody candyRigidbody = candy.GetComponent<Rigidbody>();
        candyRigidbody.AddForce(transform.forward * shotForce);
        candyRigidbody.AddTorque(new Vector3(0, shotTorque, shotTorque));

        //Candyのストックを消費
        candyManager.ConsumeCandy();
    }
}
