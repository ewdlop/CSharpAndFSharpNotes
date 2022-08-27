<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	string s ="FAREMVIEL";
	string t ="FARMVILLE";
	JaroWinkler.Similarity(s,t).Dump();
	Jaro.JaroWinklerDistance(s,t).Dump();
}




public class JaroWinkler
{
	public enum JWStringComparison { IgnoreCase, Case }
	private const double DEFAULT_THRESHOLD = 0.7;
	private const int THREE = 3;
	private const double JW_COEF = 0.1;
	private readonly JWStringComparison stringComparison;

	/// <summary>
	/// The current value of the threshold used for adding the Winkler bonus. The default value is 0.7.
	/// </summary>
	private double Threshold { get; }

	/// <summary>
	/// Creates a new instance with default threshold (0.7)
	/// </summary>
	public JaroWinkler(JWStringComparison stringComparison)
	{
		Threshold = DEFAULT_THRESHOLD;
	}

	/// <summary>
	/// Creates a new instance with given threshold to determine when Winkler bonus should
	/// be used. Set threshold to a negative value to get the Jaro distance.
	/// </summary>
	/// <param name="threshold"></param>
	public JaroWinkler(double threshold, JWStringComparison stringComparison)
	{
		Threshold = threshold;
		this.stringComparison = stringComparison;
	}

	/// <summary>
	/// Compute Jaro-Winkler similarity.
	/// </summary>
	/// <param name="s1">The first string to compare.</param>
	/// <param name="s2">The second string to compare.</param>
	/// <returns>The Jaro-Winkler similarity in the range [0, 1]</returns>
	/// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
	public double Similarity(string s1, string s2)
	{
		if (s1 == null)
		{
			throw new ArgumentNullException(nameof(s1));
		}

		if (s2 == null)
		{
			throw new ArgumentNullException(nameof(s2));
		}

		if (s1.Equals(s2))
		{
			return 1f;
		}
		if (stringComparison == JWStringComparison.IgnoreCase)
		{
			s1 = s1.ToUpper();
			s2 = s2.ToUpper();
		}

		int[] mtp = Matches(s1, s2);
		float m = mtp[0];
		if (m == 0)
		{
			return 0f;
		}
		double j = (m / s1.Length + m / s2.Length + (m - mtp[1]) / m)
				 / THREE;
		double jw = j;

		if (j > Threshold)
		{
			jw = j + Math.Min(JW_COEF, 1.0 / mtp[THREE]) * mtp[2] * (1 - j);
		}
		return jw;
	}
	public static double Similarity(string s1, string s2, double threshold = DEFAULT_THRESHOLD, JWStringComparison stringComparison = JWStringComparison.IgnoreCase)
	{
		if (s1 == null)
		{
			throw new ArgumentNullException(nameof(s1));
		}

		if (s2 == null)
		{
			throw new ArgumentNullException(nameof(s2));
		}

		if (s1.Equals(s2))
		{
			return 1f;
		}
		if (stringComparison == JWStringComparison.IgnoreCase)
		{
			s1 = s1.ToUpper();
			s2 = s2.ToUpper();
		}

		int[] mtp = Matches(s1, s2);
		float m = mtp[0];
		if (m == 0)
		{
			return 0f;
		}
		double j = (m / s1.Length + m / s2.Length + (m - mtp[1]) / m)
				 / THREE;
		double jw = j;

		if (j > threshold)
		{
			jw = j + Math.Min(JW_COEF, 1.0 / mtp[THREE]) * mtp[2] * (1 - j);
		}
		return jw;
	}
	/// <summary>
	/// Return 1 - similarity.
	/// </summary>
	/// <param name="s1">The first string to compare.</param>
	/// <param name="s2">The second string to compare.</param>
	/// <returns>1 - similarity</returns>
	/// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
	public double Distance(string s1, string s2)
		 => 1.0 - Similarity(s1, s2);

	private static int[] Matches(string s1, string s2)
	{
		string max, min;
		if (s1.Length > s2.Length)
		{
			max = s1;
			min = s2;
		}
		else
		{
			max = s2;
			min = s1;
		}
		int range = Math.Max(max.Length / 2 - 1, 0);

		//int[] matchIndexes = new int[min.Length];
		//Arrays.fill(matchIndexes, -1);
		int[] match_indexes = Enumerable.Repeat(-1, min.Length).ToArray();

		bool[] match_flags = new bool[max.Length];
		int matches = 0;
		for (int mi = 0; mi < min.Length; mi++)
		{
			char c1 = min[mi];
			for (int xi = Math.Max(mi - range, 0),
					  xn = Math.Min(mi + range + 1, max.Length); xi < xn; xi++)
			{
				if (!match_flags[xi] && c1 == max[xi])
				{
					match_indexes[mi] = xi;
					match_flags[xi] = true;
					matches++;
					break;
				}
			}
		}
		char[] ms1 = new char[matches];
		char[] ms2 = new char[matches];
		for (int i = 0, si = 0; i < min.Length; i++)
		{
			if (match_indexes[i] != -1)
			{
				ms1[si] = min[i];
				si++;
			}
		}
		for (int i = 0, si = 0; i < max.Length; i++)
		{
			if (match_flags[i])
			{
				ms2[si] = max[i];
				si++;
			}
		}
		int transpositions = 0;
		for (int mi = 0; mi < ms1.Length; mi++)
		{
			if (ms1[mi] != ms2[mi])
			{
				transpositions++;
			}
		}
		int prefix = 0;
		for (int mi = 0; mi < min.Length; mi++)
		{
			if (s1[mi] == s2[mi])
			{
				prefix++;
			}
			else
			{
				break;
			}
		}
		return new[] { matches, transpositions / 2, prefix, max.Length };
	}
	
}

public static class Jaro
{
    public static double JaroWinklerDistance(this string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        if (n == 0) return m == 0 ? 1.0 : 0.0;
        if (m == 0) return n == 0 ? 1.0 : 0.0;
        int halflen = Math.Max(0, Math.Min(n, m) / 2 - 1);
        bool[] sflags = new bool[n];
        bool[] tflags = new bool[m];
        int matches = 0;
        for (int i = 0; i < n; i++)
        {
            int low = Math.Max(0, i - halflen);
            int high = Math.Min(i + halflen + 1, m);
            for (int j = low; j < high; j++)
            {
                if (!tflags[j] && t[j] == s[i])
                {
                    sflags[i] = true;
                    tflags[j] = true;
                    matches++;
                    break;
                }
            }
        }
        if (matches == 0) return 0.0;
        //int k = 0;
        //for (int i = 0; i < n; i++)
        //{
        //    if (sflags[i])
        //    {
        //        k++;
        //    }

        //}

        //for (int i = 0; i < m; i++)
        //{
        //    if (tflags[i])
        //    {
        //        k++;
        //    }
        //}
        int transpositions = 0;
        for (int i = 0, j = 0; i < n; i++)
        {
            if (sflags[i])
            {
                while (!tflags[j])
                {
                    j++;
                }
                if (s[i] != t[j])
                {
                    transpositions++;
                }
                j++;
            }
        }
        double jaro = 1.0/3 * ((double)matches / n + (double)matches / m + (double)(matches - transpositions / 2) / matches);
		double jaroWinkler = jaro + (double)4 * 0.1 * (1.0 - jaro);
        return jaroWinkler;
    }
}