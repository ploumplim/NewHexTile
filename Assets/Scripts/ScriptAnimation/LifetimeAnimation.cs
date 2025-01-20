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
        lifetime = HexTile.lifeTime;
        speedAnimation =AnimationCurve.Evaluate(lifetimeAnimation) * lifetime;
        
        animator.speed =speedAnimation ;
    }
}
