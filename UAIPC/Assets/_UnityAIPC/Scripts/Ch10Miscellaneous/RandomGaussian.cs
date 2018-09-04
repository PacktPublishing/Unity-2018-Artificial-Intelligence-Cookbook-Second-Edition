using UnityEngine;


public class RandomGaussian
{
    public static ulong seed = 61829450;

    public static float Range()
    {
        double sum = 0;
        for (int i = 0; i < 3; i++)
        {
            ulong holdseed = seed;
            seed ^= seed << 13;
            seed ^= seed >> 17;
            seed ^= seed << 5;
            long r = (long)(holdseed * seed);
            sum += r * (1.0 / 0x7FFFFFFFFFFFFFFF);
        }
        return (float)sum;
    }

    public static float RangeAdditive(params Vector2[] values)
    {
        float sum = 0f;
        int i;
        float min, max;
        if (values.Length == 0)
        {
            values = new Vector2[3];
            for (i = 0; i < values.Length; i++)
                values[i] = new Vector2(0f, 1f);
        }

        for (i = 0; i < values.Length; i++)
        {
            min = values[i].x;
            max = values[i].y;
            sum += Random.Range(min, max);
        }
        return sum;
    }
}
