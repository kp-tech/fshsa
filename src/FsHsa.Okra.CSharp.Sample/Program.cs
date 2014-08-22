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


using System;
using System.Runtime.InteropServices;
using KPTech.FsHsa.Okra;

namespace KPTech.FsHsa.Okra.CSharp.Main
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var numItems = 40;
			if (1 == args.Length) {
				var argNumItems = numItems;
				if (System.Int32.TryParse (args [0], out argNumItems)) {
					numItems = argNumItems;
				}
			}
			var unumItems = (UInt32)numItems;

			var inArray = new float[numItems];
			var outArray = new float[numItems];

			for (int i = 0; i < numItems; i++) {
				inArray [i] = (float)i;
			}

			var inArrayGch = GCHandle.Alloc (inArray, GCHandleType.Pinned);
			var outArrayGch = GCHandle.Alloc (outArray, GCHandleType.Pinned);

			var ninArray = inArrayGch.AddrOfPinnedObject ();
			var noutArray = outArrayGch.AddrOfPinnedObject ();

			var context = 0UL;
			var getContext = Native.getContextNative (ref context);

			if (0 == getContext) {
				var kernelSource = System.IO.File.ReadAllText ("Squares.hsail");
				var kernel = 0UL;
				var getKernel = Native.createKernelNative (context, kernelSource, "&run", ref kernel);

				if (0 == getKernel) {
					var getClearArgs = Native.clearArgsNative (kernel);
					var getPushOutArray = Native.pushPointerNative (kernel, noutArray);
					var getPushInArray = Native.pushPointerNative (kernel, ninArray);

					if ((0 == getClearArgs) && (0 == getPushOutArray) && (0 == getPushInArray)) {
						var rangeGlobalSize = new UInt32[]{ unumItems, 1u, 1u };
						var rangeGroupSize = new UInt32[]{ unumItems, 1u, 1u };
						var range = new Native.OkraRangeNative (1u, rangeGlobalSize, rangeGroupSize, 0u);
						var rangeSize = Marshal.SizeOf (typeof(Native.OkraRangeNative));
						var rangePtr = Marshal.AllocHGlobal (rangeSize);
						Marshal.StructureToPtr (range, rangePtr, false);
						var getExecuteKernel = Native.executeKernelNative (context, kernel, rangePtr);

						if (0 == getExecuteKernel) {
							for (int i = 0; i < numItems; i++) {
								Console.Write ("[");
								Console.Write (i);
								Console.Write ("]: ");
								Console.WriteLine (outArray [i]);
							}
						} else {
							Console.WriteLine ("unable to execute kernel");
						}

						Marshal.FreeHGlobal (rangePtr);
					} else {
						Console.WriteLine ("unable to clear&push args");
					}

					var getDisposeKernel = Native.disposeKernelNative (kernel);
					if (0 == getDisposeKernel) {
					} else {
						Console.WriteLine ("dispose kernel failed");
					}

				} else {
					Console.WriteLine ("unable to get kernel");
				}
				var getDisposeContext = Native.disposeContextNative (context);
				if (0 == getDisposeContext) {
				} else {
					Console.WriteLine ("dispose context failed");
				}
					
				inArrayGch.Free ();
				outArrayGch.Free ();
				Console.WriteLine ("dispose finished");
			} else {
				Console.WriteLine ("unable to get context");
			}
		}
	}
}
