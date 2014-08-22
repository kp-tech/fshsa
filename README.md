## An F# binding for HSA (Heterogeneous System Architecture)

OKRA is a runtime library that enables applications to do compute offloads to HSA-enabled GPUs. 

FsHsa.Okra can be used to used to do compute offload in a .NET language. The code is all F# but an 
effort has been made to make the library easy to use from C#. See FsHsa.Okra.CSharp.Main. 

The codebase use the Apache 2.0 license.

## Status

The `master` branch is for the latest version of FsHsa.

## Build Requirements

Requires mono 3.0 or higher.  
Requires fsharp 3.1 or higher.  

## Execution Requirements

Requires HSA Linux Drivers.  
Requires HSA Runtime API.  
Requires HSA Okra Library.  
Requires mono 3.0 or higher.  

### Installing HSA Linux Drivers

Follow the install guide that is available at:  
https://github.com/HSAFoundation/HSA-Drivers-Linux-AMD

### Installing HSA Runtime API

Follow the install guide that is available at:  
https://github.com/HSAFoundation/HSA-Runtime-AMD

### Installing HSA Okra Library

Follow the install guide that is available at:  
https://github.com/HSAFoundation/Okra-Interface-to-HSA-Device

Please make sure that the native samples are working properly.

The FsHsa.Okra.dll will try to load the libokra_x86_64.so and dependencies from the current
working directory, so please make sure you symlink every shared library file to from:  
$HSA_INSTALL_DIR/HSA-Drivers-Linux-AMD/kfd-0.8/libhsakmt/lnx64a/libhsakmt.so.1  
$HSA_INSTALL_DIR/HSA-Runtime-AMD/lib/x86_64/libhsa-runtime64.so  
$HSA_INSTALL_DIR/HSA-Runtime-AMD/lib/x86_64/libhsa-runtime64.so.1  
$HSA_INSTALL_DIR/Okra-Interface-to-HSA-Device/okra/dist/bin/libokra_x86_64.so  

## How to Build

### Linux:

Linux/mono: open the FsHsa.sln project file with Monodevelop and build the project. This will
generate the FsHsa.Okra.dll assembly for you.

### OS X

HSA Drivers are not yet available for OS X.

### Windows, using msbuild

HSA Drivers (Beta) are available, not yet tested.

## Development Notes

### Using FsHsa.Okra in your project

In order to use FsHsa.Okra you will want to check the following:

1. Example F# code under FsHsa.Okra.Main.  
2. Example C# code under FsHsa.Okra.CSharp.Main.  
3. The [HSA Standards](http://www.hsafoundation.com/standards/) documentation.  
4. [Okra HSA Device Examples](https://github.com/HSAFoundation/Okra-Interface-to-HSA-Device/tree/master/okra/samples/src/cpp)  
5. [Okra HSAIL Simulator Examples](https://github.com/HSAFoundation/Okra-Interface-to-HSAIL-Simulator/tree/master/samples/src/cpp)  

### Editing the Project with Visual Studio, Xamarin Studio or MonoDevelop

Open `FsHsa.sln`, and edit in modes Debug or Release. 

## How to Test and Validate

### Linux 

After building run
```
cd $FSHSA_PATH/bin
export HSA_INSTALL_DIR=<your_hsa_path>
ln -s $HSA_INSTALL_DIR/HSA-Drivers-Linux-AMD/kfd-0.8/libhsakmt/lnx64a/libhsakmt.so.1 .
ln -s $HSA_INSTALL_DIR/HSA-Runtime-AMD/lib/x86_64/libhsa-runtime64.so .
ln -s $HSA_INSTALL_DIR/HSA-Runtime-AMD/lib/x86_64/libhsa-runtime64.so.1 .
ln -s $HSA_INSTALL_DIR/Okra-Interface-to-HSA-Device/okra/dist/bin/libokra_x86_64.so .
ln -s Release/Squares.hsail .
mono Release/FsHsa.Okra.Sample.exe
mono Release/FsHsa.Okra.CSharp.Sample.exe
```
