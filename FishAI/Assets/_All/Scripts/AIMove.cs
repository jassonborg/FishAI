using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{

    //Declare Variables for AISPawner manager script
    private AISpawner m_AIManager;


    //Declare Variables for moving and turning
    private bool m_hasTarget = false;
    private bool m_isTurning;

    //Variable for the current waypoint
    private Vector3 m_wayPoint;
    private Vector3 m_lastWaypoint = new Vector3 (0f,0f,0f);

    //going to use this to set the animation speed
    private Animator m_animator;
    private float m_speed;
    private float m_scale;

    private Collider m_collider;
    private RaycastHit m_hit;


    // Start is called before the first frame update
    void Start()
    {
        //get the AISpawner from it's parent
        m_AIManager = transform.parent.GetComponentInParent<AISpawner>();
        m_animator = GetComponent<Animator>();

        SetUpNPC();
    }

    void SetUpNPC()
    {
        //Randomly scale each NPC
        float m_scale = Random.Range(1f, 2f);
        transform.localScale += new Vector3(m_scale * 1.5f, m_scale, m_scale);
    }

    // Update is called once per frame
    void Update()
    {
        //if we have not found a way point ot move to
        //if we found a waypoint, we need to move there
        if (!m_hasTarget)
        {
            m_hasTarget = CanFindTarget();
        }
        else
        {
            //make sure we rotate the NPC to face it's waypoint
            RotateNPC(m_wayPoint, m_speed);
            //move the NPC in a Straight line toward the waypoint
            transform.position = Vector3.MoveTowards(transform.position, m_wayPoint, m_speed * Time.deltaTime);
        }

        //if NPC reaches waypoint reset target
        if (transform.position == m_wayPoint)
        {
            m_hasTarget = false;
        }
        
    }


    bool CanFindTarget(float start = 1f, float end = 7f)
    {
        m_wayPoint = m_AIManager.RandomWaypoint();
        //make sure we don't set the waypoint twice
        if (m_lastWaypoint == m_wayPoint)
        {
            //get a new waypoint
            m_wayPoint = m_AIManager.RandomWaypoint();
            return false;
        }
        else
        {
            //set the new waypoint as the last waypoint
            m_lastWaypoint = m_wayPoint;
            //get random speed for movement and animation
            m_speed = Random.Range(start, end);
            m_animator.speed = m_speed;
            //set bool to true to say we found a WP
            return true;
        }
    }

    //Rotate the NPC to face the new waypoint
    void RotateNPC (Vector3 waypoint, float currentSpeed)
    {
        //get random speed for the turn
        float TurnSpeed = currentSpeed * Random.Range(1f, 3f);

        //get new direction to look at for target
        Vector3 LookAt = waypoint - this.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LookAt), TurnSpeed * Time.deltaTime);
    }
}
