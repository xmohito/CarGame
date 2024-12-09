using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class AIHandler : MonoBehaviour
{

    [SerializeField]
    CarHandler carHandler;
    RaycastHit[] raycastHits = new RaycastHit[1];
    bool isCarAhead = false;
    [SerializeField]
    LayerMask otherCarsLayerMask;

    [SerializeField]
    MeshCollider meshCollider;
    WaitForSeconds wait = new WaitForSeconds(0.2f);

      int drivingInLane = 0;

    private void Awake()
    {
        if(CompareTag("Player"))
        {
            Destroy(this);
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateLessOfTenCO());
    }

    // Update is called once per frame
    void Update()
    {
        float accelerationInput = 1.0f;

        float steerInput = 0.0f;

        if (isCarAhead)
        {
            accelerationInput = -1;
        }

            float desiredPositionX = Utils.CarLanes[drivingInLane];
            float difference = desiredPositionX - transform.position.x;

        if(Mathf.Abs(difference) > 0.05f)
        {
            steerInput = 1.0f * difference;
        }
        

        steerInput = Mathf.Clamp(steerInput, -1.0f, 1.0f);

        carHandler.SetInput(new Vector2(steerInput, accelerationInput));

    }

    IEnumerator UpdateLessOfTenCO()
    {
        while (true)
        {
            isCarAhead = CheckIfOtherCarsIsAheda();
            yield return wait;
        }
    }

    bool CheckIfOtherCarsIsAheda()
    {
        meshCollider.enabled = false;
        int numberOfHits = Physics.BoxCastNonAlloc(transform.position, Vector3.one * 0.25f, transform.forward, raycastHits, Quaternion.identity, 2, otherCarsLayerMask);
        meshCollider.enabled = true;

        if(numberOfHits > 0)
        {
            return true;
        }
        return false;
    }

    private void OnEnable()
    {
        carHandler.SetMaxSpeed(Random.Range(2, 4));

        drivingInLane = Random.Range(0, Utils.CarLanes.Length);
    }
}
