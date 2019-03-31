using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GeneticOperators
{
    public static int[] Crossover(int[] array1, int[] array2, CrossoverMethod method)
    {
        switch (method)
        {
            //case CrossoverMethod.PMX:
            //    return PMXCrossover(array1, array2);
            case CrossoverMethod.OX:
                return OXCrossover(array1, array2);
            case CrossoverMethod.OnePoint:
                return OnePointCrossover(array1, array2);
            default:
                return new int[] { };
        }
    }

    public static float[] Crossover(float[] array1, float[] array2, CrossoverMethod method)
    {
        switch (method)
        {
            //case CrossoverMethod.PMX:
            //    return PMXCrossover(array1, array2);
            case CrossoverMethod.OX:
                return OXCrossover(array1, array2);
            case CrossoverMethod.OnePoint:
                return OnePointCrossover(array1, array2);
            default:
                return new float[] { };
        }
    }

    public static Vector3[] Crossover(Vector3[] array1, Vector3[] array2, CrossoverMethod method)
    {
        switch (method)
        {
            //case CrossoverMethod.PMX:
            //    return PMXCrossover(array1, array2);
            case CrossoverMethod.OX:
                return OXCrossover(array1, array2);
            case CrossoverMethod.OnePoint:
                return OnePointCrossover(array1, array2);
            default:
                return new Vector3[] { };
        }
    }

    public static T[] Mutation<T>(T[] solution, MutationMethod method)
    {
        switch (method)
        {
            case MutationMethod.None:
                return solution;
            case MutationMethod.Swap:
                return SwapMutation(solution);
            case MutationMethod.Scramble:
                return ScrambleMutation(solution);
            case MutationMethod.Inversion:
                return InversionMutation(solution);
            default:
                return new T[] { };
        }
    }
    private static int? randompositionmutationcount = null;
    public static Vector3[] PositionRandomMutation(Vector3[] solution, Vector3Bounds bounds)
    {
        solution[Random.Range(0, solution.Length)] = new Vector3(
                Random.Range(bounds.minX, bounds.maxX),
                Random.Range(bounds.minY, bounds.maxY),
                Random.Range(bounds.minZ, bounds.maxZ));
        //if (randompositionmutationcount == null)
        //    randompositionmutationcount = Mathf.RoundToInt(Mathf.Sqrt((float)solution.Length));
        //for (int i = 0; i < randompositionmutationcount; i++)
        //{
        //    solution[Random.Range(0, solution.Length)] = new Vector3(
        //        Random.Range(bounds.minX, bounds.maxX),
        //        Random.Range(bounds.minY, bounds.maxY),
        //        Random.Range(bounds.minZ, bounds.maxZ));
        //}
        return solution;
    }

    private static int[] OnePointCrossover(int[] mother, int[] father)
    {
        var solution = new int[mother.Length];
        for (int i = 0; i < solution.Length; i++)
            solution[i] = -1;

        List<int> gapsIndexes = new List<int>();
        int crossPoint = Random.Range(0, solution.Length);
        for (int i = 0; i < solution.Length; i++)
        {
            if (i < crossPoint)
                solution[i] = mother[i];
            else
            {
                if (solution.Contains(father[i]))
                {
                    gapsIndexes.Add(i);
                    continue;
                }
                else
                    solution[i] = father[i];
            }
        }
        //fill gaps
        for (int i = 0; i < gapsIndexes.Count; i++)
        {
            var lostGene = father.FirstOrDefault(p => !solution.Contains(p));
            solution[gapsIndexes[i]] = lostGene;
        }

        return solution;
    }

    /// <summary>
    /// Does not care about TSP-like genom correction
    /// </summary>
    private static float[] OnePointCrossover(float[] mother, float[] father)
    {
        var solution = new float[mother.Length];
        for (int i = 0; i < solution.Length; i++)
            solution[i] = -1f;

        List<int> gapsIndexes = new List<int>();
        int crossPoint = Random.Range(0, solution.Length);
        for (int i = 0; i < solution.Length; i++)
        {
            if (i < crossPoint)
                solution[i] = mother[i];
            else
                solution[i] = father[i];
        }

        return solution;
    }

    /// <summary>
    /// Does not care about TSP-like genom correction
    /// </summary>
    private static Vector3[] OnePointCrossover(Vector3[] mother, Vector3[] father)
    {
        var solution = new Vector3?[mother.Length];
        for (int i = 0; i < solution.Length; i++)
            solution[i] = null;

        List<int> gapsIndexes = new List<int>();
        int crossPoint = Random.Range(0, solution.Length);
        for (int i = 0; i < solution.Length; i++)
        {
            if (i < crossPoint)
                solution[i] = mother[i];
            else
                solution[i] = father[i];
        }

        return solution.Select(p => p.Value).ToArray();
    }

    private static int[] PMXCrossover(int[] mother, int[] father)
    {
        var solution = new int[mother.Length];
        for (int i = 0; i < solution.Length; i++)
            solution[i] = -1;
        int leftIndex = Random.Range(0, solution.Length);
        int rightIndex = Random.Range(0, solution.Length);
        if (rightIndex < leftIndex)
        {
            int tmp = leftIndex;
            leftIndex = rightIndex;
            rightIndex = tmp;
        }
        for (int i = leftIndex; i <= rightIndex; i++)
            solution[i] = mother[i];

        for (int i = leftIndex; i <= rightIndex; i++)
        {
            bool fatherValueFoundInChild = false;
            for (int j = 0; j < solution.Length; j++)
                if (solution[j] == father[i])
                {
                    fatherValueFoundInChild = true;
                    break;
                }

            if (!fatherValueFoundInChild)
            {
                int fatherIndex = -1, motherValue = mother[i];
                while (true)
                {
                    fatherIndex = Find<int>(father, motherValue);
                    if (solution[fatherIndex] != -1)
                        motherValue = mother[fatherIndex];
                    else
                        break;
                }
                solution[fatherIndex] = father[i];
            }
        }

        for (int i = 0; i < solution.Length; i++)
            if (solution[i] == -1)
                solution[i] = father[i];

        return solution;
    }

    private static Vector3[] PMXCrossover(Vector3[] mother, Vector3[] father)
    {
        var solution = new Vector3?[mother.Length];
        for (int i = 0; i < solution.Length; i++)
            solution[i] = null;
        int leftIndex = Random.Range(0, solution.Length);
        int rightIndex = Random.Range(0, solution.Length);
        if (rightIndex < leftIndex)
        {
            int tmp = leftIndex;
            leftIndex = rightIndex;
            rightIndex = tmp;
        }
        for (int i = leftIndex; i <= rightIndex; i++)
            solution[i] = mother[i];

        for (int i = leftIndex; i <= rightIndex; i++)
        {
            bool fatherValueFoundInChild = false;
            for (int j = 0; j < solution.Length; j++)
                if (solution[j] == father[i])
                {
                    fatherValueFoundInChild = true;
                    break;
                }

            if (!fatherValueFoundInChild)
            {
                int fatherIndex = -1;
                Vector3 motherValue = mother[i];
                while (true)
                {
                    fatherIndex = Find<Vector3>(father, motherValue);
                    if (solution[fatherIndex] != null)
                        motherValue = mother[fatherIndex];
                    else
                        break;
                }
                solution[fatherIndex] = father[i];
            }
        }

        for (int i = 0; i < solution.Length; i++)
            if (solution[i] == null)
                solution[i] = father[i];

        return solution.Select(p => p.Value).ToArray();
    }

    private static int[] OXCrossover(int[] mother, int[] father)
    {
        var solution = new int[mother.Length];
        for (int i = 0; i < solution.Length; i++)
            solution[i] = -1;
        int leftIndex = Random.Range(0, solution.Length);
        int rightIndex = Random.Range(0, solution.Length);
        if (rightIndex < leftIndex)
        {
            int tmp = leftIndex;
            leftIndex = rightIndex;
            rightIndex = tmp;
        }

        //Fill rest with father alleles
        int[] tmpFather = (int[])father.Clone();
        for (int i = leftIndex; i <= rightIndex; i++)
        {
            solution[i] = mother[i];
            tmpFather[Find(father, mother[i])] = -1;
        }

        int fatherIndex = (rightIndex + 1) % solution.Length;
        int solutionIndex = (rightIndex + 1) % solution.Length;
        while (solutionIndex != leftIndex)
        {
            while (tmpFather[fatherIndex] == -1)
                fatherIndex = (fatherIndex + 1) % solution.Length;

            solution[solutionIndex] = tmpFather[fatherIndex];
            solutionIndex = (solutionIndex + 1) % solution.Length;
            fatherIndex = (fatherIndex + 1) % solution.Length;
        }

        for (int i = 0; i < solution.Length; i++)
        {
            if (solution[i] == -1)
                throw new System.Exception("OX Crossover went horribly wrong");
        }

        return solution;
    }

    private static float[] OXCrossover(float[] mother, float[] father)
    {
        var solution = new float[mother.Length];
        for (int i = 0; i < solution.Length; i++)
            solution[i] = -1f;
        int leftIndex = Random.Range(0, solution.Length);
        int rightIndex = Random.Range(0, solution.Length);
        if (rightIndex < leftIndex)
        {
            int tmp = leftIndex;
            leftIndex = rightIndex;
            rightIndex = tmp;
        }

        //Fill rest with father alleles
        float[] tmpFather = (float[])father.Clone();
        for (int i = leftIndex; i <= rightIndex; i++)
        {
            solution[i] = mother[i];
            tmpFather[Find(father, mother[i])] = -1;
        }

        int fatherIndex = (rightIndex + 1) % solution.Length;
        int solutionIndex = (rightIndex + 1) % solution.Length;
        while (solutionIndex != leftIndex)
        {
            while (tmpFather[fatherIndex] < 0f) // the same as tmpFather[fatherIndex] == -1f but no float precision dilema
                fatherIndex = (fatherIndex + 1) % solution.Length;

            solution[solutionIndex] = tmpFather[fatherIndex];
            solutionIndex = (solutionIndex + 1) % solution.Length;
            fatherIndex = (fatherIndex + 1) % solution.Length;
        }

        for (int i = 0; i < solution.Length; i++)
        {
            if (solution[i] == -1)
                throw new System.Exception("OX Crossover went horribly wrong");
        }

        return solution;
    }

    private static Vector3[] OXCrossover(Vector3[] mother, Vector3[] father)
    {
        var solution = new Vector3?[mother.Length];
        for (int i = 0; i < solution.Length; i++)
            solution[i] = null;
        int leftIndex = Random.Range(0, solution.Length);
        int rightIndex = Random.Range(0, solution.Length);
        if (rightIndex < leftIndex)
        {
            int tmp = leftIndex;
            leftIndex = rightIndex;
            rightIndex = tmp;
        }

        //Fill rest with father alleles
        Vector3?[] tmpFather = (Vector3?[])father.Clone();
        for (int i = leftIndex; i <= rightIndex; i++)
        {
            solution[i] = mother[i];
            tmpFather[Find(father, mother[i])] = null;
        }

        int fatherIndex = (rightIndex + 1) % solution.Length;
        int solutionIndex = (rightIndex + 1) % solution.Length;
        while (solutionIndex != leftIndex)
        {
            while (tmpFather[fatherIndex] == null)
                fatherIndex = (fatherIndex + 1) % solution.Length;

            solution[solutionIndex] = tmpFather[fatherIndex];
            solutionIndex = (solutionIndex + 1) % solution.Length;
            fatherIndex = (fatherIndex + 1) % solution.Length;
        }

        for (int i = 0; i < solution.Length; i++)
        {
            if (solution[i] == null)
                throw new System.Exception("OX Crossover went horribly wrong");
        }

        return solution.Select(p => p.Value).ToArray();
    }

    /// <summary>
    /// Swap two randomly picked genes in solution
    /// </summary>
    private static T[] SwapMutation<T>(T[] solution)
    {
        int gene1Index = Random.Range(0, solution.Length);
        int gene2Index = Random.Range(0, solution.Length);
        var tmp = solution[gene1Index];
        solution[gene1Index] = solution[gene2Index];
        solution[gene2Index] = tmp;

        return solution;
    }

    /// <summary>
    /// Shuffle genes in a random swath in solution
    /// </summary>
    private static T[] ScrambleMutation<T>(T[] solution)
    {
        int gene1Index = Random.Range(0, solution.Length);
        int gene2Index = Random.Range(0, solution.Length);
        if (gene1Index > gene2Index)
        {
            int tmp = gene1Index;
            gene1Index = gene2Index;
            gene2Index = tmp;
        }
        int arrayToShuffleLength = gene2Index - gene1Index + 1;
        T[] arrayToShuffle = new T[arrayToShuffleLength];
        System.Array.Copy(solution, gene1Index, arrayToShuffle, 0, arrayToShuffleLength);
        RandomExtension.Shuffle<T>(arrayToShuffle);
        System.Array.Copy(arrayToShuffle, 0, solution, gene1Index, arrayToShuffleLength);

        return solution;
    }

    /// <summary>
    /// Inverse genes in rando swath in solution
    /// </summary>
    private static T[] InversionMutation<T>(T[] solution)
    {
        int gene1Index = Random.Range(0, solution.Length);
        int gene2Index = Random.Range(0, solution.Length);
        if (gene1Index > gene2Index)
        {
            int tmp = gene1Index;
            gene1Index = gene2Index;
            gene2Index = tmp;
        }
        int arrayToInverseLength = gene2Index - gene1Index + 1;
        T[] swath = new T[arrayToInverseLength];
        System.Array.Copy(solution, gene1Index, swath, 0, arrayToInverseLength);
        for (int i = gene1Index, j = swath.Length - 1; j >= 0; i++, j--)
            solution[i] = swath[j];

        return solution;
    }

    /// <summary>
    /// Return index of found argument in solution, else -1
    /// </summary>
    /// <param name="thing">Number to look in Individual (solution)</param>
    /// <returns></returns>
    private static int Find<T>(T[] solution, T thing)
    {
        for (int i = 0; i < solution.Length; i++)
        {
            if (solution[i].Equals(thing))
                return i;
        }
        return -1;
    }
}
