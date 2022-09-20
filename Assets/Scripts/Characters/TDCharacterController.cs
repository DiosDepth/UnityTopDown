using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class TDCharacterController : MonoBehaviour
{
    
    public CharacterController cc;
    public Rigidbody2D rb;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CharacterController>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {

    }

    private void FixedUpdate()
    {

    }

    public Vector3 CalculateMovement(Vector3 m_dir, float m_speed)
    {
        if(m_dir == null)
        {
            return Vector3.zero;
        }
        Vector3 tempDir = m_dir;
        if(tempDir.sqrMagnitude > 1)
        {
            tempDir = tempDir.normalized;
        }

        tempDir = tempDir * m_speed;
        return tempDir;
    }

    public void ApplyMovement(MovementType type, Vector3 m_movement)
    {

        switch(type)
        {
            case MovementType.NonPhysical:
                if (cc == null)
                {
                    Debug.Log("TDCharacterController : " + "Can't find charactercontroller on this object!");
                    return;
                }
                cc.Move(m_movement * Time.fixedDeltaTime);
                break;
            case MovementType.Physicals:
                if (rb == null)
                {
                    Debug.Log("TDCharacterController : " + "Can't find rigidbody on this object!");
                    return;
                }
                rb.velocity = new Vector3(0, 0, 0);
                rb.velocity = m_movement * Time.fixedDeltaTime;
                break;
        }

      
    }

    public void StopMovement(MovementType type)
    {
        switch (type)
        {
            case MovementType.NonPhysical:
                
                cc.Move(Vector3.zero);
                break;
            case MovementType.Physicals:
                rb.velocity = new Vector3(0, 0, 0);
                break;
        }
    
    }

    public void DebugVelocity()
    {
        Debug.Log (this.name + "--->TDCharacterController : " + "Velocity = " + rb.velocity);
    }
    public void ApplyRotationWithQuaternion(Transform m_trs, Quaternion m_rotation)
    {
        m_trs.rotation = m_rotation;
    }

    public void Flip(Transform m_trs)
    {
        if (m_trs == null) return;
        m_trs.localScale = m_trs.localScale * -1;
    }

    public void FlipWithMovdingDirection(Transform m_trs, Vector3 m_dir)
    {
        if (m_trs == null)
        {
            Debug.Log("Can't flip cus m_trs is null");
        }

        m_trs.localScale =new Vector3(  m_dir.x > 0 ? 1 : -1, m_trs.localScale.y, m_trs.localScale.z);
    }
    public bool IsMoving()
    {
        return rb.velocity.sqrMagnitude != 0;
    }

    public bool IsMoving(MovementType type,Vector3 mov)
    {
        if (type == MovementType.NonPhysical)
        {
            return mov.sqrMagnitude != 0;
        }
        else
        {
            return rb.velocity.sqrMagnitude != 0;
        }
    }


    
}
