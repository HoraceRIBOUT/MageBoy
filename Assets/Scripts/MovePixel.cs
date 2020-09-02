using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class MovePixel : MonoBehaviour
{
    public Vector2 pixelSize = new Vector2(160, 144);
    public Vector4 minMaxPoxXY = new Vector4(-5.5f, 5.5f, -4, 6);
    public Vector2 moveSize;


    public Vector2 caseSize = new Vector2(26,26);
    public Vector2 moveCase;


    public float moveEveryXSeconds = 2f;
    private bool canMove = true;
    private float secretTimer = 0;

    public void Start()
    {
        moveSize.x = (minMaxPoxXY.y - minMaxPoxXY.x) / pixelSize.x;
        moveSize.y = (minMaxPoxXY.w - minMaxPoxXY.z) / pixelSize.y;
        secretTimer = 0;

        moveCase.x = (minMaxPoxXY.y - minMaxPoxXY.x) * caseSize.x / pixelSize.x;
        moveCase.y = (minMaxPoxXY.w - minMaxPoxXY.z) * caseSize.y / pixelSize.y;
    }

    public float deathZoneForMove = 0.2f;

    public void Update()
    {
        if (canMove)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(x) > deathZoneForMove)
            {
                this.transform.Translate(Vector3.right * Mathf.Sign(x) * moveCase.x);
            }
            if (Mathf.Abs(y) > deathZoneForMove)
            {
                this.transform.Translate(Vector3.up * Mathf.Sign(y) * moveCase.y);
            }

            if (Mathf.Abs(x) > deathZoneForMove || Mathf.Abs(y) > deathZoneForMove)
            {
                canMove = false;
            }
        }


        if ((secretTimer + Time.deltaTime) % moveEveryXSeconds < (secretTimer) % moveEveryXSeconds)
        {
            canMove = true;
        }
        secretTimer += Time.deltaTime;
    }
    



    //public Vector2 offset = new Vector2(1 / 3.75f, 1 / 2.0f);
    //public GameObject pixel;
    //public List<GameObject> generatedPixel = new List<GameObject>();
    //[MyBox.ButtonMethod()]
    //public void Populate()
    //{
    //    Delete();
    //    Start();
    //    for (int i = 0; i < pixelSize.x; i++)
    //    {
    //        for (int j = 0; j < pixelSize.y; j++)
    //        {
    //            if((i+j)% 2 == 0)
    //            {
    //                Vector2 pos = new Vector2(
    //                    minMaxPoxXY.x + ((minMaxPoxXY.y - minMaxPoxXY.x) / pixelSize.x) * i,
    //                    minMaxPoxXY.z + ((minMaxPoxXY.w - minMaxPoxXY.z) / pixelSize.y) * j);
    //                pos.x -= ((minMaxPoxXY.y - minMaxPoxXY.x) / pixelSize.x) * offset.x;
    //                pos.y += moveSize.y * offset.y;

    //                generatedPixel.Add(Instantiate(pixel, pos, Quaternion.identity, this.transform));
    //                generatedPixel[generatedPixel.Count - 1].GetComponent<SpriteRenderer>().color = Color.black;
    //                generatedPixel[generatedPixel.Count - 1].GetComponent<SpriteRenderer>().sortingOrder = 3;
    //                generatedPixel[generatedPixel.Count - 1].transform.localScale *= 6.94444f;
    //            }
    //        }
    //    }
    //}

    //[MyBox.ButtonMethod()]
    //public void Delete()
    //{
    //    foreach (GameObject gO in generatedPixel)
    //    {
    //        DestroyImmediate(gO);
    //    }
    //    generatedPixel.Clear();
    //}

}
