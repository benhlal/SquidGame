using UnityEngine;
using System.Threading.Tasks;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] protected float movementSpeed = 100f;
    public bool isImmortal { get; private set; }

    protected Rigidbody rb;

    protected Animator animator;

    protected float verticalDirection = 1;
    protected float horizontalDirection = 1;

    [SerializeField] protected bool isAlive = true;
    [SerializeField] private AudioSource shot;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        shot = GetComponent<AudioSource>();
    }

    public bool IsMoving()
    {
        bool isMoving = rb.velocity.magnitude >5f;
        Debug.Log("==========>   IS MOVING CHECK :" + isMoving + "  IS ALIVE  : " + isAlive + " ***  TYPE : " +
                  rb.gameObject.name);

        return isMoving;
    }

    void FixedUpdate()
    {
        if (isAlive == true)
        {
            
                rb.velocity = Vector3.forward * verticalDirection * movementSpeed * Time.fixedDeltaTime;

                Debug.Log("======> Methode: [FixedUpdate] Comment: [ Alive supposed moveForward]  IS ALIVE  : [" + isAlive +
                          "] " +
                          "***  Object Type  :[ " + rb.gameObject.name + "]  Velocity: [" + rb.velocity + "]");
                
          
        }
        else
        {
            rb.velocity = Vector3.zero;
            Debug.Log("======> Methode: [FixedUpdate] Comment: [ Cant move forward Dead supposed]  IS ALIVE  : [" +
                      isAlive + "] " +
                      "***  Object Type  :[ " + rb.gameObject.name + "]  Velocity: [" + rb.velocity + "]");
        }
    }


    public virtual void Die()
    {
        Debug.Log("======> Methode: [Die] Comment: [ Start DIe]  IS ALIVE  : [" + isAlive + "] " +
                  "***  Object Type  :[ " + rb.gameObject.name + "]  Velocity: [" + rb.velocity + "]");
        
        shot.PlayOneShot(shot.clip);
        
        animator.SetTrigger("Death");
        isAlive = false;

        Debug.Log("======> Methode: [Die] Comment: [ Dead ]  IS ALIVE  : [" + isAlive + "] " +
                  "***  Object Type  :[ " + rb.gameObject.name + "]  Velocity: [" + rb.velocity + "]");
    }

    public virtual void Win()
    {
        isImmortal = true;
    }
}