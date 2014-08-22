// ---------------------------------------------------------------------------
// Copyright (c) 2014, Zoltan Podlovics, KP-Tech Kft. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0. See LICENSE.md in the 
// project root for license information.
// ---------------------------------------------------------------------------
// This code is based on the published Squares sample available at:
// https://github.com/HSAFoundation/Okra-Interface-to-HSA-Device/blob/master/okra/samples/src/cpp/Squares/Squares.cpp
//
// Portions of Copyright(c):
//
// University of Illinois/NCSA
// Open Source License
//
// Copyright (c) 2013, Advanced Micro Devices, Inc.
// All rights reserved.
//
// Developed by:
//
// Runtimes Team
//
// Advanced Micro Devices, Inc
//
// www.amd.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal with
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// * Redistributions of source code must retain the above copyright notice,
// this list of conditions and the following disclaimers.
//
// * Redistributions in binary form must reproduce the above copyright notice,
// this list of conditions and the following disclaimers in the
// documentation and/or other materials provided with the distribution.
//
// * Neither the names of the LLVM Team, University of Illinois at
// Urbana-Champaign, nor the names of its contributors may be used to
// endorse or promote products derived from this Software without specific
// prior written permission.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// CONTRIBUTORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS WITH THE
// SOFTWARE.
//===----------------------------------------------------------------------===//

module KPTech.FsHsa.Okra.Sample.Main

open KPTech.FsHsa.Okra.FFI
open KPTech.FsHsa.Okra.Native
open System.Runtime.InteropServices

module OD = KPTech.FsHsa.Okra.OkraDomain
module OC = KPTech.FsHsa.Okra.Core

[<EntryPoint>]
let main argv =     
    printfn "Args: %A" argv 
    let parseNumItems (argv:string array) = 
        let defaultNum = 40 in
        if 1 = argv.Length then
            let (success,argNumItems) = System.Int32.TryParse(argv.[0]) in
            if success then 
                if argNumItems > 2048 then 
                    failwith "max single dimension size is 2048"
                else
                    argNumItems
            else 
                defaultNum
        else
            defaultNum in
    let numItems = parseNumItems argv in
    let unumItems = uint32(numItems) in
    let inArray = Array.init numItems (fun x-> float32(x)) in
    let outArray = Array.create numItems 0.0f in

    let ninArray = PinnedArray.ofArray inArray in
    let noutArray = PinnedArray.ofArray outArray in

    let execute () : unit =
        let contextOpt = OC.createContext() in
        match contextOpt with
        | OD.OkraSome context ->
            let kernelSource = System.IO.File.ReadAllText("Squares.hsail") in
            let kernelOpt = OC.createKernel kernelSource "&run" context in
            match kernelOpt with
            | OD.OkraSome kernel ->
                let clearArgs = OC.clearArgs kernel in
                let pushOutArray = OC.pushPointer kernel noutArray.Addr context in
                let pushInArray = OC.pushPointer kernel ninArray.Addr context in
                match clearArgs, pushOutArray, pushInArray with
                | OD.OkraSome OD.OkraSuccess, OD.OkraSome OD.OkraSuccess, OD.OkraSome OD.OkraSuccess ->
                    let range = new OD.OkraRange(1u,(unumItems,1u,1u),(unumItems,1u,1u)) in
                    let execute = OC.executeKernel kernel range context in
                    match execute with
                    | OD.OkraSome OD.OkraSuccess ->
                        Array.iteri (fun i x -> printfn "[%d]:%f" i outArray.[i]) outArray;
                        let disposeKernel = OC.disposeKernel kernel in
                        let disposeContext = OC.disposeContext context in
                        match disposeKernel, disposeContext with
                        | OD.OkraSome OD.OkraSuccess, OD.OkraSome OD.OkraSuccess ->
                            printfn "dispose finished\n";
                        | OD.OkraNone (c,m), _| _, OD.OkraNone (c,m) ->
                            failwith ("failed to dispose the kernel: " + m)
                    | OD.OkraNone (c,m) ->
                        failwith ("failed to execute the kernel: " + m)
                | OD.OkraNone (c,m), _, _ | _, OD.OkraNone (c,m), _ | _, _, OD.OkraNone (c,m) ->
                    failwith ("failed to pass params: " + m)
            | OD.OkraNone (c,m) ->
                failwith ("failed to execute the kernel: " + m)            
        | OD.OkraNone (c,m) ->
            failwith ("failed to execute the kernel: " + m) in

    execute ();
    0 // return an integer exit code

