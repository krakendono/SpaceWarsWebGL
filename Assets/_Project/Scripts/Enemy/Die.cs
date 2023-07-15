using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    // Simply destroy this obbject so that it makes the player aim to get it before it dies
    private void Start()
    {
        Destroy(this.gameObject, 3);
    }
}
