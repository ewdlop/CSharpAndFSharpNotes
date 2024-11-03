# CSharpAndFSharpNotes

Bunch of C#/F#/.Net/Azure .Net libraries notes<br>
LINQPAD: https://www.linqpad.net/<br>
Q#: https://learn.microsoft.com/en-us/azure/quantum/overview-what-is-qsharp-and-qdk

# Category-theory AND functional programming
https://weblogs.asp.net/dixin/category-theory-via-c-sharp-1-fundamentals-category-object-and-morphism

# Premature optimization

**Premature optimization** is a term that refers to the practice of attempting to improve the efficiency of a program or system **too early in the development process**, before understanding if or where optimization is actually needed. This approach can often lead to increased complexity, more difficult code maintenance, and can even introduce bugs, all without a guaranteed benefit to performance.

Here’s a breakdown of why premature optimization is often discouraged and how to approach it wisely:

### 1. **The Risks of Premature Optimization**

   - **Increased Complexity**: Attempting to optimize early can make the codebase more complex, often involving non-intuitive, “clever” code that’s harder to understand and maintain.
   - **Reduced Flexibility**: Early optimizations often “lock in” specific design choices, making it difficult to adapt the code later on if requirements change.
   - **Wasted Resources**: Optimizing parts of the program that don’t significantly impact overall performance can waste development time and effort. It’s common for only a small percentage of code to impact runtime, so optimizing other parts yields little benefit.
   - **Bug Introduction**: Optimized code can introduce subtle bugs, particularly if the code sacrifices clarity for performance.

### 2. **A Famous Quote on Premature Optimization**

Donald Knuth, a pioneer in computer science, is often quoted on this subject:

> "Premature optimization is the root of all evil (or at least most of it) in programming."  
> — Donald Knuth

Knuth’s quote reflects the notion that optimizing code too early often detracts from the main goal of writing **clear, correct, and maintainable code**.

### 3. **When to Optimize: The 90/10 Rule**

A common guideline in programming is the **90/10 Rule** (or 80/20 Rule), which suggests that **90% of a program's execution time** is typically spent in **10% of the code**. This means it’s usually better to:

   - **Write code for clarity and correctness first**.
   - **Identify bottlenecks** using profiling tools to see where the code spends the most time.
   - **Optimize only the performance-critical sections** based on profiling data, rather than guessing.

### 4. **How to Avoid Premature Optimization**

   - **Focus on Readability and Maintainability**: First and foremost, write code that is clean, understandable, and correct. Ensure that other developers can easily understand and work with it.
   - **Use Profiling Tools**: After the code is working correctly, use profiling tools to measure performance. This helps pinpoint where optimizations would actually make a difference.
   - **Optimize Iteratively**: If a bottleneck is found, optimize it step-by-step and re-profile to measure the impact. This ensures that optimizations are targeted and effective.
   - **Leverage Efficient Algorithms and Data Structures**: Certain choices, like selecting appropriate algorithms and data structures, can naturally lead to efficient code without needing premature optimizations.

### 5. **Examples of Premature Optimization Pitfalls**

   - **Loop Unrolling**: Manually unrolling loops in the hopes of performance gains, even when the loop is not a bottleneck.
   - **Complex Caching Mechanisms**: Adding caching layers or memoization in parts of the code where there’s little measurable impact on runtime.
   - **Avoiding Abstraction**: Writing overly specific code (e.g., using inline code instead of functions) to reduce “function call overhead” when the real bottleneck lies elsewhere.

### 6. **When Optimization Is Justified**

While premature optimization is discouraged, some optimizations may be justified **early on** if:
   - The program has known **real-time requirements** (e.g., video games or high-frequency trading applications).
   - The code involves **processing large datasets** where performance bottlenecks are easily predictable (e.g., matrix multiplication in scientific computing).
   - The team has **prior knowledge** from similar projects about specific bottlenecks.

### Conclusion

In most cases, optimizing before fully understanding the code's behavior and requirements leads to unnecessary complications. Focus on clarity, use profiling to identify real bottlenecks, and optimize incrementally to ensure that your efforts are both necessary and effective.
