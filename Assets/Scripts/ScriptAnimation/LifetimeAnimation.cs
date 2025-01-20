using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifetimeAnimation : MonoBehaviour
{
    public HexagonTile HexTile;
    public Animator animator;
    private float speedAnimation;
    private float lifetime;
    

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        lifetime = HexTile.lifeTime;
        speedAnimation = 50 + lifetime;
        animator.SetFloat("Speed", speedAnimation);
    }
}
