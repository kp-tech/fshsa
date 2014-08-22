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


#r "bin/Debug/FsHsa.Okra.dll"
open KPTech.FsHsa.Okra.FFI
open KPTech.FsHsa.Okra.Native
open System.Runtime.InteropServices

let numItems = 40 in
let unumItems = uint32(numItems) in
let inArray = Array.init numItems (fun x-> float32(x)) in
let outArray = Array.create numItems 0.0f in

let ninArray = PinnedArray.ofArray inArray in
let noutArray = PinnedArray.ofArray outArray in

let mutable ctxId = 0UL in
let getContext = getContextNative &ctxId in
let mutable kernelId = 0UL in
let kernelSource = System.IO.File.ReadAllText("Squares.hsail") in
let getKernel = createKernelNative ctxId kernelSource "&run" &kernelId in
let getClearArgs = clearArgsNative kernelId in
let getPushOutArray = pushPointerNative kernelId noutArray.Addr in
let getPushInArray = pushPointerNative kernelId ninArray.Addr in
let range = new OkraRangeNative(1u, [|unumItems;1u;1u|], [|unumItems;1u;1u|], 0u) in
let size = Marshal.SizeOf(typeof<OkraRangeNative>) in
let unmanagedPtr = Marshal.AllocHGlobal(size) in
Marshal.StructureToPtr(range, unmanagedPtr, false);
let getExecuteKernel = executeKernelNative ctxId kernelId unmanagedPtr in
Marshal.FreeHGlobal(unmanagedPtr);
Array.iteri (fun i x -> printfn "[%d]:%f\n" i outArray.[i]) outArray;
let getDisposeKernel = disposeKernelNative(kernelId) in
let getDisposeContext = disposeContextNative(ctxId) in
printfn "end";
