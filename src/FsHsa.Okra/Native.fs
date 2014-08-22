// ---------------------------------------------------------------------------
// Copyright (c) 2014, Zoltan Podlovics, KP-Tech Kft. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0. See LICENSE.md in the 
// project root for license information.
// ---------------------------------------------------------------------------

#nowarn "9"

namespace KPTech.FsHsa.Okra

module Native = begin

    open System.Runtime.InteropServices
    open FFI   

        [<Struct; StructLayout(LayoutKind.Sequential)>]
        type OkraRangeNative =
                val dimension: uint32
                [<field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
                val global_size: uint32 array
                [<field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
                val group_size: uint32 array
                val reserved: uint32
                new(_dimension, _global_size, _group_size, _reserved) = {dimension=_dimension; global_size = _global_size; group_size = _group_size; reserved = _reserved }

        type OkraStatusNative =
            | OKRA_SUCCESS = 0
            | OKRA_CONTEXT_NO_DEVICE_FOUND = 1
            | OKRA_CONTEXT_QUEUE_CREATION_FAILED = 2
            | OKRA_SET_MEMORY_POLICY_FAILED = 3
            | OKRA_MEMORY_REGISTRATION_FAILED = 4
            | OKRA_MEMORY_DEREGISTRATION_FAILED = 5
            | OKRA_CONTEXT_CREATE_FAILED = 6
            | OKRA_CONTEXT_ALREADY_EXIST = 7
            | OKRA_KERNEL_HSAIL_ASSEMBLING_FAILED = 8
            | OKRA_KERNEL_FINALIZE_FAILED = 9
            | OKRA_KERNEL_CREATE_FAILED = 10
            | OKRA_KERNEL_ELF_INITIALIZATION_FAILED = 11
            | OKRA_KERNEL_INVALID_ELF_CONTAINER = 12
            | OKRA_KERNEL_INVALID_SECTION_HEADER = 13
            | OKRA_KERNEL_MISSING_STRING_SECTION = 14
            | OKRA_KERNEL_MISSING_DIRECTIVE_SECTION = 15
            | OKRA_KERNEL_MISSING_CODE_SECTION = 16
            | OKRA_KERNEL_MISSING_OPERANDS_SECTION = 17
            | OKRA_KERNEL_MISSING_DEBUG_SECTION = 18
            | OKRA_LOAD_BRIG_FAILED = 19
            | OKRA_UNLOAD_BRIG_FAILED = 20
            | OKRA_KERNEL_CREATE_FROM_BINARY_FAILED = 21
            | OKRA_KERNEL_PUSH_KERNARG_FAILED = 22
            | OKRA_KERNEL_CLEARARG_FAILED = 23
            | OKRA_RANGE_INVALID_DIMENSION = 24
            | OKRA_RANGE_INVALID_GLOBAL_SIZE = 25
            | OKRA_RANGE_INVALID_GROUP_SIZE = 26
            | OKRA_EXECUTE_FAILED = 27
            | OKRA_DISPOSE_FAILED = 28
            | OKRA_INVALID_ARGUMENT = 29
            | OKRA_UNKNOWN = 30

        let [<Literal>] okraAssemblyName = "libokra_x86_64.so"

        [<DllImport(
            okraAssemblyName,
            EntryPoint="okra_get_context",
            CallingConvention=CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)>]
        extern int (* okra_status_t *) _get_contextNative(
            uint64& context)    
        let inline getContextNative (context: byref<uint64>) =
            _get_contextNative(&context)
        
        [<DllImport(
            okraAssemblyName,
            EntryPoint="okra_create_kernel",
            CallingConvention=CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)>]
        extern int (* okra_status_t *) _create_kernelNative(
            uint64 context,
            string hsailSource,
            string entryName,
            uint64& kernel)
        let inline createKernelNative context hsailSource entryName (kernel: byref<uint64>) =
            _create_kernelNative(context, hsailSource, entryName, &kernel)

        [<DllImport(
            okraAssemblyName,
            EntryPoint="okra_create_kernel_from_binary",
            CallingConvention=CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)>]
        extern int (* okra_status_t *) _create_kernel_from_binaryNative(
            uint64 context,
            string binary,
            nativeint (* size_t *) size,
            string entryName,
            uint64& kernel)
        let inline createKernelFromBinaryNative context binary size entryName (kernel: byref<uint64>) =
            _create_kernel_from_binaryNative(context, binary, size, entryName, &kernel)
        
        [<DllImport(
            okraAssemblyName,
            EntryPoint="okra_push_pointer",
            CallingConvention=CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)>]
        extern int (* okra_status_t *) _push_pointerNative(
            uint64 kernel,
            nativeint address)
        let inline pushPointerNative kernel address =
            _push_pointerNative(kernel, address)
        
        [<DllImport(
            okraAssemblyName,
            EntryPoint="okra_push_boolean",
            CallingConvention=CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)>]
        extern int (* okra_status_t *) _push_booleanNative(
            uint64 kernel,
            byte value)
        let inline pushBooleanNative kernel value =
            _push_booleanNative(kernel, value)


        [<DllImport(
            okraAssemblyName,
            EntryPoint="okra_push_byte",
            CallingConvention=CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)>]
        extern int (* okra_status_t *) _push_byteNative(
            uint64 kernel,
            byte value)
        let inline pushByteNative kernel value =
            _push_byteNative(kernel, value)

        [<DllImport(
            okraAssemblyName,
            EntryPoint="okra_push_double",
            CallingConvention=CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)>]
        extern int (* okra_status_t *) _push_doubleNative(
            uint64 kernel,
            double value)
        let inline pushDoubleNative kernel value =
            _push_doubleNative(kernel, value)
        
        [<DllImport(
            okraAssemblyName,
            EntryPoint="okra_push_float",
            CallingConvention=CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)>]
        extern int (* okra_status_t *) _push_floatNative(
            uint64 kernel,
            float32 value)
        let pushFloatNative kernel value =
            _push_floatNative(kernel, value)
        
        [<DllImport(
            okraAssemblyName,
            EntryPoint="okra_push_int",
            CallingConvention=CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)>]
        extern int (* okra_status_t *) _push_intNative(
            uint64 kernel,
            int value)
        let inline pushIntNative kernel value =
            _push_intNative(kernel, value)
        
        [<DllImport(
            okraAssemblyName,
            EntryPoint="okra_push_long",
            CallingConvention=CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)>]
        extern int (* okra_status_t *) _push_longNative(
            uint64 kernel,
            int64 value)
        let inline pushLongNative kernel value =
            _push_longNative(kernel, value)
        
        [<DllImport(
            okraAssemblyName,
            EntryPoint="okra_clear_args",
            CallingConvention=CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)>]
        extern int (* okra_status_t *) _clear_argsNative(
            uint64 kernel)
        let inline clearArgsNative kernel =
            _clear_argsNative(kernel)
        
        [<DllImport(
            okraAssemblyName,
            EntryPoint="okra_execute_kernel",
            CallingConvention=CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)>]
        extern int (* okra_status_t *) _execute_kernelNative(
            uint64 context,
            uint64 kernel,
            nativeint range)
        let inline executeKernelNative context kernel range =
            _execute_kernelNative (context, kernel, range)
        
        [<DllImport(
            okraAssemblyName,
            EntryPoint="okra_dispose_kernel",
            CallingConvention=CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)>]
        extern int (* okra_status_t *) _dispose_kernelNative(
            uint64 kernel)
        let inline disposeKernelNative kernel =
            _dispose_kernelNative(kernel)


        [<DllImport(
            okraAssemblyName,
            EntryPoint="okra_dispose_context",
            CallingConvention=CallingConvention.Cdecl,
            CharSet=CharSet.Ansi)>]
        extern int (* okra_status_t *) _dispose_contextNative(
            uint64 context)
        let inline disposeContextNative context =
            _dispose_contextNative(context)

    end