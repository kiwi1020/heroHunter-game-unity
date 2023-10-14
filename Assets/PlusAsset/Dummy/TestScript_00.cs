using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // 두트윈 쓰려면 임포트 해야함

public class TestScript_00 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Transform>().DOMoveX(transform.position.x + 4, 3); /// <<<< 두트윈 함수 사용 예시
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
