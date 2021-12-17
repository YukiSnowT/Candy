using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    const int MaxShotPower = 5;
    const int RecoverySeconds = 3;

    int shotPower = MaxShotPower;
    AudioSource shotSound;

    public GameObject[] candyPrefabs; //candyプレハブのプロパティの配列→インスペクターにも複数登録可能
    public Transform candyParentTransform;
    public CandyManager candyManager;
    public float shotForce;
    public float shotTorque;
    public float baseWidth;
    public float baseHeight;

    void Start()
    {
        shotSound = GetComponent<AudioSource>();
    }

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
        if(shotPower <= 0) return; //現在のshotPowerが0以下なら投入キャンセル

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
        float y = baseHeight * (Input.mousePosition.y / Screen.height) - (baseHeight / 2);
        shotForce = 560.0f + Mathf.Round(y)*50.0f;
        candyRigidbody.AddForce(transform.forward * shotForce);
        candyRigidbody.AddTorque(new Vector3(0, shotTorque, shotTorque));

        //Candyのストックを消費
        candyManager.ConsumeCandy();
        //ShotPowerを消費
        ConsumePower();

        //サウンドを再生
        shotSound.Play();
    }

    void OnGUI()
    {
        GUI.color = Color.black;

        //ShotPowerの数を■の数で表示
        string label = "";
        for(int i=0;i<shotPower;i++) label = label + "■";

        GUI.matrix = Matrix4x4.Scale(Vector3.one * 5);
        GUI.Label(new Rect(50,65,200,100),label);
    }

    void ConsumePower()
    {
        //ShotPowerを消費すると同時に回復のカウントをスタート
        shotPower--;
        StartCoroutine(RecoveryPower());
    }

    IEnumerator RecoveryPower()
    {
        //一定秒数待った後にshotPowerを回復
        yield return new WaitForSeconds(RecoverySeconds);
        shotPower++;
    }
}
