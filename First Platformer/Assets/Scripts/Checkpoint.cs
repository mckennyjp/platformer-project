using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public HealthManager healthMan;

    public Renderer theRend;

    public Material checkPointOff;
    public Material checkPointOn;
 
    // Start is called before the first frame update
    void Start()
    {
        healthMan = FindObjectOfType<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckPointOn()
    {
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint cp in checkpoints)
        {
            cp.CheckPointOff();
        }

        theRend.material = checkPointOn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            healthMan.SetSpawnPoint(transform.position);
            CheckPointOn();
        }
    }

    public void CheckPointOff()
    {
        theRend.material = checkPointOff;
    }

}
