using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class FallingObject : MonoBehaviour
{
    public float speed = 5.0f;
    public float destroyZ = GameConfig.MoveDownDestroyZ;
    protected Rigidbody objectRb;

    protected virtual void Start()
    {
        objectRb = GetComponent<Rigidbody>();
        objectRb.useGravity = true;
        objectRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }

    protected virtual void Update()
    {
        objectRb.velocity = new Vector3(0, 0, -speed);

        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}
