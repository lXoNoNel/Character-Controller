using System;
using UnityEngine;

public class FoundFood : IState
{
    GameObject food;
    Action<GameObject> destroyFoodCallback;
    public FoundFood(GameObject food, Action<GameObject> destroyFoodCallback)
    {
        this.food = food;
        this.destroyFoodCallback = destroyFoodCallback;
    }

    public override void Enter()
    {
        this.destroyFoodCallback(food);
    }
    
    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        
    }
}
