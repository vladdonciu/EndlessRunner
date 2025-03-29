using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SegmentGenerator : MonoBehaviour
{
    public GameObject[] segmentPrefabs;
    public Transform player;
    public int poolSize = 10; // Number of segments to pre-instantiate

    [SerializeField] int zPos = 50;
    [SerializeField] bool creatingSegment = false;
    [SerializeField] float deleteDelay = 2f;

    private List<SegmentTracker> activeSegments = new List<SegmentTracker>();
    private Queue<GameObject>[] segmentPools;

    private void Awake()
    {
        // Initialize pools for each segment type
        segmentPools = new Queue<GameObject>[segmentPrefabs.Length];

        for (int i = 0; i < segmentPrefabs.Length; i++)
        {
            segmentPools[i] = new Queue<GameObject>();

            // Pre-instantiate segments
            for (int j = 0; j < poolSize; j++)
            {
                GameObject segment = Instantiate(segmentPrefabs[i]);
                segment.SetActive(false);
                segmentPools[i].Enqueue(segment);
            }
        }
    }

    void Start()
    {
        // Găsește jucătorul dacă nu este atribuit
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
                Debug.LogError("Player transform not assigned and couldn't be found!");
        }

        // Generează primul segment la start
        if (!creatingSegment)
        {
            creatingSegment = true;
            StartCoroutine(SegmentGen());
        }
    }

    void Update()
    {
        if (!creatingSegment)
        {
            creatingSegment = true;
            StartCoroutine(SegmentGen());
        }

        CheckPlayerPosition();
    }

    // Get a segment from the pool
    private GameObject GetSegmentFromPool(int segmentType)
    {
        if (segmentPools[segmentType].Count > 0)
        {
            GameObject segment = segmentPools[segmentType].Dequeue();
            segment.SetActive(true);
            return segment;
        }
        else
        {
            // If pool is empty, instantiate a new one
            GameObject segment = Instantiate(segmentPrefabs[segmentType]);
            return segment;
        }
    }

    // Return segment to the pool instead of destroying
    private void ReturnSegmentToPool(GameObject segment, int segmentType)
    {
        segment.SetActive(false);
        segmentPools[segmentType].Enqueue(segment);
    }

    // Update your SegmentGen coroutine to use pooling
    IEnumerator SegmentGen()
    {
        int segmentNum = Random.Range(0, segmentPrefabs.Length);
        GameObject newSegment = GetSegmentFromPool(segmentNum);
        newSegment.transform.position = new Vector3(0, 0, zPos);

        float segmentLength = 50f;
        float startZ = zPos - (segmentLength / 2);
        float endZ = zPos + (segmentLength / 2);

        activeSegments.Add(new SegmentTracker(newSegment, startZ, endZ, segmentNum));

        zPos += 50;

        yield return new WaitForSeconds(4);
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

    // Update your SegmentTracker class to include segment type
    private class SegmentTracker
    {
        public GameObject segment;
        public float startZ;
        public float endZ;
        public bool markedForDeletion;
        public int segmentType;

        public SegmentTracker(GameObject seg, float start, float end, int type)
        {
            segment = seg;
            startZ = start;
            endZ = end;
            markedForDeletion = false;
            segmentType = type;
        }
    }

    // Update your deletion method to return to pool
    IEnumerator DeleteSegmentAfterDelay(SegmentTracker tracker, int index, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (tracker.segment != null)
        {
            ReturnSegmentToPool(tracker.segment, tracker.segmentType);
            Debug.Log("Segment returned to pool after player left it");
        }

        if (index < activeSegments.Count && activeSegments[index] == tracker)
        {
            activeSegments.RemoveAt(index);
        }
    }
}
