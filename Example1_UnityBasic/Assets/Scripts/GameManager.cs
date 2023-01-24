using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField]
    private CubePlayer player;
    [SerializeField]
    GameObject coinPrefab;
    [SerializeField]
    LayerMask rayLayerMask_Floor;
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    TextMeshProUGUI text_Score;

    private float timer = 0;
    private float timerTotal = 1;

    [HideInInspector]
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        //Create coins
        if(timer > timerTotal)
		{
            timer = 0;
            GameObject coin = Instantiate(coinPrefab);
            Vector3 v3 = new Vector3(Random.Range(-10f, 10f), 30, Random.Range(-10f, 10f));
            coin.transform.position = v3;
        }
        else
		{
            timer += Time.deltaTime;
		}

        //Mouse click detection
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 touchUpPos = Input.mousePosition;
            Ray currentRay = mainCamera.ScreenPointToRay(touchUpPos);
            RaycastHit hit;
            if (Physics.Raycast(currentRay, out hit, 3000, rayLayerMask_Floor))
            {
                player.MoveTo(hit.point);
            }
        }

        //Update UI
        text_Score.text = "<color=#000fff>Score: </color>" + score;
    }


}
