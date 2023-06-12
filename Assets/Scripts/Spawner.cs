using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    public GameObject player;
    public EZObjectPool botSpawner;
    public EZObjectPool bombSpawner;
    public List<GameObject> leaderboardList = new List<GameObject>();
    public List<GameObject> rankList = new List<GameObject>();
    public Text rank1Text;
    public Text rank2Text;
    public Text rank3Text;
    public Text currentRankText;
    public List<Color> skinList = new List<Color>();
    public List<Sprite> flagList = new List<Sprite>();
    float mapLimit = 30;
    public GameObject crown;

    void OnEnable()
    {
        instance = this;
        skinListUpdate(GameController.instance.randomSkin);
        for (int i = 0; i < botSpawner.ObjectList.Count; i++)
        {
            leaderboardList.Add(botSpawner.ObjectList[i]);
            var randomSkin = Random.Range(0, skinList.Count);
            var skin = skinList[randomSkin];
            var setColor = botSpawner.ObjectList[i];
            setColor.GetComponent<Renderer>().material.color = skin;
            setColor.GetComponent<Renderer>().material.SetColor("_EmissionColor", skin * 0.5f);
            skinList.RemoveAt(randomSkin);
            skinList.TrimExcess();
            var randomFlag = Random.Range(0, flagList.Count);
            botSpawner.ObjectList[i].GetComponent<Bot>().botFlag.sprite = flagList[randomFlag];
            GameObject bot;
            botSpawner.TryGetNextObject(GameController.instance.spawnPoints[i], Quaternion.Euler(new Vector3(0, 0, 0)), out bot);
            bot.transform.localScale = new Vector3(1, 1, 1);
        }
        InvokeRepeating("SpawnBot", 0, 2f);
        leaderboardList.Add(player);
        RefreshLeaderboard();
    }

    void SpawnBot()
    {
        GameObject bot;
        try
        { 
            botSpawner.TryGetNextObject(new Vector3(Random.Range(-mapLimit, mapLimit), 1.5f, Random.Range(-mapLimit, mapLimit)), Quaternion.Euler(new Vector3(0, 0, 0)), out bot);
            bot.transform.localScale = new Vector3(1, 1, 1);
        }
        catch {
            CancelInvoke("SpawnBot");
        }
    }

    public void SpawnBomb(Transform pos, Vector3 dirPush)
    {
        GameObject bomb;
        bombSpawner.TryGetNextObject(new Vector3(pos.position.x, 0.15f, pos.position.z), Quaternion.Euler(new Vector3(0, 0, 0)), out bomb);
        bomb.GetComponent<Rigidbody>().isKinematic = false;
        if (pos.tag == "Bot")
        {
            dirPush *= 2000 * transform.localScale.x;
            dirPush = new Vector3(Mathf.Clamp(dirPush.x, -4000, 4000), 0, Mathf.Clamp(dirPush.z, -4000, 4000));
        }
        else
        {
            dirPush *= 2500 * transform.localScale.x;
            dirPush = new Vector3(Mathf.Clamp(dirPush.x, -4000, 4000), 0, Mathf.Clamp(dirPush.z, -4000, 4000));
        }
        bomb.GetComponent<Rigidbody>().AddForce(dirPush);
        var host = pos.transform.parent;
        bomb.GetComponent<Bomb>().BombTrigger(host.gameObject);
        float torque;
        if (dirPush.x > dirPush.z)
        {
            torque = dirPush.x;
        }
        else
        {
            torque = dirPush.z;
        }
        bomb.GetComponent<Rigidbody>().AddTorque(new Vector3(0, torque, 0));
    }

    public void StartSpawnBot()
    {
        InvokeRepeating("SpawnBot", 2, 2);
    }

    public void RefreshLeaderboard()
    {
        rankList = leaderboardList.ToList();
        GameObject temp;
        for (int j = 0; j <= rankList.Count - 2; j++)
        {
            for (int i = 0; i <= rankList.Count - 2; i++)
            {
                int a;
                int b;
                if(rankList[i].tag == "Bot")
                {
                    a = rankList[i].GetComponent<Bot>().botTotalScore;
                }
                else
                {
                    a = rankList[i].GetComponent<GameController>().totalScore;
                }
                if (rankList[i + 1].tag == "Bot")
                {
                    b = rankList[i + 1].GetComponent<Bot>().botTotalScore;
                }
                else
                {
                    b = rankList[i + 1].GetComponent<GameController>().totalScore;
                }
                if (a < b)
                {
                    temp = rankList[i + 1];
                    rankList[i + 1] = rankList[i];
                    rankList[i] = temp;
                }
            }
        }

        if (rankList[0].tag == "Bot")
        {
            rank1Text.text = "#1   " + rankList[0].GetComponent<Bot>().botTotalScore + "  " + rankList[0].name;
            rank1Text.color = rankList[0].GetComponent<Bot>().botColor;
            rank1Text.transform.parent.GetComponent<Image>().color = new Color32(0, 0, 0, 130);

        }
        else
        {
            rank1Text.text = "#1   " + rankList[0].GetComponent<GameController>().totalScore + "  " + rankList[0].name;
            rank1Text.color = Color.white;
            rank1Text.transform.parent.GetComponent<Image>().color = GameController.instance.playerColor;
        }
        if (rankList[1].tag == "Bot")
        {
            rank2Text.text = "#2   " + rankList[1].GetComponent<Bot>().botTotalScore + "  " + rankList[1].name;
            rank2Text.color = rankList[1].GetComponent<Bot>().botColor;
            rank2Text.transform.parent.GetComponent<Image>().color = new Color32(0, 0, 0, 130);
        }
        else
        {
            rank2Text.text = "#2   " + rankList[1].GetComponent<GameController>().totalScore + "  " + rankList[1].name;
            rank2Text.color = Color.white;
            rank2Text.transform.parent.GetComponent<Image>().color = GameController.instance.playerColor;
        }
        if (rankList[2].tag == "Bot")
        {
            rank3Text.text = "#3   " + rankList[2].GetComponent<Bot>().botTotalScore + "  " + rankList[2].name;
            rank3Text.color = rankList[2].GetComponent<Bot>().botColor;
            rank3Text.transform.parent.GetComponent<Image>().color = new Color32(0, 0, 0, 130);
        }
        else
        {
            rank3Text.text = "#3   " + rankList[2].GetComponent<GameController>().totalScore + "  " + rankList[2].name;
            rank3Text.color = Color.white;
            rank3Text.transform.parent.GetComponent<Image>().color = GameController.instance.playerColor;
        }
        for (int i = 0; i < rankList.Count; i++)
        {
            if (rankList[i].tag == "Bot")
            {
                rankList[i].GetComponent<bl_Hud>().HudInfo.m_Text = (i + 1).ToString();
            }
            else
            {
                currentRankText.text = "#" + (i + 1).ToString() + "   " + rankList[i].GetComponent<GameController>().totalScore + "  " + rankList[i].name;
            }
        }
        crown.transform.parent = rankList[0].transform;
        crown.transform.localPosition = new Vector3(0, 2, 0);
        crown.transform.localScale = rankList[0].transform.localScale;
    }

    public void skinListUpdate(int id)
    {
        skinList.RemoveAt(id);
        skinList.TrimExcess();
    }
}
