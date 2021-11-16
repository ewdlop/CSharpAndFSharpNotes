namespace CSharpClassLibrary.Nested;

public abstract class BankAccount
{
    private BankAccount() { } // prevent third-party subclassing.
    private sealed class SavingsAccount : BankAccount {  }
    private sealed class CheckingAccount : BankAccount {  }
    public static BankAccount MakeSavingAccount() { return new SavingsAccount(); }
    public static BankAccount MakeCheckingAccount() { return new CheckingAccount(); }
}