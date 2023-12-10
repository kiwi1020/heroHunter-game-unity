using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBackgroundMove : MonoBehaviour
{

    [SerializeField] GameObject[] tree, shadow;

    void Start()
    {
        RepositionBackground();    
    }

    public void RepositionBackground()
    {
        for(int i = 0; i<20; i++)
        {
            var tmp_tree = Instantiate(tree[Random.Range(0, tree.Length)], transform);
            tmp_tree.transform.position = 
                new Vector3( -7 + Random.Range( -1 , 1f ) + 5f * i ,
                1 + Random.Range( -0.5f , 1f ) + 3.5f * i);

            tmp_tree = Instantiate(tree[Random.Range(0, tree.Length)], transform);
            tmp_tree.transform.position =
                new Vector3(1 + Random.Range(-1, 1f) + 5f * i,
                -5 + Random.Range(-0.5f, 1f) + 3.5f * i);
        }
        for (int i = 0; i < 30; i++)
        {
            var tmp_shadow = Instantiate(shadow[Random.Range(0, shadow.Length)], transform);
            tmp_shadow.transform.position =
                new Vector3(-11 + Random.Range(-0.3f, 0.3f) + 3f * i,
                0 + Random.Range(-0.3f, 0.3f) + 2f * i);
            tmp_shadow = Instantiate(shadow[Random.Range(0, shadow.Length)], transform);
            tmp_shadow.transform.position =
                new Vector3(-11 + Random.Range(-0.3f, 0.3f) + 3f * i,
                0 + Random.Range(-0.3f, 0.3f) + 3f * i);


            tmp_shadow = Instantiate(shadow[Random.Range(0, shadow.Length)], transform);
            tmp_shadow.transform.position =
                new Vector3(0 + Random.Range(-0.3f, 0.3f) + 3f * i,
                -7 + Random.Range(-0.3f, 0.3f) + 2.2f * i);
            tmp_shadow = Instantiate(shadow[Random.Range(0, shadow.Length)], transform);
            tmp_shadow.transform.position =
                new Vector3(0 + Random.Range(-0.3f, 0.3f) + 3f * i,
                -7 + Random.Range(-0.3f, 0.3f) + 1.2f * i);


        }
    }
}
