using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace BenchmarkApp;

public static class SplitterMud
{
    private const string NextBoundary = ".*?\\b";

    private static StringBuilder? s_stringBuilderCached;

    /// <summary>
    /// Splits the text into fragments, according to the
    /// text to be highlighted
    /// </summary>
    /// <param name="text">The whole text</param>
    /// <param name="highlightedText">The text to be highlighted</param>
    /// <param name="highlightedTexts">The texts to be highlighted</param>
    /// <param name="regex">Regex expression that was used to split fragments.</param>
    /// <param name="caseSensitive">Whether it's case sensitive or not</param>
    /// <param name="untilNextBoundary">If true, splits until the next regex boundary</param>
    /// <returns></returns>
    public static Memory<string> GetFragments(string? text,
                                                   string? highlightedText,
                                                   IEnumerable<string>? highlightedTexts,
                                                   out string regex,
                                                   bool caseSensitive = false,
                                                   bool untilNextBoundary = false)
    {
        if (string.IsNullOrEmpty(text))
        {
            regex = string.Empty;
            return Memory<string>.Empty;
        }

        var builder = Interlocked.Exchange(ref s_stringBuilderCached, null) ?? new();
        //the first brace in the pattern is to keep the patten when splitting,
        //the `(?:` in the pattern is to accept multiple highlightedTexts but not capture them.
        builder.Append("((?:");

        //this becomes true if `AppendPattern` was called at least once.
        bool hasAtLeastOnePattern = false;
        if (!string.IsNullOrEmpty(highlightedText))
        {
            AppendPattern(highlightedText);
        }

        if (highlightedTexts is not null)
        {
            foreach (var substring in highlightedTexts)
            {
                if (string.IsNullOrEmpty(substring))
                    continue;

                //split pattern if we already added an string to search.
                if (hasAtLeastOnePattern)
                {
                    builder.Append(")|(?:");
                }

                AppendPattern(substring);
            }
        }

        if (hasAtLeastOnePattern)
        {
            //close the last pattern group and the capture group.
            builder.Append("))");
        }
        else
        {
            builder.Clear();
            s_stringBuilderCached = builder;

            //all patterns were empty or null.
            regex = string.Empty;
            return new[] { text };
        }

        regex = builder.ToString();
        builder.Clear();
        s_stringBuilderCached = builder;

        var splits = Regex
                .Split(text,
                       regex,
                       caseSensitive
                         ? RegexOptions.None
                         : RegexOptions.IgnoreCase);
        var length = 0;
        for (var i = 0; i < splits.Length; i++)
        {
            var s = splits[i];
            if (!string.IsNullOrEmpty(s))
            {
                splits[length++] = s;
            }
        }
        Array.Clear(splits, length, splits.Length - length);
        return splits.AsMemory(0, length);

        void AppendPattern(string value)
        {
            hasAtLeastOnePattern = true;
            //escapes the text for regex
            value = Regex.Escape(value);
            builder.Append(value);
            if (untilNextBoundary)
            {
                builder.Append(NextBoundary);
            }
        }
    }
}
public static class Splitter
{
    private const string NextBoundary = ".*?\\b";
    /// <summary>
    /// Splits the text into fragments, according to the
    /// text to be highlighted
    /// </summary>
    /// <param name = "text">The whole text</param>
    /// <param name = "highlightedTexts">The text(s) to be highlighted</param>
    /// <param name = "caseSensitive">Whether it's case sensitive or not</param>
    /// <param name = "untilNextBoundary">If true, splits until the next regex boundary</param>
    /// <returns></returns>
    public static List<string> GetFragments(string text, IReadOnlyList<string> highlightedTexts, bool caseSensitive = false, bool untilNextBoundary = false)
    {
        List<string>? fragements = null;
        if (string.IsNullOrWhiteSpace(text))
        {
            return fragements ??= new List<string>();
        }
        if (!highlightedTexts.Any())
        {
            return fragements ??= new List<string>(1) { text };
        }
        else
        {
            string[] escapedHighlightedTexts = highlightedTexts
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => untilNextBoundary ? $"{Regex.Escape(s)}{NextBoundary}" : Regex.Escape(s))
                .ToArray();
            string regexPattern = $"({string.Join('|', escapedHighlightedTexts)})";
            return Regex.Split(text, regexPattern, caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase)
                .Where(s => !string.IsNullOrEmpty(s)).ToList();

        }
    }
}

[InProcessEmitConfig6]
[MemoryDiagnoser]
public class SplitterTest
{
    private const string LOREM_IPSUM = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";

    
    [Benchmark]
    public void SplitterMudMethod()
    {
        bool anyMatch = false;
        string? _regex = null;
        Memory<string> fragments = SplitterMud.GetFragments("LOREM_IPSUM", null, new string[2] { "printing", "industry" }, out _regex, true, true);
        foreach (string fragment in fragments.Span)
        {
            if (!string.IsNullOrWhiteSpace(_regex)
                && Regex.IsMatch(fragment,
                    _regex,
                        true
                        ? RegexOptions.None
                        : RegexOptions.IgnoreCase))
            {
                anyMatch = true;
            }
        }
    }


    [Benchmark]
    public void SplitterMethod()
    {
        bool anyMatch = false;
        string[]? CustomHighlightedTexts = new string[2] { "printing", "industry" };
        List<string>? fragments = Splitter.GetFragments(LOREM_IPSUM, CustomHighlightedTexts, true, true);
        for (int i = 0; i < fragments.Count; i++)
        {
            for(int j = 0; j < CustomHighlightedTexts.Length; j++)
            {
                if (!string.IsNullOrWhiteSpace(CustomHighlightedTexts[j])
                    && Regex.IsMatch(fragments[i],
                        Regex.Escape(CustomHighlightedTexts[j]), //escape until add a property to accept regex
                            true ? RegexOptions.None : RegexOptions.IgnoreCase))
                {
                    anyMatch = true;
                }
            }

        }
    }
}