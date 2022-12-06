using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrQuest : Quest
{
    private Quest quest1;
    private Quest quest2;

    public OrQuest(Quest questArg1, Quest questArg2) : base(0) {
        quest1 = questArg1;
        quest2 = questArg2;
        this.description = "yeet"; //TODO
    }

    public override bool IsAccomplished() {
        return quest1.IsAccomplished() || quest2.IsAccomplished();
    }
}
