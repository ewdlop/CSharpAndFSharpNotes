module MontleCarlo

open System

// Generate a random float between given lower and upper bounds using the provided random generator.
let randomFloatBetween (randomGen : Random) (lowerBound, upperBound) = 
    randomGen.NextDouble() * (upperBound - lowerBound) + lowerBound

// Generate a random sample for each pair of bounds.
let generateRandomSample randomGen (boundPairs: (float*float) array) =
    boundPairs
    |> Array.map (randomFloatBetween randomGen)

// Calculate the volume (in the mathematical sense) of the region defined by the bound pairs.
let calculateVolume (boundPairs: (float*float) array) = 
    boundPairs
    |> Array.map (fun (lowerBound, upperBound) -> upperBound - lowerBound)
    |> Array.reduce (*)

// Monte Carlo Integration Method
let monteCarloIntegration targetFunction (boundPairs: (float*float) array) numberOfSamples =
    let randomGen = Random()
    [|1..numberOfSamples|]
    |> Array.map (fun _ -> generateRandomSample randomGen boundPairs)  // Generate numberOfSamples points
    |> Array.map targetFunction // Evaluate the target function at each sample point
    |> Array.average // Calculate the average of the function values
    |> fun averageValue -> averageValue * (calculateVolume boundPairs)