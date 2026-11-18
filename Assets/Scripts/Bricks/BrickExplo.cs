using UnityEngine;

public class BrickExplo : ParentBrick
{
    protected override void Activate()
    {

        RaycastHit2D[] hitUp = Physics2D.RaycastAll(gameObject.transform.position, Vector3.up, 1.4f);
        RaycastHit2D[] hitDown = Physics2D.RaycastAll(gameObject.transform.position, Vector3.down, 1.4f);
        RaycastHit2D[] hitLeft = Physics2D.RaycastAll(gameObject.transform.position, Vector3.left, 1.4f);
        RaycastHit2D[] hitRight = Physics2D.RaycastAll(gameObject.transform.position, Vector3.right, 1.4f);
        ParentBrick brickUp = null;
        foreach (RaycastHit2D hit in hitUp)
        {
            if (hit.collider != null)
            {
                brickUp = hit.collider.gameObject.GetComponent<ParentBrick>();
                break;
            }
        }
        ParentBrick brickDown = null;
        foreach (RaycastHit2D hit in hitDown)
        {
            if (hit.collider != null)
            {
                brickDown = hit.collider.gameObject.GetComponent<ParentBrick>();
                break;
            }
        }
        ParentBrick brickLeft = null;
        foreach (RaycastHit2D hit in hitLeft)
        {
            if (hit.collider != null)
            {
                brickLeft = hit.collider.gameObject.GetComponent<ParentBrick>();
                break;
            }
        }
        ParentBrick brickRight = null;
        foreach (RaycastHit2D hit in hitRight)
        {
            if (hit.collider != null)
            {
                brickRight = hit.collider.gameObject.GetComponent<ParentBrick>();
                break;
            }
        }
        brickUp?.TakeDamage(true);
        brickDown?.TakeDamage(true);
        brickLeft?.TakeDamage(true);
        brickRight?.TakeDamage(true);
    }
}
