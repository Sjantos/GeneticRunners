using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FireFlyOperators
{
    public static CreatureData MoveTowardsOther(CreatureData original, CreatureData other, float t, bool randUnderT = false)
    {
        var tmpT = t;

        var newCreature = new CreatureData()
        {
            positions = original.positions.Clone() as Vector3[],
            structure = original.structure.Clone() as int[],
            timers = original.timers.Clone() as float[]
        };

        for (int i = 0; i < other.positions.Length; i++)
        {
            if (randUnderT)
                tmpT = UnityEngine.Random.Range(0f, t);
            newCreature.positions[i] = Vector3.Lerp(newCreature.positions[i], other.positions[i], tmpT);
        }
        for (int i = 0; i < other.timers.Length; i++)
        {
            if (randUnderT)
                tmpT = UnityEngine.Random.Range(0f, t);
            newCreature.timers[i] = Mathf.Lerp(newCreature.timers[i], other.timers[i], tmpT);
        }

        return newCreature;
    }

    public static CreatureData ExploreRandomly(CreatureData original, int fireflySize, float randomMoveScale = 1f)
    {
        Vector3[] pos = RandomExtension.RandomArray(fireflySize, Creature.positionBounds);
        float[] tim = RandomExtension.RandomArray(fireflySize, Creature.minMuscleTime, Creature.maxMuscleTime);
        RandomExtension.Shuffle(pos);
        RandomExtension.Shuffle(tim);
        var maxRandomCreature =  new CreatureData()
        {
            positions = pos,
            structure = original.structure,
            timers = tim
        };
        var newCreature = new CreatureData()
        {
            positions = original.positions.Clone() as Vector3[],
            structure = original.structure.Clone() as int[],
            timers = original.timers.Clone() as float[]
        };
        for (int i = 0; i < pos.Length; i++)
        {
            newCreature.positions[i] = Vector3.Lerp(newCreature.positions[i], maxRandomCreature.positions[i], randomMoveScale);
        }
        for (int i = 0; i < tim.Length; i++)
        {
            newCreature.timers[i] = Mathf.Lerp(newCreature.timers[i], maxRandomCreature.timers[i], randomMoveScale);
        }
        return newCreature;
    }

    public static float Distance(CreatureData c1, CreatureData c2)
    {
        var result = 0f;
        var posCount = c1.positions.Length < c2.positions.Length ? c1.positions.Length : c2.positions.Length;
        var timCount = c1.timers.Length < c2.timers.Length ? c1.timers.Length : c2.timers.Length;
        for (int i = 0; i < posCount; i++)
            result += Vector3.Distance(c1.positions[i], c2.positions[i]);
        for (int i = 0; i < timCount; i++)
            result += (10f * Mathf.Abs(c1.timers[i] - c2.timers[i]));
        return result;
    }
}
