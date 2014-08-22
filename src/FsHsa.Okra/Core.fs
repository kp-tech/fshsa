// ---------------------------------------------------------------------------
// Copyright (c) 2014, Zoltan Podlovics, KP-Tech Kft. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0. See LICENSE.md in the 
// project root for license information.
// ---------------------------------------------------------------------------

module KPTech.FsHsa.Okra.Core
#nowarn "9"

open System.Runtime.InteropServices
open System.Threading

module OD = OkraDomain
module N = Native
module S = Support

/// <summary>
/// create an Okra context
/// </summary>
/// <param name="()">unit</param>
/// <returns>context okraoption</returns>
let createContext () = 
    let mutable contextId = 0UL in
    let result = N.getContextNative &contextId in
    let context = OD.OkraContext contextId in
    if 0 = result then
        OD.OkraSome context
    else
        let okraStatusEnum = S.okraStatusIntToType result in
        OD.OkraNone (S.okraStatusNativeToOkraError okraStatusEnum, S.okraStatusNativeToString okraStatusEnum)
    
/// <summary>
/// create an Okra kernel
/// </summary>
/// <param name="kernelSource">string representation of the kernel</param>
/// <param name="methodName">kernel main method name Note: the name must start with '&'</param>
/// <param name="context">Okra context</param>
/// <returns>kernel okraoption</returns>
let createKernel (kernelSource: string) (methodName: string) (context: OD.OkraContext) =
    let (OD.OkraContext(contextId)) = context in
    let mutable kernelId = 0UL in
    let result = N.createKernelNative contextId kernelSource (methodName) &kernelId in
    let kernel = OD.OkraKernel kernelId in
    if 0 = result then
        OD.OkraSome kernel
    else
        let okraStatusEnum = S.okraStatusIntToType result in
        OD.OkraNone (S.okraStatusNativeToOkraError okraStatusEnum, S.okraStatusNativeToString okraStatusEnum)


/// <summary>
/// dispose an Okra context
/// </summary>
/// <returns>kernel okraoption</returns>
let disposeContext (context: OD.OkraContext) =
    let (OD.OkraContext(contextId)) = context in
    let result = N.disposeContextNative contextId in
    if 0 = result then
        OD.OkraSome OD.OkraSuccess
    else
        let okraStatusEnum = S.okraStatusIntToType result in
        OD.OkraNone (S.okraStatusNativeToOkraError okraStatusEnum, S.okraStatusNativeToString okraStatusEnum)


/// <summary>
/// clear Okra kernel args
/// </summary>
/// <param name="kernel">Okra kernel</param>
/// <returns>okrasuccess okraoption</returns>
let clearArgs (kernel: OD.OkraKernel) =
    let (OD.OkraKernel(kernelId)) = kernel in
    let result = N.clearArgsNative kernelId in
    if 0 = result then
        OD.OkraSome OD.OkraSuccess
    else
        let okraStatusEnum = S.okraStatusIntToType result in
        OD.OkraNone (S.okraStatusNativeToOkraError okraStatusEnum, S.okraStatusNativeToString okraStatusEnum)


/// <summary>
/// push bool Okra kernel arg
/// </summary>
/// <param name="kernel">Okra kernel</param>
/// <param name="value">bool value</param>
/// <param name="context">Okra context</param>
/// <returns>okrasuccess okraoption</returns>
let pushBool (kernel: OD.OkraKernel) (value: bool) (context: OD.OkraContext) =
    let (OD.OkraContext(contextId)) = context in
    let (OD.OkraKernel(kernelId)) = kernel in
    let byteValue = if value then 0uy else 1uy in
    let result = N.pushBooleanNative kernelId byteValue in
    if 0 = result then
        OD.OkraSome OD.OkraSuccess
    else
        let okraStatusEnum = S.okraStatusIntToType result in
        OD.OkraNone (S.okraStatusNativeToOkraError okraStatusEnum, S.okraStatusNativeToString okraStatusEnum)


/// <summary>
/// push byte Okra kernel arg
/// </summary>
/// <param name="kernel">Okra kernel</param>
/// <param name="value">byte value</param>
/// <param name="context">Okra context</param>
/// <returns>okrasuccess okraoption</returns>
let pushByte (kernel: OD.OkraKernel) (value: byte) (context: OD.OkraContext) =
    let (OD.OkraContext(contextId)) = context in
    let (OD.OkraKernel(kernelId)) = kernel in
    let result = N.pushByteNative kernelId value in
    if 0 = result then
        OD.OkraSome OD.OkraSuccess
    else
        let okraStatusEnum = S.okraStatusIntToType result in
        OD.OkraNone (S.okraStatusNativeToOkraError okraStatusEnum, S.okraStatusNativeToString okraStatusEnum)

    
/// <summary>
/// push int Okra kernel arg
/// </summary>
/// <param name="kernel">Okra kernel</param>
/// <param name="value">int value</param>
/// <param name="context">Okra context</param>
/// <returns>okrasuccess okraoption</returns>
let pushInt (kernel: OD.OkraKernel) (value: int) (context: OD.OkraContext) =
    let (OD.OkraContext(contextId)) = context in
    let (OD.OkraKernel(kernelId)) = kernel in
    let result = N.pushIntNative kernelId value in
    if 0 = result then
        OD.OkraSome OD.OkraSuccess
    else
        let okraStatusEnum = S.okraStatusIntToType result in
        OD.OkraNone (S.okraStatusNativeToOkraError okraStatusEnum, S.okraStatusNativeToString okraStatusEnum)


/// <summary>
/// push int64 Okra kernel arg
/// </summary>
/// <param name="kernel">Okra kernel</param>
/// <param name="value">int64 value</param>
/// <param name="context">Okra context</param>
/// <returns>okrasuccess okraoption</returns>
let pushInt64 (kernel: OD.OkraKernel) (value: int64) (context: OD.OkraContext) =
    let (OD.OkraContext(contextId)) = context in
    let (OD.OkraKernel(kernelId)) = kernel in
    let result = N.pushLongNative kernelId value in
    if 0 = result then
        OD.OkraSome OD.OkraSuccess
    else
        let okraStatusEnum = S.okraStatusIntToType result in
        OD.OkraNone (S.okraStatusNativeToOkraError okraStatusEnum, S.okraStatusNativeToString okraStatusEnum)


/// <summary>
/// push float32 Okra kernel arg
/// </summary>
/// <param name="kernel">Okra kernel</param>
/// <param name="value">float32 value</param>
/// <param name="context">Okra context</param>
/// <returns>okrasuccess okraoption</returns>
let pushFloat32 (kernel: OD.OkraKernel) (value: float32) (context: OD.OkraContext) =
    let (OD.OkraContext(contextId)) = context in
    let (OD.OkraKernel(kernelId)) = kernel in
    let result = N.pushFloatNative kernelId value in
    if 0 = result then
        OD.OkraSome OD.OkraSuccess
    else
        let okraStatusEnum = S.okraStatusIntToType result in
        OD.OkraNone (S.okraStatusNativeToOkraError okraStatusEnum, S.okraStatusNativeToString okraStatusEnum)


/// <summary>
/// push float Okra kernel arg
/// </summary>
/// <param name="kernel">Okra kernel</param>
/// <param name="value">float value</param>
/// <param name="context">Okra context</param>
/// <returns>okrasuccess okraoption</returns>
let pushFloat (kernel: OD.OkraKernel) (value: float) (context: OD.OkraContext) =
    let (OD.OkraContext(contextId)) = context in
    let (OD.OkraKernel(kernelId)) = kernel in
    let result = N.pushDoubleNative kernelId value in
    if 0 = result then
        OD.OkraSome OD.OkraSuccess
    else
        let okraStatusEnum = S.okraStatusIntToType result in
        OD.OkraNone (S.okraStatusNativeToOkraError okraStatusEnum, S.okraStatusNativeToString okraStatusEnum)

    
/// <summary>
/// push pointer Okra kernel arg
/// </summary>
/// <param name="kernel">Okra kernel</param>
/// <param name="value">pointer value</param>
/// <param name="context">Okra context</param>
/// <returns>okrasuccess okraoption</returns>
let pushPointer (kernel: OD.OkraKernel) (value: nativeint) (context: OD.OkraContext) =
    let (OD.OkraContext(contextId)) = context in
    let (OD.OkraKernel(kernelId)) = kernel in
    let result = N.pushPointerNative kernelId value in
    if 0 = result then
        OD.OkraSome OD.OkraSuccess
    else
        let okraStatusEnum = S.okraStatusIntToType result in
        OD.OkraNone (S.okraStatusNativeToOkraError okraStatusEnum, S.okraStatusNativeToString okraStatusEnum)


/// <summary>
/// execute Okra kernel
/// </summary>
/// <param name="kernel">Okra kernel</param>
/// <param name="range">kernel range parameter</param>
/// <param name="context">Okra context</param>
/// <returns>okrasuccess okraoption</returns>
let executeKernel (kernel: OD.OkraKernel) (range: OD.OkraRange) (context: OD.OkraContext) =
    let (OD.OkraContext(contextId)) = context in
    let (OD.OkraKernel(kernelId)) = kernel in
    use rangeNative = new S.OkraRangePtr(range) in
    let result = N.executeKernelNative contextId kernelId rangeNative.Ptr in
    if 0 = result then
        OD.OkraSome OD.OkraSuccess
    else
        let okraStatusEnum = S.okraStatusIntToType result in
        OD.OkraNone (S.okraStatusNativeToOkraError okraStatusEnum, S.okraStatusNativeToString okraStatusEnum)

    
/// <summary>
/// dispose Okra kernel
/// </summary>
/// <param name="kernel">Okra kernel</param>
/// <returns>okrasuccess okraoption</returns>
let disposeKernel (kernel: OD.OkraKernel) =
    let (OD.OkraKernel(kernelId)) = kernel in
    let result = N.disposeKernelNative kernelId in
    if 0 = result then
        OD.OkraSome OD.OkraSuccess
    else
        let okraStatusEnum = S.okraStatusIntToType result in
        OD.OkraNone (S.okraStatusNativeToOkraError okraStatusEnum, S.okraStatusNativeToString okraStatusEnum)
            