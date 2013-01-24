Snappy.Net
======

.NET bindings for Google's compression library: [snappy](http://code.google.com/p/snappy/).  Currently, the native snappy code is compiled into a DLL, and accessed using P/Invoke from the Snappy.Net dll.  At the moment, the solution and projects are only configured to build in Visual Studio 2012, targeting .NET 4.5 and the Visual C++ 2012 runtime.

This project's code is licensed under the [Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0.txt) license.  Original snappy code is licensed under the [New BSD](http://opensource.org/licenses/BSD-3-Clause) license

Version 0.1
======

Note that the current version is in alpha state and the API is subject to change.


Portability
======

Portability is a large goal of this project, and effort has been made to support both 32-bit and 64-bit architectures.  This is somewhat challenging with P/Invoke because of API differences between the 32-bit and 64-bit code of snappy (size_t is used in most public API calls).


Usage
======

Both the x86 and x64 platforms are supported in the current library.