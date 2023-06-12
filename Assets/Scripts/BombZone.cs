using UnityEngine;
using MoreMountains.NiceVibrations;

public class BombZone : MonoBehaviour
{
    public GameObject bomb;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject != bomb.GetComponent<Bomb>().exception && other.gameObject != bomb && other.tag != "Env" && other.tag != "Wall" && other.tag != "BombZone" && other.tag != "MovingBomb")
        {
            if (bomb.GetComponent<Bomb>().isCheckZone)
            {
                try
                {
                    var randomExplosion = Random.Range(0, bomb.GetComponent<Bomb>().explosions.Length);
                    var explosion = bomb.GetComponent<Bomb>().explosions[randomExplosion];
                    var explosion2 = bomb.GetComponent<Bomb>().explosions[randomExplosion].transform.GetChild(0);
                    var explosion3 = bomb.GetComponent<Bomb>().explosions[randomExplosion].transform.GetChild(1);
                    Color otherColor = Color.white;
                    if (other.tag == "Dot")
                    {
                        otherColor = other.GetComponent<Renderer>().material.color;
                    }
                    else if (other.tag == "Bot")
                    {
                        otherColor = other.GetComponent<Renderer>().material.color;
                    }
                    else if (other.tag == "Player")
                    {
                        otherColor = other.GetComponent<Renderer>().material.color;
                    }
                    var mainColor = explosion.GetComponent<ParticleSystem>().main;
                    mainColor.startColor = otherColor;
                    var mainColor2 = explosion2.GetComponent<ParticleSystem>().main;
                    mainColor2.startColor = otherColor;
                    var mainColor3 = explosion3.GetComponent<ParticleSystem>().main;
                    mainColor3.startColor = otherColor;
                    Instantiate(bomb.GetComponent<Bomb>().explosions[randomExplosion], other.transform.position, Quaternion.Euler(0, 0, 0));
                    int score = (int)((0.05f + other.transform.localScale.x / 10) * 100);
                    if (bomb.GetComponent<Bomb>().exception.tag == "Player")
                    {
                        bomb.GetComponent<Bomb>().PlusSize(other.transform.localScale.x);
                        if (other.tag == "Dot")
                        {
                            GameController.instance.Scoring(score, other.gameObject);
                        }
                        else
                        {
                            string enemyName = other.name;
                            string killText = "You Killed " + enemyName;
                            GameController.instance.KillText(killText);
                            GameController.instance.UpdateKillCount();
                            GameController.instance.Scoring(score, other.gameObject);
                        }
                        if (GameController.instance.isVibrate == 0)
                            MMVibrationManager.Vibrate();
                    }
                    else
                    {
                        bomb.GetComponent<Bomb>().PlusSize(other.transform.localScale.x * 3);
                        score *= 3;
                        bomb.GetComponent<Bomb>().exception.GetComponent<Bot>().UpdateBotKillCount(score);
                    }
                    if (other.tag == "Player")
                    {
                        string botName = bomb.GetComponent<Bomb>().exception.name;
                        Color botColor = bomb.GetComponent<Bomb>().exception.GetComponent<Bot>().botColor;
                        GameController.instance.Respawn(botName, botColor);
                    }
                    else
                    {
                        //var trail = other.transform.GetChild(1);
                        //trail.gameObject.SetActive(false);
                        other.gameObject.SetActive(false);
                        bomb.GetComponent<Bomb>().spawner.StartSpawnBot();
                    }
                }
                catch { }
            }
        }
    }
}
