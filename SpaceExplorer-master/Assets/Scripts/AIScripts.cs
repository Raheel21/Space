using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScripts : MonoBehaviour
{
    [SerializeField] Vector3 target;
    [SerializeField] Vector3 normaltarget;
    bool turning;
    public GameObject warpObj;
    [SerializeField] int distanceFromCenter;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.position - transform.up;
        StartCoroutine("checkPath");
    }

    //This runs the avoidance code over 0.05 seconds- if this was in Update it would be run once per frame
    //upwards of 60 frames per second, a big increase.
    IEnumerator checkPath()
    {
        yield return new WaitForSeconds(0.05f);
        Avoidance();
        StartCoroutine("checkPath");

    }

    private bool testApproximate(float a, float b, float tolerance)
    {
        return (Mathf.Abs(a - b) < tolerance);
    }

    public void Avoidance()
    {
        //subtract AI thing’s position from waypoint, player, whatever it is going towards…

        //set the target to be a position just in front of the ship
        target = transform.position - transform.up;
        float differenceRotationAngle = Quaternion.Angle(transform.rotation, torotation);
        if (differenceRotationAngle < 5)
        {
            turning = false;
        }

        //if it's far enough from the center (we use Absolute to remove the sign, doesn't matter if it's -500 or +500)
        if (Mathf.Abs(target.x) > distanceFromCenter || Mathf.Abs(target.y) > distanceFromCenter || Mathf.Abs(target.z) > distanceFromCenter)
        {

            Vector3 NewLocation;
            bool isInside;

            //Inside UnitSphere picks a random point inside a sphere of 1 unit width
            NewLocation = (Random.insideUnitSphere) * (distanceFromCenter - 100f);
            //CheckSphere checks if the object will fit in that new location
            isInside = Physics.CheckSphere(NewLocation, transform.GetComponentInChildren<MeshCollider>().bounds.extents.magnitude);

            //if the sphere is blocked pick a new one
            while (isInside)
            {
                NewLocation = (Random.insideUnitSphere) * (distanceFromCenter - 100);
                isInside = Physics.CheckSphere(NewLocation, transform.GetComponentInChildren<MeshCollider>().bounds.extents.magnitude);
            }
            //rather than instantiate warp objects we just reuse existing ones

            warpObj.transform.position = transform.position;
            warpObj.GetComponent<ParticleSystem>().Play();
            foreach (ParticleSystem p in warpObj.GetComponentsInChildren<ParticleSystem>())
            {
                p.Play();
            }

            transform.position = NewLocation;

        }
        else
        {
            target = transform.position - transform.up;

        }
        if (!turning)
        {
            //normalize target to get direction
            normaltarget = target.normalized;

            //now make a new raycast hit
            //and draw a line from the AI out some distance in the forward direction
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -transform.up, out hit, 100f))
            {
                //check that its not hitting itself
                //then add the normalised hit direction to your direction plus some repulsion force
                if (hit.transform != transform)
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);

                    //normaltarget += hit.normal * 300f;
                    normaltarget = -transform.up + new Vector3(0, 20, 0);
                    turning = true;
                }

            }

            //now make two more raycasts out to the left and right to make the cornering more accurate and reducing collisions more

            Vector3 leftR = transform.position;
            Vector3 rightR = transform.position;

            leftR.x -= 3;
            rightR.x += 3;

            if (Physics.Raycast(leftR, -transform.up, out hit, 100f))
            {
                if (hit.transform != transform)
                {
                    Debug.DrawLine(leftR, hit.point, Color.red);
                    //normaltarget += hit.normal * 200f;
                    normaltarget = -transform.up + new Vector3(20, 0, 0);
                    turning = true;

                }

            }
            if (Physics.Raycast(rightR, -transform.up, out hit, 500f))
            {
                if (hit.transform != transform)
                {
                    Debug.DrawLine(rightR, hit.point, Color.red);
                    normaltarget = -transform.up + new Vector3(-20, 0, 0);
                    turning = true;
                    //normaltarget += hit.normal * 200f;
                }
            }
        }
    }
    Quaternion torotation;

    // Update is called once per frame
    void Update()
    {
        //set the look rotation toward this new target based on the collisions
        if (normaltarget!=Vector3.zero)
        {
            torotation= Quaternion.LookRotation(normaltarget);

            //then slerp the rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, torotation, Time.deltaTime * 5f);
            //finally add some propulsion to move the object forward based on this rotation

            transform.position = Vector3.Lerp(transform.position, transform.position - transform.up, speed);
        }
    }
}
