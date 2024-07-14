using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float ghostdelay;
    private float ghostdelayseconds;
    public GameObject ghost;
    public bool makeghost;
    // Start is called before the first frame update
    void Awake()
    {
        ghostdelayseconds = ghostdelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(makeghost)
        {
            if (ghostdelayseconds > 0)
            {
                ghostdelayseconds -= Time.deltaTime;
            }
            else
            {

                GameObject currentghost = Instantiate(ghost, transform.position, transform.rotation);
                Sprite currentsprite = GetComponent<SpriteRenderer>().sprite;
                currentghost.transform.localScale = this.transform.localScale;
                currentghost.GetComponent<SpriteRenderer>().sprite = currentsprite;
                ghostdelayseconds = ghostdelay;
                Destroy(currentghost,1f);
            }
        }

    }
}
