using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    public struct Ability
    {
        public int abilityID;
        public int abilityPower;
        public Ability(int _abilityID, int _abilityPower)
        {
            abilityID = _abilityID;
            abilityPower = _abilityPower;
        }
    }
    public static GameManager instance;

    [SerializeField]
    TextMeshPro tX_PlayerNum;
    [SerializeField]
    GateController gate_Prefab;
    [SerializeField]
    PlayerUnit playerUnit_Prefab;
    [SerializeField]
    Transform playerGroup;

    public Transform playerTarget;

    int _playerNum;
    int PlayerNum {
        get
		{
            return _playerNum;
		}
        set
        {
            _playerNum = Mathf.Clamp(value, 0, 500);
        }
    }
    int playerVisualMaxNum = 100;
    float speed = 10;

    [HideInInspector]
    public string[] abilityName = { "+", "-", "x", "÷"};

    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();

    float gateTimer = 0;
    float gateTimerTotal = 1;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        PlayerNum = 0;
        CreateUnit(1);
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        playerTarget.Translate(Vector3.right * horizontal * speed * Time.deltaTime);
        if (playerTarget.position.x < -5)
		{
            playerTarget.position = new Vector3(-5, playerTarget.position.y, playerTarget.position.z);
        }
        else if (playerTarget.position.x > 5)
        {
            playerTarget.position = new Vector3(5, playerTarget.position.y, playerTarget.position.z);
        }

        if(gateTimer > gateTimerTotal)
		{
            gateTimer = 0;
            gateTimerTotal = 0.5f + Random.value * 3;


            GateController gate = Instantiate(gate_Prefab);
            gate.transform.position = new Vector3(0, 0, 25);
        }
        else
		{
            gateTimer += Time.deltaTime;
		}
    }

	private void LateUpdate()
	{
        tX_PlayerNum.text = PlayerNum.ToString();
    }

	void CreateUnit(int num)
    {
        //Change PlayerNum
        PlayerNum += num;

        var increasedNum = Mathf.Clamp(num, 0, playerVisualMaxNum - playerUnitList.Count);
        for (int i = 0; i < increasedNum; i++)
		{
            PlayerUnit unit = Instantiate(playerUnit_Prefab, playerGroup);
            unit.transform.position = playerTarget.position + new Vector3(Random.value, Random.value, Random.value);
            playerUnitList.Add(unit);
        }
	}

    public void RemoveUnit(PlayerUnit unit)
	{
        playerUnitList.Remove(unit);
        Destroy(unit.gameObject);
    }

    public void ApplyGateAbility(Ability ability)
	{
        if(ability.abilityID == 0)
		{
            // +
            CreateUnit(ability.abilityPower);
        }
        else if (ability.abilityID == 1)
        {
            // -
            int decreasedNum = CalculateDecreasedPlayerNum(PlayerNum - ability.abilityPower);

            for (int i = 0; i < decreasedNum; i++)
            {
                RemoveUnit(playerUnitList[Random.Range(0, playerUnitList.Count)]);
            }
        }
        else if (ability.abilityID == 2)
        {
            // x
            CreateUnit(PlayerNum * (ability.abilityPower - 1));
        }
        else if (ability.abilityID == 3)
        {
            // ÷
            int decreasedNum = CalculateDecreasedPlayerNum(Mathf.RoundToInt((float)PlayerNum / ability.abilityPower));


            for (int i = 0; i < decreasedNum; i++)
            {
                RemoveUnit(playerUnitList[Random.Range(0, playerUnitList.Count)]);
            }
        }
    }

    int CalculateDecreasedPlayerNum(int formulaResult)
	{
        int decreasedNum = 0;
        if (PlayerNum > playerVisualMaxNum)
        {
            if (formulaResult < playerVisualMaxNum)
            {
                decreasedNum = playerVisualMaxNum - formulaResult;
            }
        }
        else
        {
            decreasedNum = Mathf.Max(0, PlayerNum - formulaResult);
        }

        //Change PlayerNum
        PlayerNum = formulaResult;

        return decreasedNum;
    }
}
