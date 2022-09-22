using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class TDCharacterController : MonoBehaviour
{

    public Vector3 movement;
    public Rigidbody2D rb;
    // Start is called before the first frame update



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

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
        //todo handle exceptional state such as forzen 
        //todo handle force apply
        //todo handle moving platform
        Vector2 newMovement = rb.position + new Vector2(movement.x * Time.fixedDeltaTime, movement.y * Time.fixedDeltaTime);
        rb.MovePosition(newMovement);

    }



    public void ApplyMovement( Vector3 m_movement)
    {

        movement = m_movement;
    }

    public void StopMovement( )
    {

        rb.velocity = new Vector3(0, 0, 0);
        
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
