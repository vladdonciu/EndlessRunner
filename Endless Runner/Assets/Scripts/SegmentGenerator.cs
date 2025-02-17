using UnityEngine;
using System.Collections;

public class SegmentGenerator : MonoBehaviour
{
    public GameObject segmentMap01;
    public GameObject segmentMap02;
    public GameObject segmentMap03;
    public GameObject segmentMap04;
    public GameObject segmentMap05;
    public GameObject segmentMap06;
    public GameObject segmentMap07;




    void Start()
    {
        StartCoroutine(SegmentGen());
    }

    IEnumerator SegmentGen()
    {
        yield return new WaitForSeconds(10);
        segmentMap02.SetActive(true);
        yield return new WaitForSeconds(10);
        segmentMap03.SetActive(true);

    }


}
