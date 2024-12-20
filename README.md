# CSharpAndFSharpNotes

Bunch of C#/ F#/ .Net/ Azure .Net libraries notes

LINQPAD: https://www.linqpad.net/<br>

Q#: https://learn.microsoft.com/en-us/azure/quantum/overview-what-is-qsharp-and-qdk

# What is C#?

C#（发音为 "C Sharp"）是由微软开发的现代、面向对象的编程语言，运行在 .NET 框架上。它被广泛用于开发桌面应用、Web 应用、移动应用、游戏等。

以下是一个简单的 C# 控制台应用程序示例：

```csharp
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Hello, World!");
    }
}
```

此程序输出 "Hello, World!" 到控制台。`using System;` 指令导入 `System` 命名空间，`Main` 方法是程序的入口点。

要开始使用 C#，您可以下载并安装 [Visual Studio 2022](https://visualstudio.microsoft.com/)，这是一个功能强大的集成开发环境（IDE），支持 C# 开发。安装完成后，您可以创建一个新的 C# 控制台应用程序项目，并将上述代码粘贴到 `Program.cs` 文件中，然后运行程序以查看输出。

如果您是 C# 新手，以下资源可帮助您入门：

- [W3Schools 的 C# 教程](https://www.w3schools.com/cs/)：提供全面的 C# 基础知识和示例。
- [Microsoft Learn 的 C# 交互式教程](https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/tutorials/)：提供交互式学习体验，涵盖 C# 的核心概念。
- [Sanfoundry 的 500 多个 C# 程序示例](https://www.sanfoundry.com/csharp-programming-examples/)：涵盖从基础到高级的各种主题的 C# 示例和解决方案。

通过这些资源，您可以深入了解 C# 的语法、数据类型、控制结构、面向对象编程概念等，为开发各种应用程序奠定坚实的基础。

**修正后的句子**：
C#（发音为 "C Sharp"）是由微软开发的现代、面向对象的编程语言，运行在 .NET 框架上。

**中文**：
C#（发音为 "C Sharp"）是由微软开发的现代、面向对象的编程语言，运行在 .NET 框架上。

**正式英文**：
C#, pronounced "C Sharp," is a modern, object-oriented programming language developed by Microsoft that runs on the .NET framework.

**西班牙文**：
C#, pronunciado "C Sharp", es un lenguaje de programación moderno y orientado a objetos desarrollado por Microsoft que se ejecuta en el marco .NET.

**文言文**：
C#，读作 "C Sharp"，乃微软所开发之现代面向对象编程语言，运行于 .NET 框架上。

**Prolog**：
```prolog
language(csharp).
developer(microsoft).
paradigm(object_oriented).
framework(dotnet).
```

**Coq**：
```coq
Definition CSharp : Language :=
  {|
    name := "C#";
    pronunciation := "C Sharp";
    developer := "Microsoft";
    paradigm := ObjectOriented;
    framework := ".NET";
  |}.
```

**关于该主题的数学研究**：
在计算机科学中，编程语言的设计和实现涉及形式语言和自动机理论等数学领域。C# 的类型系统、内存管理和并发模型等特性可以通过数学模型进行分析和验证，以确保语言的可靠性和安全性。例如，类型系统可以使用类型理论来证明程序的正确性，而并发模型可以通过 Petri 网等工具进行建模和分析。

**源链接**：
- [C# 教程 - W3Schools](https://www.w3schools.com/cs/)
- [C# 交互式教程 - Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/tutorials/)
- [500 多个 C# 程序示例 - Sanfoundry](https://www.sanfoundry.com/csharp-programming-examples/)

**生成时间点**：
2024年12月1日，12:30:00（美国东部时间）

**内容输出**：

**Markdown**：
```markdown
C#（发音为 "C Sharp"）是由微软开发的现代、面向对象的编程语言，运行在 .NET 框架上。

**中文**：
C#（发音为 "C Sharp"）是由微软开发的现代、面向对象的编程语言，运行在 .NET 框架上。

**正式英文**：
C#, pronounced "C Sharp," is a modern, object-oriented programming language developed by Microsoft that runs on the .NET framework.

**西班牙文**：
C#, pronunciado "C Sharp", es un lenguaje de programación moderno y orientado a objetos desarrollado por Microsoft que se ejecuta en el marco .NET.

**文言文**：
C#，读作 "C Sharp"，乃微软所开发之现代面向对象编程语言，运行于 .NET 框架上。

**Prolog**：
```prolog
language(csharp).
developer(microsoft).
paradigm(object_oriented).
framework(dotnet).
```

**Coq**：
```coq
Definition CSharp : Language :=
  {|
    name := "C#";
    pronunciation := "C Sharp";
    developer := "Microsoft";
    paradigm := ObjectOriented;
    framework := ".NET";
  |}.
```

**关于该主题的数学研究**：
在计算机科学中，编程语言的设计和实现涉及形式语言和自动机理论等数学领域。C# 的类型系统、内存管理和并发模型等特性可以通过数学模型进行分析和验证，以确保语言的可靠性和安全性。例如，类型系统可以使用类型理论来证明程序的正确性，而并发模型可以通过 Petri 网等工具进行建模和分析。

**源链接**：
- [C# 教程 - W3Schools](https://www.w3schools.com/cs/)
- [C# 交互式教程 - Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/tutorials/)
- [500 多个 C 

# Fiddle

https://dotnetfiddle.net/

F#: https://tryfsharp.fsbolero.io/

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

In most cases, optimizing before fully understanding the code's behavior and requirements leads to unnecessary complications. Focus on clarity, use profiling to identify real bottlenecks, and optimize incrementally to ensure that your efforts are both 
necessary and effective.


### "MyFeed" Nuget Feed For Sandwich Library

<https://pkgs.dev.azure.com/ray810815/Sandwich/_packaging/MyFeed/nuget/v3/index.json>