using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SegmentGenerator : MonoBehaviour
{
    public GameObject[] segment;
    public Transform player; 

    [SerializeField] int zPos = 50;
    [SerializeField] bool creatingSegment = false;
    [SerializeField] int segmentNum;
    [SerializeField] float deleteDelay = 2f;

    private List<SegmentTracker> activeSegments = new List<SegmentTracker>();

    private class SegmentTracker
    {
        public GameObject segment;
        public float startZ;
        public float endZ;
        public bool markedForDeletion;

        public SegmentTracker(GameObject seg, float start, float end)
        {
            segment = seg;
            startZ = start;
            endZ = end;
            markedForDeletion = false;
        }
    }

    void Start()
    {
     
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
                Debug.LogError("Player transform not assigned and couldn't be found!");
        }
    }

    void Update()
    {
        if (creatingSegment == false)
        {
            creatingSegment = true;
            StartCoroutine(SegmentGen());
        }

        
        CheckPlayerPosition();
    }

    IEnumerator SegmentGen()
    {
        segmentNum = Random.Range(0, 3);
        GameObject newSegment = Instantiate(segment[segmentNum], new Vector3(0, 0, zPos), Quaternion.identity);

       
        float segmentLength = 50f;
        float startZ = zPos - (segmentLength / 2);
        float endZ = zPos + (segmentLength / 2);

    
        activeSegments.Add(new SegmentTracker(newSegment, startZ, endZ));

        zPos += 50;

        yield return new WaitForSeconds(3);
        creatingSegment = false;
    }

    void CheckPlayerPosition()
    {
        if (player == null) return;

        float playerZ = player.position.z;

        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            SegmentTracker tracker = activeSegments[i];

            if (playerZ > tracker.endZ && !tracker.markedForDeletion)
            {
                tracker.markedForDeletion = true;
                StartCoroutine(DeleteSegmentAfterDelay(tracker, i, deleteDelay));
            }
        }
    }

    IEnumerator DeleteSegmentAfterDelay(SegmentTracker tracker, int index, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (tracker.segment != null)
        {
            Destroy(tracker.segment);
            Debug.Log("Segment deleted after player left it");
        }

        if (index < activeSegments.Count && activeSegments[index] == tracker)
        {
            activeSegments.RemoveAt(index);
        }
    }
}
