// ---------------------------------------------------------------------------
// Copyright (c) 2014, Zoltan Podlovics, KP-Tech Kft. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0. See LICENSE.md in the 
// project root for license information.
// ---------------------------------------------------------------------------

module KPTech.FsHsa.Okra.OkraDomain

type OkraContext = OkraContext of uint64
type OkraKernel = OkraKernel of uint64

[<Struct>]
type OkraRange =
    val Dimension: uint32
    val GlobalSize: uint32*uint32*uint32
    val GroupSize: uint32*uint32*uint32
    new(dimension, globalSize, groupSize) = {Dimension=dimension; GlobalSize = globalSize; GroupSize = groupSize}

type OkraSuccess = OkraSuccess

type OkraError =
    | OkraErrorContextNoDeviceFound
    | OkraErrorContextQueueCreationFailed
    | OkraErrorSetMemoryPolicyFailed
    | OkraErrorMemoryRegistrationFailed
    | OkraErrorMemoryDeregistrationFailed
    | OkraErrorContextCreateFailed
    | OkraErrorContextAlreadyExists
    | OkraErrorKernelHsailAssemblingFailed
    | OkraErrorKernelFinalizeFailed
    | OkraErrorKernelCreateFailed
    | OkraErrorKernelElfInitializationFailed
    | OkraErrorKernelInvalidElfContainer
    | OkraErrorKernelInvalidSectionHeader
    | OkraErrorKernelMissingStringSection
    | OkraErrorKernelMissingDirectiveSection
    | OkraErrorKernelMissingCodeSection
    | OkraErrorKernelMissingOperandsSection
    | OkraErrorKernelMissingDebugSection
    | OkraErrorLoadBrigFailed
    | OkraErrorUnloadBrigFailed
    | OkraErrorKernelCreateFromBinaryFailed
    | OkraErrorKernelPushKernargFailed
    | OkraErrorKernelClearargFailed
    | OkraErrorRangeInvalidDimension
    | OkraErrorRangeInvalidGlobalSize
    | OkraErrorRangeInvalidGroupSize
    | OkraErrorExecuteFailed
    | OkraErrorDisposeFailed
    | OkraErrorInvalidArgument
    | OkraErrorUnkown

type OkraOption<'T> = 
    | OkraSome of 'T
    | OkraNone of OkraError*string

