using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimeAnimationBasic : MonoBehaviour
{
    public HexagonTile HexTile;
    public Animator animator;
    public float speedAnimation;
    
    public AnimationCurve AnimationCurve;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        animator.speed = HexTile.lifeTime * speedAnimation;
        if (HexTile.lifeTime == 1 ) 
        {
            animator.speed = 0.55f;
        }

    }
}
