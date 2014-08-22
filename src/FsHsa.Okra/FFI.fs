// ---------------------------------------------------------------------------
// Copyright (c) 2014, Zoltan Podlovics, KP-Tech Kft. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0. See LICENSE.md in the 
// project root for license information.
// ---------------------------------------------------------------------------
// Portions of Copyright (c) FSharp Powerpack Authors.
//
// This work is partially based FSharp Powerpack which is also licensed under 
// the Apache License, Version 2.0.
// ---------------------------------------------------------------------------

module KPTech.FsHsa.Okra.FFI

#nowarn "9"

open System.Runtime.InteropServices
open System.Threading
open System
open Microsoft.FSharp.NativeInterop

module OpsNative = 
    [<NoDynamicInvocation>]
    let inline pinObjUnscoped (obj: obj) =  GCHandle.Alloc(obj,GCHandleType.Pinned) 
    [<NoDynamicInvocation>]
    let inline pinObj (obj: obj) f = 
        let gch = pinObjUnscoped obj 
        try f gch
        finally
            gch.Free()
        
type ArrayNative<'T when 'T : unmanaged>(ptr : nativeptr<'T>, len: int) =
    member x.Ptr = ptr
    [<NoDynamicInvocation>]
    member inline x.Item 
       with get n = NativePtr.get x.Ptr n
       and  set n v = NativePtr.set x.Ptr n v
    member x.Length = len
       
[<Sealed>]
type PinnedArray<'T when 'T : unmanaged>(narray: ArrayNative<'T>, gch: GCHandle) =
    [<VolatileField>]
    let mutable disposed = 0

    [<NoDynamicInvocation>]
    static member inline ofArray(arr: 'T[]) =
        let gch = OpsNative.pinObjUnscoped (box arr) 
        let ptr = &&arr.[0]
        new PinnedArray<'T>(new ArrayNative<_>(ptr,Array.length arr),gch)

    member x.Ptr = narray.Ptr
    member x.Addr = NativePtr.toNativeInt narray.Ptr
    member x.Free() = 
        if Interlocked.CompareExchange(&disposed, 1, 0) = 0 then    
            gch.Free()
    member x.Length = narray.Length
    member x.NativeArray = narray
    interface System.IDisposable with         
        member x.Dispose() = 
            if Interlocked.CompareExchange(&disposed, 1, 0) = 0 then
                gch.Free()
                
type IOkraRef = abstract Ptr : nativeint with get               