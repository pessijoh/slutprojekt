using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private NPCController occupant;

    public bool IsFree()
    {
        return occupant == null;
    }

    public void SetOccupant(NPCController npc)
    {
        occupant = npc;
    }

    public void ClearOccupant()
    {
        occupant = null;
    }

    public NPCController GetOccupant()
    {
        return occupant;
    }
}
