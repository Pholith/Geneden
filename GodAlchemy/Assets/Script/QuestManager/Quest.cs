using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    [SerializeField]
    protected int ObjectiveAmount;

    protected int currentAmount;
    public string description;

    public Quest(int objective) {
        ObjectiveAmount = objective;
    }

    public virtual bool IsAccomplished() {
        return currentAmount >= ObjectiveAmount;
    }

    public virtual void SetCurrentAmount(int newAmount) {
        currentAmount = newAmount;
    }
}
