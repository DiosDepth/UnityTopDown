using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDAIPerception : MonoBehaviour
{
    public bool isDebug = false;

    public bool hasHearing;
    public float hearingRadius;
    public LayerMask hearingMask;

    public GameObject[] hearingObjects;
    public TDCharacter[] hearingAI;
    public TDCharacter hearingPlayer;

    public RaycastHit[] hearingHits;


    public bool hasVision;
    public float visionRadius = 10;
    public float visionAngle = 110;
    public LayerMask visionMask;
    public LayerMask visionBlockMask;

    public GameObject[] seeingObjects;
    public TDCharacter[] seeingAI;
    public TDCharacter seeingPlayer;

    public RaycastHit[] visionHits;

    private Vector3 characterFaceDirection;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void UpdatePerception()
    {
        characterFaceDirection = GetComponent<TDCharacterAbilityMovement>().faceDirection;
        if(hasVision)
        {
            VisionCheck();
        }
        if (hasHearing)
        {
            HearingCheck();
        }
    }

    public void HearingCheck()
    {
        List<GameObject> objects = new List<GameObject>();
        List<TDCharacter> ai = new List<TDCharacter>();
        hearingPlayer = null;
        Collider[] temp = Physics.OverlapSphere(transform.position, hearingRadius, hearingMask);

        for (int i = 0; i < temp.Length; i++)
        {
            if(temp[i].gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                hearingPlayer = temp[i].GetComponent<TDCharacter>();
                continue;
            }
            if(temp[i].gameObject.layer == LayerMask.NameToLayer("AI"))
            {
                if(!temp[i].gameObject.Equals(this.gameObject))
                {
                    ai.Add(temp[i].GetComponent<TDCharacter>());
                    continue;
                }
                
            }
            objects.Add(temp[i].gameObject);
        }
        hearingAI = ai.ToArray();
        hearingObjects = objects.ToArray();
    }



    public void VisionCheck()
    {
        List<GameObject> objects = new List<GameObject>();
        List<TDCharacter> ai = new List<TDCharacter>();
        seeingPlayer = null;
        Collider[] temp = Physics.OverlapSphere(transform.position,visionRadius, visionMask);

        for (int i = 0; i < temp.Length; i++)
        {
            if(Vector3.Angle(characterFaceDirection, (temp[i].transform.position - transform.position).normalized) <= visionAngle/2)
            {
                
                if (temp[i].gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if (!Physics.Raycast(transform.position, temp[i].transform.position,visionRadius,visionBlockMask))
                    {
                        seeingPlayer = temp[i].GetComponent<TDCharacter>();
                        continue;
                    }
                    else
                    {
                        seeingPlayer = null;
                        continue;
                    }
                        
                }
                if (temp[i].gameObject.layer == LayerMask.NameToLayer("AI"))
                {
                    if (!temp[i].gameObject.Equals(this.gameObject))
                    {
                        if (!Physics.Raycast(transform.position, temp[i].transform.position, visionRadius, visionBlockMask))
                        {
                            ai.Add(temp[i].GetComponent<TDCharacter>());
                            continue;
                        }
                    }
                }
                if (!Physics.Raycast(transform.position, temp[i].transform.position, visionRadius, visionBlockMask))
                {
                    objects.Add(temp[i].gameObject);
                }
            }
        }

        seeingAI = ai.ToArray();
        seeingObjects = objects.ToArray();
    }

    private void OnDrawGizmos()
    {
        if(!isDebug)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hearingRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, characterFaceDirection * visionRadius + transform.position);
        Gizmos.DrawLine(transform.position, Quaternion.Euler(0, -1 * visionAngle / 2, 0) * characterFaceDirection * visionRadius + transform.position);
        Gizmos.DrawLine(transform.position, Quaternion.Euler(0, visionAngle / 2, 0) * characterFaceDirection * visionRadius + transform.position);
        Gizmos.color = Color.red;

        if(hearingPlayer)
        {
            Gizmos.DrawSphere(hearingPlayer.transform.position, 0.5f);
        }

        if (seeingPlayer)
        {
            Gizmos.DrawLine(transform.position, seeingPlayer.transform.position);
        }
        if (hearingObjects.Length > 0)
        {
            for (int i = 0; i < hearingObjects.Length; i++)
            {
                
                Gizmos.DrawSphere(hearingObjects[i].transform.position, 0.5f);
            }
        }

        if(seeingObjects.Length>0)
        {
            for (int i = 0; i < seeingObjects.Length; i++)
            {
                Gizmos.DrawLine(transform.position, seeingObjects[i].transform.position);
            }
        }
    }
}
