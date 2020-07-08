namespace Quantum.QSharpConsoleApp {

    open Microsoft.Quantum.Convert;
    open Microsoft.Quantum.Math;
    open Microsoft.Quantum.Measurement;
    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Intrinsic;

    operation SampleRandomNumberInRange(max : Int) : Int {
        mutable output = 0; 
        repeat {
            mutable bits = new Result[0]; 
            for (idxBit in 1..BitSizeI(max)) {
                set bits += [GenerateRandomBit()]; 
            }
            set output = ResultArrayAsInt(bits);
        } until (output <= max);
        return output;
    }

    operation GenerateRandomBit() : Result {
        // Allocate a qubit.
        using (q = Qubit()) {
            // Put the qubit to superposition.
            H(q);
            // It now has a 50% chance of being measured 0 or 1.
            // Measure the qubit value.
            return MResetZ(q);
        }
    }

    @EntryPoint()
    operation SampleRandomNumber() : Int {
        let max = 50;
        Message($"Sampling a random number between 0 and {max}: ");
        return SampleRandomNumberInRange(max);
    }
}
