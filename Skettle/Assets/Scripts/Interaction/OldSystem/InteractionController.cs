using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*TODO: This script needs considerable improvement.
 * - The calculate target function needs to be simplified/cleaned up
 * - The enter exit functions apparently add too much to the potential target list
 * - Just a simple enter and exit would work. Potentially have two different buttons, one for gameplay interactions (levers, etc) , another for world interactions (characters, doors, etc)
 *          - Similarly, have two different current targets update, one for each interaction type
*/

public class InteractionController : GameplayComponent
{
    public bool somethingInRange;
    [SerializeField] GameObject eKey;

    [SerializeField] Interactable currentTarget;

    [SerializeField] List<Interactable> potentialTargets = new List<Interactable>();
    List<Interactable> removeTargetList = new List<Interactable>();

    Interactable potentialTarget;
    float potentialDistance = 0f;
    [SerializeField] public float maxDistance = 10f;

    public override void GameplayUpdate()
    {
        if(Input.GetKey(KeyCode.E) && currentTarget != null)
        {
            Interact(Input.GetKeyDown(KeyCode.E));
        }

        if(currentTarget != null)
        {
            eKey.SetActive(true);
        }
        else
        {
            eKey.SetActive(false);
        }
    }

    public void Interact(bool buttonDown)
    {
        currentTarget.Interact(buttonDown,this);
    }

    public void CalculateTarget()
    {
        potentialDistance = maxDistance;
        foreach(Interactable io in potentialTargets)
        {
            if(potentialTarget == null)
            {
                removeTargetList.Add(potentialTarget);
                continue;
            }

            if((io.gameObject.transform.position - transform.position).magnitude < potentialDistance)
            {
                potentialDistance = (io.gameObject.transform.position - transform.position).magnitude;
                potentialTarget = io;
            }
        }

        foreach(Interactable io in removeTargetList)
        {
            potentialTargets.Remove(io);
        }

        removeTargetList.Clear();

        currentTarget = potentialTarget;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable io = collision.GetComponent<Interactable>();
        if (io != null)
        {
            currentTarget = io;
            //potentialTargets.Add(io);
            //CalculateTarget();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Interactable io = collision.GetComponent<Interactable>();
        if (io != null && currentTarget == io)
        {
            currentTarget = null;
            //potentialTargets.Remove(io);
            //CalculateTarget();
        }
    }
}
