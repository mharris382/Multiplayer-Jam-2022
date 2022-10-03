%%
#scripting #godot #gdnative #c
seealso: [[GDNative C++ Example]], [[GDNative C Example]], [[Cpp-Godot Integration Test]]
%%
# [GDNative demos](https://github.com/godotengine/gdnative-demos#gdnative-demos)

The top level folders split this repo into demos for different languages. Each folder inside of those is a GDNative project. Once compiled, the `project.godot` file can be opened with [Godot Engine](https://godotengine.org/), the open source 2D and 3D game engine.

**Important:** Each demo depends on submodules for the `godot-headers` and `godot-cpp` GDNative dependencies. You should initialize this repository by checking out the submodules recursively:

```
git submodule update --init --recursive
```

For non-GDNative demos (GDScript, VisualScript, and C#), please see the [Godot demo projects](https://github.com/godotengine/godot-demo-projects/) repo.


---


[[Cpp-Godot Integration Test]]

```ad-hint
title: Hint: SConstructFile
collapse: open
This example project also contains a SConstruct file that makes compiling a little easier, but in this tutorial we'll be doing things by hand to understand the process.
```

GDNative can be used to create several types of additions to Godot, using interfaces such as PluginScript or ARVRInterfaceGDNative. In this tutorial we are going to look at creating a NativeScript module. NativeScript allows you to write logic in C or C++ in a similar fashion as you would write a GDScript file. We'll be creating the C equivalent of this GDScript:

```python
extends Reference

var data

func _ready():
    data = "World from GDScript!"

func get_data():
    return data
```

Future tutorials will focus on the other types of GDNative modules and explain when and how to use each of them.

## Prerequisites[¶](https://docs.godotengine.org/en/latest/tutorials/scripting/gdnative/gdnative_c_example.html#prerequisites "Permalink to this headline")

Before we start you'll need a few things:

1.  A Godot executable for your target version.
2.  A C compiler. On Linux, install `gcc` or `clang` from your package manager. On macOS, you can install Xcode from the Mac App Store. On Windows, you can use Visual Studio 2015 or later, or MinGW-w64.
3.  A Git clone of the [godot-headers repository](https://github.com/godotengine/godot-headers.git): these are the C headers for Godot's public API exposed to GDNative.

For the latter, we suggest that you create a dedicated folder for this GDNative example project, open a terminal in that folder and execute:
`git clone https://github.com/godotengine/godot-headers.git`
This will download the required files into that folder.

```ad-tip
collapse: open

If you plan to use Git for your GDNative project, you can also add `godot-headers` as a Git submodule.
```

```ad-note
collapse: open

The `godot-headers` repository has different branches. As Godot evolves, so does GDNative. While we try to preserve compatibility between version, you should always build your GDNative module against headers matching the Godot stable branch (e.g. `3.1`) and ideally actual release (e.g. `3.1.1-stable`) that you use. GDNative modules built against older versions of the Godot headers _may_ work with newer versions of the engine, but not the other way around.
```

The `master` branch of the `godot-headers` repository is kept in line with the `master` branch of Godot and thus contains the GDNative class and structure definitions that will work with the latest development builds.

If you want to write a GDNative module for a stable version of Godot, look at the available Git tags (with `git tags`) for the one matching your engine version. In the `godot-headers` repository, such tags are prefixed with `godot-`, so you can e.g. checkout the `godot-3.1.1-stable` tag for use with Godot 3.1.1. In your cloned repository, you can do:

```
git checkout godot-3.1.1-stable
```
If a tag matching your stable release is missing for any reason, you can fall back to the matching stable branch (e.g. `3.1`), which you would also check out with `git checkout 3.1`.

If you are building Godot from source with your own changes that impact GDNative, you can find the updated class and structure definition in `<godotsource>/modules/gdnative/include`

## Our C source[¶](https://docs.godotengine.org/en/latest/tutorials/scripting/gdnative/gdnative_c_example.html#our-c-source "Permalink to this headline")

Let's start by writing our main code. Eventually, we want to end up with a file structure that looks along those lines:

```
+ <your development folder>
  + godot-headers
    - <lots of files here>
  + simple
    + bin
      - libsimple.dll/so/dylib
      - libsimple.gdnlib
      - simple.gdns
    main.tscn
    project.godot
  + src
    - simple.c
```

Open up Godot and create a new project called "simple" alongside your `godot-headers` Git clone. This will create the `simple` folder and `project.godot` file. Then manually create a `src` folder alongside the `simple` folder, and a `bin` subfolder in the `simple` folder.

We're going to start by having a look at what our `simple.c` file contains. 

Now, for our example here we're making a single C source file without a header to keep things simple. Once you start writing bigger projects it is advisable to break your project up into multiple files. That however falls outside of the scope of this tutorial.

We'll be looking at the source code bit by bit so all the parts below should all be put together into one big file. Each section will be explained as we add it.
```c
#include <gdnative_api_struct.gen.h>

#include <string.h>

const godot_gdnative_core_api_struct *api = NULL;
const godot_gdnative_ext_nativescript_api_struct *nativescript_api = NULL;
```
The above code includes the GDNative API struct header and a standard header that we will use further down for string operations. It then defines two pointers to two different structs. GDNative supports a large collection of functions for calling back into the main Godot executable. In order for your module to have access to these functions, GDNative provides your application with a struct containing pointers to all these functions.

To keep this implementation modular and easily extendable, the core functions are available directly through the "core" API struct, but additional functions have their own "GDNative structs" that are accessible through extensions.

In our example, we access one of these extension to gain access to the functions specifically needed for NativeScript.

A NativeScript behaves like any other script in Godot. Because the NativeScript API is rather low level, it requires the library to specify many things more verbosely than other scripting systems, such as GDScript. When a NativeScript instance gets created, a library-given constructor gets called. When that instance gets destroyed, the given destructor will be executed.

[DEAR GOD THIS IS A LOT RIGHT NOW... Come back to this later](https://docs.godotengine.org/en/latest/tutorials/scripting/gdnative/gdnative_c_example.html#doc-gdnative-c-example)
