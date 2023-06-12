using System.Collections;
using UnityEngine;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class Bomb : MonoBehaviour
{
    public ParticleSystem[] explosions;
    public bool isCheckZone = false;
    public GameObject exception;
    public Color exceptionColor;
    public int exceptionCount = 1;
    public float sizeScale;
    public GameObject zoneExplode;
    float timeDelayExplode = 1.5f;
    public Spawner spawner;

    private void OnEnable()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        exceptionCount = 1;
        zoneExplode.transform.DOKill();
        zoneExplode.transform.localScale = Vector3.zero;
        zoneExplode.GetComponentInChildren<Light>().DOKill();
        zoneExplode.GetComponentInChildren<Light>().color = Color.white;
        zoneExplode.GetComponentInChildren<Light>().intensity = 0;
        isCheckZone = false;
        exception = null;
        sizeScale = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        var isMovingBomb = Random.Range(0, 10);
        GetComponent<Renderer>().material.DOKill();     
        GetComponent<Renderer>().material.color = new Color32(255,255,255,130);
        GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * 0.25f);
        timeDelayExplode = 1.5f;
    }

    public void BombTrigger(GameObject host)
    {
        sizeScale = host.transform.localScale.x;
        exception = host.gameObject;
        if (host.gameObject.tag == "Bot")
        {
            timeDelayExplode = 2f;
        }
        else
        {
            GameController.instance.counterClearText = 0;
        }
        GetComponent<Renderer>().material.DOColor(exceptionColor, timeDelayExplode);
        DOTween.To(() => GetComponent<Renderer>().material.GetColor("_EmissionColor"), x => GetComponent<Renderer>().material.SetColor("_EmissionColor", x), exceptionColor * 0.5f, timeDelayExplode);
        StartCoroutine(delayBomb());
        exceptionColor = host.GetComponent<Renderer>().material.color;
        zoneExplode.GetComponent<Renderer>().material.color = new Color(exceptionColor.r, exceptionColor.g, exceptionColor.b, 0.2f);
        zoneExplode.transform.DOScale(new Vector3(sizeScale * 2 + 10, 0.005f, sizeScale * 2 + 10), 0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "BombZone" && other.GetComponentInParent<Bomb>().isCheckZone)
        {
            if (exceptionCount > 0)
            {
                //try
                //{
                    exceptionCount--;
                    exception = other.GetComponentInParent<Bomb>().exception;
                    sizeScale = exception.transform.localScale.x;
                    exceptionColor = exception.GetComponent<Renderer>().material.color;
                    zoneExplode.GetComponent<Renderer>().material.color = new Color(exceptionColor.r, exceptionColor.g, exceptionColor.b, 0.2f);
                    zoneExplode.transform.DOScale(new Vector3(sizeScale * 2 + 10, 0.005f, sizeScale * 2 + 10), 0.2f);
                    GetComponent<Renderer>().material.DOColor(exceptionColor, timeDelayExplode);
                    DOTween.To(() => GetComponent<Renderer>().material.GetColor("_EmissionColor"), x => GetComponent<Renderer>().material.SetColor("_EmissionColor", x), exceptionColor * 0.25f, timeDelayExplode);
                    StartCoroutine(delayBomb());
                //}
                //catch { }
            }
        }
    }

    public void PlusSize(float bonus)
    {
        if (exception.transform.localScale.x < 3)
        {
            var size = exception.transform.localScale.x + 0.05f + bonus / 10;
            if (size > 5)
            {
                size = 5;
            }
            exception.transform.localScale = new Vector3(size, size, size);
            exception.GetComponent<Rigidbody>().mass += (0.05f + bonus / 10) / 100;
            if (exception.tag == "Bot")
            {
                var aim = exception.transform.GetChild(0);
                aim.GetComponent<SphereCollider>().radius = (35 / (exception.transform.localScale.x)) + (exception.transform.localScale.x);
            }
        }
    }

    IEnumerator delayBomb()
    {
        yield return new WaitForSeconds(timeDelayExplode - 0.2f);
        isCheckZone = true;
        zoneExplode.transform.DOScale(Vector3.zero, 0);
        zoneExplode.GetComponent<Renderer>().material.color = new Color(exceptionColor.r, exceptionColor.g, exceptionColor.b, 0.5f);
        zoneExplode.GetComponentInChildren<Light>().color = exceptionColor;
        zoneExplode.GetComponentInChildren<Light>().DOIntensity(sizeScale * 2 + 20, 0.2f);
        zoneExplode.transform.DOScale(new Vector3(sizeScale * 2 + 10, 0.005f, sizeScale * 2 + 10), 0.2f);
        yield return new WaitForSeconds(0.2f);
        var zone = transform.GetChild(0).localScale = Vector3.zero;
        var randomExplosion = Random.Range(0, explosions.Length);
        var explosion = explosions[randomExplosion];
        var explosion2 = explosions[randomExplosion].transform.GetChild(0);
        var explosion3 = explosions[randomExplosion].transform.GetChild(1);
        var mainColor = explosion.GetComponent<ParticleSystem>().main;
        mainColor.startColor = exceptionColor;
        var mainColor2 = explosion2.GetComponent<ParticleSystem>().main;
        mainColor2.startColor = exceptionColor;
        var mainColor3 = explosion3.GetComponent<ParticleSystem>().main;
        mainColor3.startColor = exceptionColor;
        Instantiate(explosions[randomExplosion], transform.position, Quaternion.Euler(0, 0, 0));
        if (exception.tag == "Player")
        {
            SoundManager.instance.PlaySound(SoundManager.instance.bomb);
            if (GameController.instance.isVibrate == 0)
                MMVibrationManager.Vibrate();
        }
        Spawner.instance.RefreshLeaderboard();
        gameObject.SetActive(false);
    }
}
