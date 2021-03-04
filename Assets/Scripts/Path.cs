using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public List<Tuile> Checkpoints;


    public Path()
    {
        Checkpoints = new List<Tuile>();
    }

    public Tuile GetNextTuile(Tuile a_currentTuile)
    {
        if (Checkpoints.Count == 0)
            return null;

        //Si tuile null, return first tuile
        if (a_currentTuile == null)
        {
            return Checkpoints[0];
        }

        int t_CurrentIndex = Checkpoints.IndexOf(a_currentTuile);

        // si tuile == derniere, return null
        if (t_CurrentIndex == Checkpoints.Count - 1)
        {
            return null;
        }

        //sinon, retourne la suivante
        return Checkpoints[t_CurrentIndex + 1];
    }
}
