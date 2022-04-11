using UnityEngine;

public class LineController : MonoBehaviour
{
    public Transform pillar;
    public Transform point;

    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        pillar = gameObject.transform;
    }
    
    public void Connection()
    {
        lr.SetPosition(0, pillar.position);
        lr.SetPosition(1, point.position);
    }

    public void DestroyLine()
    {
        Destroy(gameObject);
    }
}
