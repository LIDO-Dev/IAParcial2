using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSM
{
    protected NPC npc;

    public NPCState(NPC npc)
    {
        this.npc = npc;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
