module Day5

open System
open System.Security.Cryptography
open System.Text

let doorId = "abbhdwsy"

let md5 = MD5.Create()

let hashIsValid (hash:array<byte>) =
    hash.[0] = 0uy && hash.[1] = 0uy && hash.[2] < 16uy

let passwordIsIncomplete password =
    password |> Seq.exists ((=) '_')

let sixthChar (hash:array<byte>) =
    hash.[2].ToString("x2").[1]

let sixthByte (hash:array<byte>) =
    hash.[2] &&& 15uy

let seventhChar (hash:array<byte>) =
    hash.[3].ToString("x2").[0]

let calcHash num =
    md5.ComputeHash(Encoding.UTF8.GetBytes(doorId + string num))

let part1 =
    let passwordChars =
        seq { 0L..Int64.MaxValue }
        |> Seq.map calcHash
        |> Seq.filter hashIsValid
        |> Seq.map sixthChar
        |> Seq.take 8
        |> Seq.toArray
    new String(passwordChars)

let part2 =
    let passwordPositions =
        seq { 0L..Int64.MaxValue }
        |> Seq.map calcHash
        |> Seq.filter hashIsValid
        |> Seq.map (fun hash -> (sixthByte hash, seventhChar hash))
        |> Seq.filter (fun (pos, _) -> pos < 8uy)
    let passwords = seq {
        let password = Array.create 8 '_'
        for (pos, c) in passwordPositions ->
            if password.[int pos] = '_' then Array.set password (int pos) c
            password
    }
    let passwordChars =
        passwords
        |> Seq.skipWhile passwordIsIncomplete
        |> Seq.head
    new String(passwordChars)
