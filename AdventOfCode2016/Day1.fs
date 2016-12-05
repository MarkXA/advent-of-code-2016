module Day1

open System
open FsAlg.Generic

let private rotateLeft = matrix [[0.;1.];[-1.;0.]]
let private rotateRight = matrix [[0.;-1.];[1.;0.]]

let private processInput =
    let input = Common.readAsString "Day1.input.txt"
    let instructions =
        input.Split(',')
        |> Array.map (fun s -> s.Trim())
        |> Array.map (fun s -> (s.Substring(0, 1), Convert.ToDouble(s.Substring(1))))
    instructions

let private followInstruction (location, direction:Vector<float>) (rotate, distance:float) =
    let newDirection =
        match rotate with
        | "L" -> direction * rotateLeft
        | "R" -> direction * rotateRight
        | _ -> direction
    let newLocation = location + newDirection * distance
    (newLocation, newDirection)

let private traceForDuplicates (history, found, (location, direction)) (rotate, distance) =
    let result =
        match found with
        | Some _ -> (history, found, (location, direction))
        | None ->
            let (newLocation, newDirection) = followInstruction (location, direction) (rotate, distance)
            if distance > 0. && List.exists (fun v -> Common.vectorEquals v newLocation) history then
                (history, Some newLocation, (newLocation, newDirection))
            else
                (List.append history [newLocation], None, (newLocation, newDirection))
    result

let private blocksAway (location:Vector<float>) =
    int (Math.Abs location.[0] + Math.Abs location.[1])

let part1 = 
    let instructions =
        processInput
        |> Array.collect (fun i -> Array.append [|(fst i, 0.)|] (Array.map (fun _ -> ("N", 1.)) [|1..int (snd i)|]))
    let start = (vector[0.; 0.], vector [0.; 1.])
    let (finalLocation, _) = instructions |> Array.fold followInstruction start
    blocksAway finalLocation

let part2 = 
    let instructions =
        processInput
        |> Array.collect (fun i -> Array.append [|(fst i, 0.)|] (Array.map (fun _ -> ("N", 1.)) [|1..int (snd i)|]))
    let start = (vector[0.; 0.], vector [0.; 1.])
    let (_, found, _) = instructions |> Array.fold traceForDuplicates ([fst start], None, start)
    match found with
        | Some location -> blocksAway location
        | None -> 0
