using System.Collections.Generic;
using UnityEngine;

public enum AlignAxis
{
    X,
    Y,
    Z
}

public class PryInteractor : MonoBehaviour
{
    [Header("Interaction requirements")]
    public string pryInteractorTag;

    public string blockerTag;

    public Rigidbody rb;

    [Range(0f, 1f)]
    public float prySensitivity = 1f;

    public AlignAxis localAxisConstraint = AlignAxis.X;

    //Private stuff
    [Header("Privates")]
    [SerializeField]
    private GameObject cachedInteractor;

    [SerializeField]
    private float lastTickYRot = 0f;

    [SerializeField]
    private float dotDir = 0f;

    [SerializeField]
    private int directionToMove = 0;

    [SerializeField]
    private Vector3 alignmentAxis;

    [SerializeField]
    private List<PryInteractor> cachedFrameSockets = new List<PryInteractor>();

    [SerializeField]
    private GameObject blocker;

    private void Start()
    {
        //Set the alignmentAxis using the global stuff
        switch (localAxisConstraint)
        {
            case AlignAxis.X:
                alignmentAxis = gameObject.transform.right;
                break;
            case AlignAxis.Y:
                alignmentAxis = gameObject.transform.up;
                break;
            case AlignAxis.Z:
                alignmentAxis = gameObject.transform.forward;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == pryInteractorTag)
        {
            //Cache the pryToolInteractor
            cachedInteractor = other.gameObject;

            /*Calculate and store the dot product between the frame's alignmentAxis and the tool position
            This will get the correct direction to move the frames when the prying motion occurs*/
            Vector3 vectorToTool_Y_Exclusive = new Vector3(cachedInteractor.transform.position.x, 0, cachedInteractor.transform.position.z) 
                - new Vector3(transform.position.x, 0, transform.position.z);
            dotDir = Vector3.Dot(alignmentAxis, vectorToTool_Y_Exclusive.normalized);

            if (dotDir == 0f) return;

            directionToMove = dotDir > 0 ? 1 : -1;

            lastTickYRot = cachedInteractor.transform.eulerAngles.y;
        }
        else if (other.tag == blockerTag)
        {
            //Touched a blocker
            blocker = other.gameObject;
        }
        else
        {
            //If its not a tool, check if its a frame socket
            PryInteractor temp = other.GetComponent<PryInteractor>();

            if (temp != null && !CheckCacheHasFrame(temp))
            {
                //This is a frame and it is not already in the cache
                cachedFrameSockets.Add(temp);
            }
        }
    }

    private bool CheckCacheHasFrame(PryInteractor _v)
    {
        foreach (PryInteractor p in cachedFrameSockets)
        {
            if (_v == p)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == pryInteractorTag)
        {
            //Clear the cached pryToolInteractor and any values associated with it
            cachedInteractor = null;
            dotDir = 0;
            directionToMove = 0;
            lastTickYRot = 0;
        }
        else if (other.tag == blockerTag)
        {
            //Touched a blocker
            blocker = null;
        }
        else
        {
            //If its not a tool, check if its a frame socket
            PryInteractor temp = other.GetComponent<PryInteractor>();

            if (temp != null && CheckCacheHasFrame(temp))
            {
                //This is a frame socket and it is in the cache, remove it
                cachedFrameSockets.Remove(temp);
            }
        }
    }

    public bool CheckCanMove(int _depth, Vector3 _movement)
    {
        //Check if there is a blocker in the way
        if (blocker != null)
        {
            Vector3 dirToBlocker = blocker.transform.position - transform.position;
            if (Vector3.Dot(_movement, dirToBlocker) > 1)
            {
                //The blocker is in the way
                return false;
            }
        }
        if (cachedFrameSockets.Count == 0)
        {
            //Touching no frames, so it can move
            return true;
        }
        else
        {
            foreach (PryInteractor p in cachedFrameSockets)
            {
                Vector3 dirToObject = p.transform.position - transform.position;

                //Check if it is in the way of movement
                if (Vector3.Dot(_movement, dirToObject.normalized) > 0)
                {
                    if (_depth == 0)
                    {
                        return false;
                    }
                    else
                    {
                        //The object is in the way
                        return p.CheckCanMove(_depth - 1, _movement);
                    }
                }
            }
        }
        return true;
    }

    public void MoveFrames(Vector3 _movement)
    {
        foreach (PryInteractor p in cachedFrameSockets)
        {
            Vector3 dirToObject = p.transform.position - transform.position;

            //Check if it is in the way of movement
            if (Vector3.Dot(_movement, dirToObject.normalized) > 0)
            {
                p.MoveFrames(_movement);
            }
        }
        transform.position += _movement;
    }

    private void Update()
    {
        if (cachedInteractor == null)
        {
            return;
        }

        //Calculate the amount of movement to apply
        float newYRotation = cachedInteractor.transform.eulerAngles.y;

        float movementAmount = Mathf.Abs(newYRotation - lastTickYRot) * prySensitivity;

        //Try to apply the movement
        if (CheckCanMove(2, (alignmentAxis * -directionToMove * movementAmount)))
        {
            //The frame can move
            MoveFrames((alignmentAxis * -directionToMove * movementAmount));
        }

        lastTickYRot = newYRotation;
    }
}
