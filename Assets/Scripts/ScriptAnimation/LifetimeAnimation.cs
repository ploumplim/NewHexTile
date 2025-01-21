using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifetimeAnimation : MonoBehaviour
{
    public HexagonTile HexTile;
    public Animator animator;
    private float speedAnimation;
    private float lifetime;
    private float lifetimeAnimation;
    public AnimationCurve AnimationCurve;
    

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        
        if (HexTile.lifeTime == 1 ) 
        {
            lifetime = 15;
        }
        if (HexTile.lifeTime == 2)
        {
            lifetime = 9;
        }
        if (HexTile.lifeTime == 3)
        {
            lifetime = 6;
        }
        if (HexTile.lifeTime == 4)
        {
            lifetime = 3;
        }
        if (HexTile.lifeTime >= 5 )
        {
            lifetime = 1;
        }
       
        speedAnimation =AnimationCurve.Evaluate(lifetimeAnimation) * lifetime;
        
        animator.speed =speedAnimation ;
    }
}
