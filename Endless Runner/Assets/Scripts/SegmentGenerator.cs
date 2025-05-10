using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SegmentGenerator : MonoBehaviour
{
    public GameObject[] segmentPrefabs;
    public Transform player; // Se va popula automat dacă e gol
    public int poolSize = 10; // Număr de segmente pre-instanțiate

    [SerializeField] int zPos = 50;
    [SerializeField] bool creatingSegment = false;
    [SerializeField] float deleteDelay = 2f;

    private List<SegmentTracker> activeSegments = new List<SegmentTracker>();
    private Queue<GameObject>[] segmentPools;

    private void Awake()
    {
        // Inițializează pool-urile pentru fiecare tip de segment
        segmentPools = new Queue<GameObject>[segmentPrefabs.Length];

        for (int i = 0; i < segmentPrefabs.Length; i++)
        {
            segmentPools[i] = new Queue<GameObject>();

            // Pre-instanțierea segmentelor
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
        // Găsește jucătorul dacă nu este atribuit manual în Inspector
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject == null && CharacterSelectionManager.Instance != null && CharacterSelectionManager.Instance.selectedCharacter != null)
            {
                // CAZ RAR: Dacă nu există încă în scenă, încearcă să găsești runnerPrefab-ul
                // ATENȚIE: runnerPrefab-ul din ScriptableObject nu este instanța din scenă!
                // Deci, această linie va funcționa doar dacă runnerPrefab-ul este deja instanțiat și are tag-ul "Player"
                playerObject = GameObject.Find(CharacterSelectionManager.Instance.selectedCharacter.runnerPrefab.name + "(Clone)");
            }

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError("Player transform not found! Asignează manual sau verifică instanțierea.");
            }
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

    // Scoate un segment din pool
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
            // Dacă pool-ul e gol, instanțiază unul nou
            GameObject segment = Instantiate(segmentPrefabs[segmentType]);
            return segment;
        }
    }

    // Returnează segmentul în pool în loc să-l distrugă
    private void ReturnSegmentToPool(GameObject segment, int segmentType)
    {
        segment.SetActive(false);
        segmentPools[segmentType].Enqueue(segment);
    }

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
