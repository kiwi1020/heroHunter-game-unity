using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HorizontalLoop_UI : MonoBehaviour
{
    [SerializeField] Collider2D[] moveObj;
    [SerializeField] float speed;
    [SerializeField] Vector3 returnPosition, moveToPosition;

    private void Update()
    {
        MoveObj();
        CheckPosition();
    }

    void MoveObj()
    {
        moveObj[0].transform.Translate(Vector3.left * speed);
        moveObj[1].transform.Translate(Vector3.left * speed);
    }

    void CheckPosition()
    {
        if(moveObj[0].transform.position.x < returnPosition.x)
        {
            moveObj[1].transform.position = moveToPosition;

            var tmpObj = moveObj[0];
            moveObj[0] = moveObj[1];
            moveObj[1] = tmpObj;
        }
    }
}
