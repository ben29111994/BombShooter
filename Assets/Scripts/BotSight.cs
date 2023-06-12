using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSight : MonoBehaviour
{
    public GameObject bot;
    public List<GameObject> opponents = new List<GameObject>();
    public List<GameObject> dots = new List<GameObject>();
    float countDetectRate = 0;
    public Spawner spawner;

    private void OnEnable()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
    }

    private void Update()
    {
        if(countDetectRate > 0)
        {
            countDetectRate -= 0.02f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bot.GetComponent<Bot>().isStop == false)
        {
            if (other.tag == "Player" || other.tag == "Bot")
            {
                opponents.Add(other.gameObject);
                try
                {
                    bot.GetComponent<Bot>().StopInvoke();
                    bot.GetComponent<Bot>().ChangeDir(other.transform.position);
                    bot.GetComponent<Bot>().isStop = true;
                    bot.GetComponent<Bot>().StartInvoke(1f);
                }
                catch { }
            }

            if (other.tag == "Wall")
            {
                bot.GetComponent<Bot>().StopInvoke();
                bot.GetComponent<Bot>().CheckWall(true);
                bot.GetComponent<Bot>().ChangeDir(new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5)));
            }

            if (other.tag == "MovingBomb")
            {
                bot.GetComponent<Bot>().MeetBomb(true);
                var runAway = transform.InverseTransformDirection(other.transform.position * 100);
                runAway.y = bot.transform.position.y;
                bot.GetComponent<Bot>().StopInvoke();
                bot.GetComponent<Bot>().ChangeDir(runAway);
                bot.GetComponent<Bot>().StartInvoke(1f);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (bot.GetComponent<Bot>().isStop == false)
        {
            if (other.tag == "MovingBomb" && countDetectRate <= 0)
            {
                var runAway = transform.InverseTransformDirection(other.transform.position * 100);
                runAway.y = bot.transform.position.y;
                bot.GetComponent<Bot>().StopInvoke();
                bot.GetComponent<Bot>().ChangeDir(runAway);
                bot.GetComponent<Bot>().StartInvoke(1f);
                countDetectRate += 0.5f;
            }

            if (other.tag == "Wall" && countDetectRate <= 0)
            {
                bot.GetComponent<Bot>().StopInvoke();
                bot.GetComponent<Bot>().CheckWall(true);
                bot.GetComponent<Bot>().ChangeDir(new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5)));
                countDetectRate += 0.5f;
            }

            if ((other.tag == "Player" || other.tag == "Bot") && countDetectRate <= 0)
            {
                GameObject target = other.gameObject;
                if (opponents.Count > 0)
                {
                    float decision = 0;
                    foreach (var item in opponents)
                    {
                        //if ((item == spawner.rankList[0] || item == spawner.rankList[1] || item == spawner.rankList[2]) && item.tag == "Bot")
                        //{
                        //    var randomDecision = Random.Range(0, 10);
                        //    if (randomDecision > 7)
                        //    {
                        //        target = item;
                        //        decision = 1;
                        //        break;
                        //    }
                        //}
                        //else
                        //{
                            var randomDecision = Random.Range(0, 10);
                            if (randomDecision > 5)
                            {
                                target = item;
                                decision = 1;
                                break;
                            }
                        //}
                    }

                    //if (decision > 0)
                    //{
                    //    bot.GetComponent<Bot>().StopInvoke();
                    //    bot.GetComponent<Bot>().ChangeDir(target.transform.position);
                    //    bot.GetComponent<Bot>().isStop = true;
                    //    bot.GetComponent<Bot>().StartInvoke(1f);
                    //}
                    //else
                    //{
                        var runAway = transform.InverseTransformDirection(target.transform.position * 100);
                        runAway.y = bot.transform.position.y;
                        bot.GetComponent<Bot>().StopInvoke();
                        bot.GetComponent<Bot>().ChangeDir(runAway);
                        bot.GetComponent<Bot>().StartInvoke(1f);
                        StartCoroutine(delayShootTarget(target));
                    //}
                }
                countDetectRate += 3f;
            }
        }
    }

    IEnumerator delayShootTarget(GameObject target)
    {
        yield return new WaitForSeconds(2);
        bot.GetComponent<Bot>().StopInvoke();
        bot.GetComponent<Bot>().ChangeDir(target.transform.position);
        bot.GetComponent<Bot>().isStop = true;
        bot.GetComponent<Bot>().StartInvoke(1f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (bot.GetComponent<Bot>().isStop == false)
        {
            if (other.tag == "Player" || other.tag == "Bot")
            {
                opponents.Remove(other.gameObject);
                opponents.TrimExcess();
                try
                {
                    bot.GetComponent<Bot>().StopInvoke();
                    bot.GetComponent<Bot>().ChangeDir(other.transform.position);
                    bot.GetComponent<Bot>().isStop = true;
                    bot.GetComponent<Bot>().StartInvoke(1f);
                }
                catch { }
            }

            if (other.tag == "Wall")
            {
                bot.GetComponent<Bot>().CheckWall(false);
                bot.GetComponent<Bot>().StartInvoke(1f);
            }

            if (other.tag == "MovingBomb")
            {
                bot.GetComponent<Bot>().MeetBomb(false);
            }
        }
    }
}
