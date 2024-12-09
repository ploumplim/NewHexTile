using UnityEngine;

public abstract class States : MonoBehaviour
{
   protected GameManager GM;
   
   public virtual void Initialize(GameManager GM)
   {
      this.GM = GM;
   }

   public virtual void Enter()
   {
      
   }

   public virtual void Tick()
   {
      
   }

   public virtual void Exit()
   {
      
   }
}
