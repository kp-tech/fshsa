// ---------------------------------------------------------------------------
// Copyright (c) 2014, Zoltan Podlovics, KP-Tech Kft. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0. See LICENSE.md in the 
// project root for license information.
// ---------------------------------------------------------------------------

module KPTech.FsHsa.Okra.Support

open System.Runtime.InteropServices
open System.Threading

module OD = OkraDomain
module N = Native
   
let okraStatusNativeToString: (N.OkraStatusNative -> string) = function
    | N.OkraStatusNative.OKRA_SUCCESS -> "Success"
    | N.OkraStatusNative.OKRA_CONTEXT_NO_DEVICE_FOUND -> "No device found"
    | N.OkraStatusNative.OKRA_CONTEXT_QUEUE_CREATION_FAILED -> "Queue creation failed"
    | N.OkraStatusNative.OKRA_SET_MEMORY_POLICY_FAILED -> "Set memory policy failed"
    | N.OkraStatusNative.OKRA_MEMORY_REGISTRATION_FAILED -> "Memory registration failed"
    | N.OkraStatusNative.OKRA_MEMORY_DEREGISTRATION_FAILED -> "Memory deregistration failed"
    | N.OkraStatusNative.OKRA_CONTEXT_CREATE_FAILED -> "Context create failed"
    | N.OkraStatusNative.OKRA_CONTEXT_ALREADY_EXIST -> "Context already exists"
    | N.OkraStatusNative.OKRA_KERNEL_HSAIL_ASSEMBLING_FAILED -> "Kernel HSAIL assembling failed"
    | N.OkraStatusNative.OKRA_KERNEL_FINALIZE_FAILED -> "Kernel finalize failed"
    | N.OkraStatusNative.OKRA_KERNEL_CREATE_FAILED -> "Kernel create failed"
    | N.OkraStatusNative.OKRA_KERNEL_ELF_INITIALIZATION_FAILED -> "Kernel elf initialization failed"
    | N.OkraStatusNative.OKRA_KERNEL_INVALID_ELF_CONTAINER -> "Kernel invalid elf container"
    | N.OkraStatusNative.OKRA_KERNEL_INVALID_SECTION_HEADER -> "Kernel invalid section header"
    | N.OkraStatusNative.OKRA_KERNEL_MISSING_STRING_SECTION -> "Kernel missing string section"
    | N.OkraStatusNative.OKRA_KERNEL_MISSING_DIRECTIVE_SECTION -> "Kernel missing directive section"
    | N.OkraStatusNative.OKRA_KERNEL_MISSING_CODE_SECTION -> "Kernel missing code section"
    | N.OkraStatusNative.OKRA_KERNEL_MISSING_OPERANDS_SECTION -> "Kernel missing operand section"
    | N.OkraStatusNative.OKRA_KERNEL_MISSING_DEBUG_SECTION -> "Kernel missing debug section"
    | N.OkraStatusNative.OKRA_LOAD_BRIG_FAILED -> "Load brig failed"
    | N.OkraStatusNative.OKRA_UNLOAD_BRIG_FAILED -> "Unload brig failed"
    | N.OkraStatusNative.OKRA_KERNEL_CREATE_FROM_BINARY_FAILED -> "Kernel create from binary failed"
    | N.OkraStatusNative.OKRA_KERNEL_PUSH_KERNARG_FAILED -> "Kernel push kernarg failed"
    | N.OkraStatusNative.OKRA_KERNEL_CLEARARG_FAILED -> "Kernel cleararg failed"
    | N.OkraStatusNative.OKRA_RANGE_INVALID_DIMENSION -> "Range invalid dimension"
    | N.OkraStatusNative.OKRA_RANGE_INVALID_GLOBAL_SIZE -> "Range invalid global size"
    | N.OkraStatusNative.OKRA_RANGE_INVALID_GROUP_SIZE -> "Range invalid group size"
    | N.OkraStatusNative.OKRA_EXECUTE_FAILED -> "Execute failed"
    | N.OkraStatusNative.OKRA_DISPOSE_FAILED -> "Dispose failed"
    | N.OkraStatusNative.OKRA_INVALID_ARGUMENT -> "Invalid argument"
    | N.OkraStatusNative.OKRA_UNKNOWN -> "Unknown"
    | _ -> failwith "" "Invalid status"

let okraStatusNativeToOkraError: (N.OkraStatusNative -> OD.OkraError) = function
    | N.OkraStatusNative.OKRA_SUCCESS -> invalidArg "not an error" ""
    | N.OkraStatusNative.OKRA_CONTEXT_NO_DEVICE_FOUND -> OD.OkraErrorContextNoDeviceFound
    | N.OkraStatusNative.OKRA_CONTEXT_QUEUE_CREATION_FAILED -> OD.OkraErrorContextQueueCreationFailed
    | N.OkraStatusNative.OKRA_SET_MEMORY_POLICY_FAILED -> OD.OkraErrorSetMemoryPolicyFailed
    | N.OkraStatusNative.OKRA_MEMORY_REGISTRATION_FAILED -> OD.OkraErrorMemoryRegistrationFailed
    | N.OkraStatusNative.OKRA_MEMORY_DEREGISTRATION_FAILED -> OD.OkraErrorMemoryDeregistrationFailed
    | N.OkraStatusNative.OKRA_CONTEXT_CREATE_FAILED -> OD.OkraErrorContextCreateFailed
    | N.OkraStatusNative.OKRA_CONTEXT_ALREADY_EXIST -> OD.OkraErrorContextAlreadyExists
    | N.OkraStatusNative.OKRA_KERNEL_HSAIL_ASSEMBLING_FAILED -> OD.OkraErrorKernelHsailAssemblingFailed
    | N.OkraStatusNative.OKRA_KERNEL_FINALIZE_FAILED -> OD.OkraErrorKernelFinalizeFailed
    | N.OkraStatusNative.OKRA_KERNEL_CREATE_FAILED -> OD.OkraErrorKernelCreateFailed
    | N.OkraStatusNative.OKRA_KERNEL_ELF_INITIALIZATION_FAILED -> OD.OkraErrorKernelElfInitializationFailed
    | N.OkraStatusNative.OKRA_KERNEL_INVALID_ELF_CONTAINER -> OD.OkraErrorKernelInvalidElfContainer
    | N.OkraStatusNative.OKRA_KERNEL_INVALID_SECTION_HEADER -> OD.OkraErrorKernelInvalidSectionHeader
    | N.OkraStatusNative.OKRA_KERNEL_MISSING_STRING_SECTION -> OD.OkraErrorKernelMissingStringSection
    | N.OkraStatusNative.OKRA_KERNEL_MISSING_DIRECTIVE_SECTION -> OD.OkraErrorKernelMissingDirectiveSection
    | N.OkraStatusNative.OKRA_KERNEL_MISSING_CODE_SECTION -> OD.OkraErrorKernelMissingCodeSection
    | N.OkraStatusNative.OKRA_KERNEL_MISSING_OPERANDS_SECTION -> OD.OkraErrorKernelMissingOperandsSection
    | N.OkraStatusNative.OKRA_KERNEL_MISSING_DEBUG_SECTION -> OD.OkraErrorKernelMissingDebugSection
    | N.OkraStatusNative.OKRA_LOAD_BRIG_FAILED -> OD.OkraErrorLoadBrigFailed
    | N.OkraStatusNative.OKRA_UNLOAD_BRIG_FAILED -> OD.OkraErrorUnloadBrigFailed
    | N.OkraStatusNative.OKRA_KERNEL_CREATE_FROM_BINARY_FAILED -> OD.OkraErrorKernelCreateFromBinaryFailed
    | N.OkraStatusNative.OKRA_KERNEL_PUSH_KERNARG_FAILED -> OD.OkraErrorKernelPushKernargFailed
    | N.OkraStatusNative.OKRA_KERNEL_CLEARARG_FAILED -> OD.OkraErrorKernelClearargFailed
    | N.OkraStatusNative.OKRA_RANGE_INVALID_DIMENSION -> OD.OkraErrorRangeInvalidDimension
    | N.OkraStatusNative.OKRA_RANGE_INVALID_GLOBAL_SIZE -> OD.OkraErrorRangeInvalidGlobalSize
    | N.OkraStatusNative.OKRA_RANGE_INVALID_GROUP_SIZE -> OD.OkraErrorRangeInvalidGroupSize
    | N.OkraStatusNative.OKRA_EXECUTE_FAILED -> OD.OkraErrorExecuteFailed
    | N.OkraStatusNative.OKRA_DISPOSE_FAILED -> OD.OkraErrorDisposeFailed
    | N.OkraStatusNative.OKRA_INVALID_ARGUMENT -> OD.OkraErrorInvalidArgument
    | N.OkraStatusNative.OKRA_UNKNOWN -> OD.OkraErrorUnkown
    | _ -> failwith "" "Invalid status"


/// <summary>
/// create OkraStatusNative enum from an int value
/// </summary>
/// <param name="v">int value of enum</param>
/// <returns>enum</returns>
let okraStatusIntToType v =
    let (result: N.OkraStatusNative) = LanguagePrimitives.EnumOfValue v
    result

/// <summary>
/// convert OkraRange struct to OkraRangeNative struct
/// the OkraRange is simpler structure than the native one
/// </summary>
/// <param name="x">OkraRange struct</param>
/// <returns>OkraRangeNative</returns>
let convertOkraRangeToOkraRangeNative (x: OD.OkraRange) =
        let (globalSizeX, globalSizeY, globalSizeZ) = x.GlobalSize in
        let (groupSizeX, groupSizeY, groupSizeZ) = x.GroupSize in
        new N.OkraRangeNative(x.Dimension, [|globalSizeX; globalSizeY; globalSizeZ|], [|groupSizeX; groupSizeY; groupSizeZ|], 0u)            

/// <summary>
/// OkraRange native holder type
/// the constructor will convert to native struct which could be used later with the .Ptr access
/// </summary>
/// <param name="range">OkraRange struct</param>
/// <returns>type</returns>
type OkraRangePtr(range : OD.OkraRange) =
    [<VolatileField>]
    let mutable disposed = 0
    let rangeNative = convertOkraRangeToOkraRangeNative range in
    let size = Marshal.SizeOf(typeof<N.OkraRangeNative>) in
    let ptr = Marshal.AllocHGlobal(size) in
    let _ = Marshal.StructureToPtr(rangeNative, ptr, false);

    member x.Ptr = ptr

    interface System.IDisposable with
        member x.Dispose () =
            if Interlocked.CompareExchange(&disposed, 1, 0) = 0 then
                Marshal.FreeHGlobal ptr
            