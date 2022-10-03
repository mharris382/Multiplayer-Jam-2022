%%
#scripting #godot #gdnative #c
seealso: [[GDNative C++ Example]], [[GDNative C Example]]
%%
# Cpp Integration Test Example
[link](https://github.com/godotengine/godot-cpp/tree/master/test/src) to cpp integration testing project

## [CMakeList.txt](https://github.com/godotengine/godot-cpp/blob/master/test/CMakeLists.txt)
```cpp
project(godot-cpp-test)
cmake_minimum_required(VERSION 3.6)

set(GODOT_HEADERS_PATH ../godot-headers/ CACHE STRING "Path to Godot headers")
set(CPP_BINDINGS_PATH ../ CACHE STRING "Path to C++ bindings")

if(CMAKE_SYSTEM_NAME STREQUAL "Linux")
	set(TARGET_PATH x11)
elseif(CMAKE_SYSTEM_NAME STREQUAL "Windows")
	set(TARGET_PATH win64)
elseif(CMAKE_SYSTEM_NAME STREQUAL "Darwin")
	set(TARGET_PATH macos)
else()
	message(FATAL_ERROR "Not implemented support for ${CMAKE_SYSTEM_NAME}")
endif()

# Change the output directory to the bin directory
set(BUILD_PATH ${CMAKE_SOURCE_DIR}/bin/${TARGET_PATH})
set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY "${BUILD_PATH}")
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY "${BUILD_PATH}")
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY "${BUILD_PATH}")
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY_DEBUG "${BUILD_PATH}")
SET(CMAKE_RUNTIME_OUTPUT_DIRECTORY_RELEASE "${BUILD_PATH}")
SET(CMAKE_LIBRARY_OUTPUT_DIRECTORY_DEBUG "${BUILD_PATH}")
SET(CMAKE_LIBRARY_OUTPUT_DIRECTORY_RELEASE "${BUILD_PATH}")
SET(CMAKE_ARCHIVE_OUTPUT_DIRECTORY_DEBUG "${BUILD_PATH}")
SET(CMAKE_ARCHIVE_OUTPUT_DIRECTORY_RELEASE "${BUILD_PATH}")

# Set the c++ standard to c++17
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

set(GODOT_COMPILE_FLAGS )
set(GODOT_LINKER_FLAGS )

if ("${CMAKE_CXX_COMPILER_ID}" STREQUAL "MSVC")
	# using Visual Studio C++
	set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} /EHsc /WX") # /GF /MP
	set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} /DTYPED_METHOD_BIND")

	if(CMAKE_BUILD_TYPE MATCHES Debug)
		set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} /MDd") # /Od /RTC1 /Zi
	else()
		set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} /MD /O2") # /Oy /GL /Gy
		STRING(REGEX REPLACE "/RTC(su|[1su])" "" CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS}")
		string(REPLACE "/RTC1" "" CMAKE_CXX_FLAGS_DEBUG ${CMAKE_CXX_FLAGS_DEBUG})
	endif(CMAKE_BUILD_TYPE MATCHES Debug)

	# Disable conversion warning, truncation, unreferenced var, signed mismatch
	set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} /wd4244 /wd4305 /wd4101 /wd4018 /wd4267")

	add_definitions(-DNOMINMAX)

	# Unkomment for warning level 4
	#if(CMAKE_CXX_FLAGS MATCHES "/W[0-4]")
	#	string(REGEX REPLACE "/W[0-4]" "" CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS}")
	#endif()

else()

#elseif ("${CMAKE_CXX_COMPILER_ID}" STREQUAL "Clang")
	# using Clang
#elseif ("${CMAKE_CXX_COMPILER_ID}" STREQUAL "GNU")
	# using GCC and maybe MinGW?

	set(GODOT_LINKER_FLAGS "-static-libgcc -static-libstdc++ -Wl,-R,'$$ORIGIN'")

	# Hmm.. maybe to strikt?
	set(GODOT_COMPILE_FLAGS "-fPIC -g -Wwrite-strings")
	set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} -Wchar-subscripts -Wcomment -Wdisabled-optimization")
	set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} -Wformat -Wformat=2 -Wformat-security -Wformat-y2k")
	set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} -Wimport -Winit-self -Winline -Winvalid-pch -Werror")
	set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} -Wmissing-braces -Wmissing-format-attribute")
	set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} -Wmissing-include-dirs -Wmissing-noreturn -Wpacked -Wpointer-arith")
	set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} -Wredundant-decls -Wreturn-type -Wsequence-point")
	set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} -Wswitch -Wswitch-enum -Wtrigraphs")
	set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} -Wuninitialized -Wunknown-pragmas -Wunreachable-code -Wunused-label")
	set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} -Wunused-value -Wvariadic-macros -Wvolatile-register-var -Wno-error=attributes")

	# -Wshadow -Wextra -Wall -Weffc++ -Wfloat-equal -Wstack-protector -Wunused-parameter -Wsign-compare -Wunused-variable -Wcast-align
	# -Wunused-function -Wstrict-aliasing -Wstrict-aliasing=2 -Wmissing-field-initializers

	if(NOT CMAKE_SYSTEM_NAME STREQUAL "Android")
		set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} -Wno-ignored-attributes")
	endif()

	if(CMAKE_BUILD_TYPE MATCHES Debug)
		set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} -fno-omit-frame-pointer -O0")
	else()
		set(GODOT_COMPILE_FLAGS "${GODOT_COMPILE_FLAGS} -O3")
	endif(CMAKE_BUILD_TYPE MATCHES Debug)
endif()

# Get Sources
file(GLOB_RECURSE SOURCES src/*.c**)
file(GLOB_RECURSE HEADERS include/*.h**)

# Define our godot-cpp library
add_library(${PROJECT_NAME} SHARED ${SOURCES} ${HEADERS})

target_include_directories(${PROJECT_NAME} SYSTEM
	PRIVATE
		${CPP_BINDINGS_PATH}/include
	${CPP_BINDINGS_PATH}/gen/include
		${GODOT_HEADERS_PATH}
)

# Create the correct name (godot.os.build_type.system_bits)
# Synchronized with godot-cpp's CMakeLists.txt

set(BITS 32)
if(CMAKE_SIZEOF_VOID_P EQUAL 8)
	set(BITS 64)
endif(CMAKE_SIZEOF_VOID_P EQUAL 8)

if(CMAKE_BUILD_TYPE MATCHES Debug)
	set(GODOT_CPP_BUILD_TYPE Debug)
else()
	set(GODOT_CPP_BUILD_TYPE Release)
endif()

string(TOLOWER ${CMAKE_SYSTEM_NAME} SYSTEM_NAME)
string(TOLOWER ${GODOT_CPP_BUILD_TYPE} BUILD_TYPE)

if(ANDROID)
	# Added the android abi after system name
	set(SYSTEM_NAME ${SYSTEM_NAME}.${ANDROID_ABI})
endif()

if(CMAKE_VERSION VERSION_GREATER "3.13")
	target_link_directories(${PROJECT_NAME}
		PRIVATE
		${CPP_BINDINGS_PATH}/bin/
	)

	target_link_libraries(${PROJECT_NAME}
		godot-cpp.${SYSTEM_NAME}.${BUILD_TYPE}$<$<NOT:$<PLATFORM_ID:Android>>:.${BITS}>
	)
else()
	target_link_libraries(${PROJECT_NAME}
			${CPP_BINDINGS_PATH}/bin/libgodot-cpp.${SYSTEM_NAME}.${BUILD_TYPE}$<$<NOT:$<PLATFORM_ID:Android>>:.${BITS}>.a
	)
endif()

# Add the compile flags
set_property(TARGET ${PROJECT_NAME} APPEND_STRING PROPERTY COMPILE_FLAGS ${GODOT_COMPILE_FLAGS})
set_property(TARGET ${PROJECT_NAME} APPEND_STRING PROPERTY LINK_FLAGS ${GODOT_LINKER_FLAGS})

set_property(TARGET ${PROJECT_NAME} PROPERTY OUTPUT_NAME "gdexample")
```




# Cpp Source
## [`example.h`](https://github.com/godotengine/godot-cpp/blob/master/test/src/example.h)
```cpp
/* godot-cpp integration testing project.
 *
 * This is free and unencumbered software released into the public domain.
 */

#ifndef EXAMPLE_CLASS_H
#define EXAMPLE_CLASS_H

// We don't need windows.h in this example plugin but many others do, and it can
// lead to annoying situations due to the ton of macros it defines.
// So we include it and make sure CI warns us if we use something that conflicts
// with a Windows define.
#ifdef WIN32
#include <windows.h>
#endif

#include <godot_cpp/classes/control.hpp>
#include <godot_cpp/classes/global_constants.hpp>
#include <godot_cpp/classes/viewport.hpp>

#include <godot_cpp/core/binder_common.hpp>

using namespace godot;

class ExampleRef : public RefCounted {
	GDCLASS(ExampleRef, RefCounted);

protected:
	static void _bind_methods() {}

public:
	ExampleRef();
	~ExampleRef();
};

class ExampleMin : public Control {
	GDCLASS(ExampleMin, Control);

protected:
	static void _bind_methods(){};
};

class Example : public Control {
	GDCLASS(Example, Control);

protected:
	static void _bind_methods();

	void _notification(int p_what);
	bool _set(const StringName &p_name, const Variant &p_value);
	bool _get(const StringName &p_name, Variant &r_ret) const;
	void _get_property_list(List<PropertyInfo> *p_list) const;
	bool _property_can_revert(const StringName &p_name) const;
	bool _property_get_revert(const StringName &p_name, Variant &r_property) const;

	String _to_string() const;

private:
	Vector2 custom_position;
	Vector3 property_from_list;
	Vector2 dprop[3];

public:
	// Constants.
	enum Constants {
		FIRST,
		ANSWER_TO_EVERYTHING = 42,
	};

	enum {
		CONSTANT_WITHOUT_ENUM = 314,
	};

	Example();
	~Example();

	// Functions.
	void simple_func();
	void simple_const_func() const;
	String return_something(const String &base);
	Viewport *return_something_const() const;
	ExampleRef *return_extended_ref() const;
	Ref<ExampleRef> extended_ref_checks(Ref<ExampleRef> p_ref) const;
	Variant varargs_func(const Variant **args, GDNativeInt arg_count, GDNativeCallError &error);
	int varargs_func_nv(const Variant **args, GDNativeInt arg_count, GDNativeCallError &error);
	void varargs_func_void(const Variant **args, GDNativeInt arg_count, GDNativeCallError &error);
	void emit_custom_signal(const String &name, int value);
	int def_args(int p_a = 100, int p_b = 200);

	Array test_array() const;
	void test_tarray_arg(const TypedArray<int64_t> &p_array);
	TypedArray<Vector2> test_tarray() const;
	Dictionary test_dictionary() const;

	// Property.
	void set_custom_position(const Vector2 &pos);
	Vector2 get_custom_position() const;
	Vector4 get_v4() const;

	// Static method.
	static int test_static(int p_a, int p_b);
	static void test_static2();

	// Virtual function override (no need to bind manually).
	virtual bool _has_point(const Vector2 &point) const override;
};

VARIANT_ENUM_CAST(Example, Constants);

#endif // EXAMPLE_CLASS_H
```
## [`example.cpp`](https://github.com/godotengine/godot-cpp/blob/master/test/src/example.cpp)
```cpp
/* godot-cpp integration testing project.
 *
 * This is free and unencumbered software released into the public domain.
 */

#include "example.h"

#include <godot_cpp/core/class_db.hpp>

#include <godot_cpp/classes/global_constants.hpp>
#include <godot_cpp/classes/label.hpp>
#include <godot_cpp/variant/utility_functions.hpp>

using namespace godot;

ExampleRef::ExampleRef() {
	UtilityFunctions::print("ExampleRef created.");
}

ExampleRef::~ExampleRef() {
	UtilityFunctions::print("ExampleRef destroyed.");
}

int Example::test_static(int p_a, int p_b) {
	return p_a + p_b;
}

void Example::test_static2() {
	UtilityFunctions::print("  void static");
}

int Example::def_args(int p_a, int p_b) {
	return p_a + p_b;
}

void Example::_notification(int p_what) {
	UtilityFunctions::print("Notification: ", String::num(p_what));
}

bool Example::_set(const StringName &p_name, const Variant &p_value) {
	String name = p_name;
	if (name.begins_with("dproperty")) {
		int index = name.get_slicec('_', 1).to_int();
		dprop[index] = p_value;
		return true;
	}
	if (name == "property_from_list") {
		property_from_list = p_value;
		return true;
	}
	return false;
}

bool Example::_get(const StringName &p_name, Variant &r_ret) const {
	String name = p_name;
	if (name.begins_with("dproperty")) {
		int index = name.get_slicec('_', 1).to_int();
		r_ret = dprop[index];
		return true;
	}
	if (name == "property_from_list") {
		r_ret = property_from_list;
		return true;
	}
	return false;
}

String Example::_to_string() const {
	return "[ GDExtension::Example <--> Instance ID:" + itos(get_instance_id()) + " ]";
}

void Example::_get_property_list(List<PropertyInfo> *p_list) const {
	p_list->push_back(PropertyInfo(Variant::VECTOR3, "property_from_list"));
	for (int i = 0; i < 3; i++) {
		p_list->push_back(PropertyInfo(Variant::VECTOR2, "dproperty_" + itos(i)));
	}
}

bool Example::_property_can_revert(const StringName &p_name) const {
	if (p_name == StringName("property_from_list") && property_from_list != Vector3(42, 42, 42)) {
		return true;
	} else {
		return false;
	}
};

bool Example::_property_get_revert(const StringName &p_name, Variant &r_property) const {
	if (p_name == StringName("property_from_list")) {
		r_property = Vector3(42, 42, 42);
		return true;
	} else {
		return false;
	}
};

void Example::_bind_methods() {
	// Methods.
	ClassDB::bind_method(D_METHOD("simple_func"), &Example::simple_func);
	ClassDB::bind_method(D_METHOD("simple_const_func"), &Example::simple_const_func);
	ClassDB::bind_method(D_METHOD("return_something"), &Example::return_something);
	ClassDB::bind_method(D_METHOD("return_something_const"), &Example::return_something_const);
	ClassDB::bind_method(D_METHOD("return_extended_ref"), &Example::return_extended_ref);
	ClassDB::bind_method(D_METHOD("extended_ref_checks"), &Example::extended_ref_checks);

	ClassDB::bind_method(D_METHOD("test_array"), &Example::test_array);
	ClassDB::bind_method(D_METHOD("test_tarray_arg", "array"), &Example::test_tarray_arg);
	ClassDB::bind_method(D_METHOD("test_tarray"), &Example::test_tarray);
	ClassDB::bind_method(D_METHOD("test_dictionary"), &Example::test_dictionary);

	ClassDB::bind_method(D_METHOD("def_args", "a", "b"), &Example::def_args, DEFVAL(100), DEFVAL(200));

	ClassDB::bind_static_method("Example", D_METHOD("test_static", "a", "b"), &Example::test_static);
	ClassDB::bind_static_method("Example", D_METHOD("test_static2"), &Example::test_static2);

	{
		MethodInfo mi;
		mi.arguments.push_back(PropertyInfo(Variant::STRING, "some_argument"));
		mi.name = "varargs_func";
		ClassDB::bind_vararg_method(METHOD_FLAGS_DEFAULT, "varargs_func", &Example::varargs_func, mi);
	}

	{
		MethodInfo mi;
		mi.arguments.push_back(PropertyInfo(Variant::STRING, "some_argument"));
		mi.name = "varargs_func_nv";
		ClassDB::bind_vararg_method(METHOD_FLAGS_DEFAULT, "varargs_func_nv", &Example::varargs_func_nv, mi);
	}

	{
		MethodInfo mi;
		mi.arguments.push_back(PropertyInfo(Variant::STRING, "some_argument"));
		mi.name = "varargs_func_void";
		ClassDB::bind_vararg_method(METHOD_FLAGS_DEFAULT, "varargs_func_void", &Example::varargs_func_void, mi);
	}

	// Properties.
	ADD_GROUP("Test group", "group_");
	ADD_SUBGROUP("Test subgroup", "group_subgroup_");

	ClassDB::bind_method(D_METHOD("get_custom_position"), &Example::get_custom_position);
	ClassDB::bind_method(D_METHOD("get_v4"), &Example::get_v4);
	ClassDB::bind_method(D_METHOD("set_custom_position", "position"), &Example::set_custom_position);
	ADD_PROPERTY(PropertyInfo(Variant::VECTOR2, "group_subgroup_custom_position"), "set_custom_position", "get_custom_position");

	// Signals.
	ADD_SIGNAL(MethodInfo("custom_signal", PropertyInfo(Variant::STRING, "name"), PropertyInfo(Variant::INT, "value")));
	ClassDB::bind_method(D_METHOD("emit_custom_signal", "name", "value"), &Example::emit_custom_signal);

	// Constants.
	BIND_ENUM_CONSTANT(FIRST);
	BIND_ENUM_CONSTANT(ANSWER_TO_EVERYTHING);

	BIND_CONSTANT(CONSTANT_WITHOUT_ENUM);
}

Example::Example() {
	UtilityFunctions::print("Constructor.");
}

Example::~Example() {
	UtilityFunctions::print("Destructor.");
}

// Methods.
void Example::simple_func() {
	UtilityFunctions::print("  Simple func called.");
}

void Example::simple_const_func() const {
	UtilityFunctions::print("  Simple const func called.");
}

String Example::return_something(const String &base) {
	UtilityFunctions::print("  Return something called.");
	return base;
}

Viewport *Example::return_something_const() const {
	UtilityFunctions::print("  Return something const called.");
	if (is_inside_tree()) {
		Viewport *result = get_viewport();
		return result;
	}
	return nullptr;
}

ExampleRef *Example::return_extended_ref() const {
	return memnew(ExampleRef());
}

Ref<ExampleRef> Example::extended_ref_checks(Ref<ExampleRef> p_ref) const {
	Ref<ExampleRef> ref;
	ref.instantiate();
	// TODO the returned value gets dereferenced too early and return a null object otherwise.
	ref->reference();
	UtilityFunctions::print("  Example ref checks called with value: ", p_ref->get_instance_id(), ", returning value: ", ref->get_instance_id());
	return ref;
}

Variant Example::varargs_func(const Variant **args, GDNativeInt arg_count, GDNativeCallError &error) {
	UtilityFunctions::print("  Varargs (Variant return) called with ", String::num((double)arg_count), " arguments");
	return arg_count;
}

int Example::varargs_func_nv(const Variant **args, GDNativeInt arg_count, GDNativeCallError &error) {
	UtilityFunctions::print("  Varargs (int return) called with ", String::num((double)arg_count), " arguments");
	return 42;
}

void Example::varargs_func_void(const Variant **args, GDNativeInt arg_count, GDNativeCallError &error) {
	UtilityFunctions::print("  Varargs (no return) called with ", String::num((double)arg_count), " arguments");
}

void Example::emit_custom_signal(const String &name, int value) {
	emit_signal("custom_signal", name, value);
}

Array Example::test_array() const {
	Array arr;

	arr.resize(2);
	arr[0] = Variant(1);
	arr[1] = Variant(2);

	return arr;
}

void Example::test_tarray_arg(const TypedArray<int64_t> &p_array) {
	for (int i = 0; i < p_array.size(); i++) {
		UtilityFunctions::print(p_array[i]);
	}
}

TypedArray<Vector2> Example::test_tarray() const {
	TypedArray<Vector2> arr;

	arr.resize(2);
	arr[0] = Vector2(1, 2);
	arr[1] = Vector2(2, 3);

	return arr;
}

Dictionary Example::test_dictionary() const {
	Dictionary dict;

	dict["hello"] = "world";
	dict["foo"] = "bar";

	return dict;
}

// Properties.
void Example::set_custom_position(const Vector2 &pos) {
	custom_position = pos;
}

Vector2 Example::get_custom_position() const {
	return custom_position;
}

Vector4 Example::get_v4() const {
	return Vector4(1.2, 3.4, 5.6, 7.8);
}

// Virtual function override.
bool Example::_has_point(const Vector2 &point) const {
	Label *label = get_node<Label>("Label");
	label->set_text("Got point: " + Variant(point).stringify());

	return false;
}
```

## [`register_types.h`](https://github.com/godotengine/godot-cpp/blob/master/test/src/register_types.h)
```cpp
/* godot-cpp integration testing project.
 *
 * This is free and unencumbered software released into the public domain.
 */

#ifndef EXAMPLE_REGISTER_TYPES_H
#define EXAMPLE_REGISTER_TYPES_H

#include <godot_cpp/core/class_db.hpp>

using namespace godot;

void initialize_example_module(ModuleInitializationLevel p_level);
void uninitialize_example_module(ModuleInitializationLevel p_level);

#endif // EXAMPLE_REGISTER_TYPES_H
```
## [`register_types.cpp`](https://github.com/godotengine/godot-cpp/blob/master/test/src/register_types.cpp)
```cpp
/* godot-cpp integration testing project.
 *
 * This is free and unencumbered software released into the public domain.
 */

#include "register_types.h"

#include <godot/gdnative_interface.h>

#include <godot_cpp/core/class_db.hpp>
#include <godot_cpp/core/defs.hpp>
#include <godot_cpp/godot.hpp>

#include "example.h"
#include "tests.h"

using namespace godot;

void initialize_example_module(ModuleInitializationLevel p_level) {
	if (p_level != MODULE_INITIALIZATION_LEVEL_SCENE) {
		return;
	}

	ClassDB::register_class<ExampleRef>();
	ClassDB::register_class<ExampleMin>();
	ClassDB::register_class<Example>();
}

void uninitialize_example_module(ModuleInitializationLevel p_level) {
	if (p_level != MODULE_INITIALIZATION_LEVEL_SCENE) {
		return;
	}
}

extern "C" {
// Initialization.
GDNativeBool GDN_EXPORT example_library_init(const GDNativeInterface *p_interface, const GDNativeExtensionClassLibraryPtr p_library, GDNativeInitialization *r_initialization) {
	godot::GDExtensionBinding::InitObject init_obj(p_interface, p_library, r_initialization);

	init_obj.register_initializer(initialize_example_module);
	init_obj.register_terminator(uninitialize_example_module);
	init_obj.set_minimum_library_initialization_level(MODULE_INITIALIZATION_LEVEL_SCENE);

	return init_obj.init();
}
}
```


## [`test.h`](https://github.com/godotengine/godot-cpp/blob/master/test/src/tests.h)
```cpp
/*************************************************************************/
/*  tests.h                                                              */
/*************************************************************************/
/*                       This file is part of:                           */
/*                           GODOT ENGINE                                */
/*                      https://godotengine.org                          */
/*************************************************************************/
/* Copyright (c) 2007-2022 Juan Linietsky, Ariel Manzur.                 */
/* Copyright (c) 2014-2022 Godot Engine contributors (cf. AUTHORS.md).   */
/*                                                                       */
/* Permission is hereby granted, free of charge, to any person obtaining */
/* a copy of this software and associated documentation files (the       */
/* "Software"), to deal in the Software without restriction, including   */
/* without limitation the rights to use, copy, modify, merge, publish,   */
/* distribute, sublicense, and/or sell copies of the Software, and to    */
/* permit persons to whom the Software is furnished to do so, subject to */
/* the following conditions:                                             */
/*                                                                       */
/* The above copyright notice and this permission notice shall be        */
/* included in all copies or substantial portions of the Software.       */
/*                                                                       */
/* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,       */
/* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF    */
/* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.*/
/* IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY  */
/* CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,  */
/* TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE     */
/* SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                */
/*************************************************************************/

#ifndef TESTS_H
#define TESTS_H

#include "godot_cpp/templates/cowdata.hpp"
#include "godot_cpp/templates/hash_map.hpp"
#include "godot_cpp/templates/hash_set.hpp"
#include "godot_cpp/templates/hashfuncs.hpp"
#include "godot_cpp/templates/list.hpp"
#include "godot_cpp/templates/pair.hpp"
#include "godot_cpp/templates/rb_map.hpp"
#include "godot_cpp/templates/rb_set.hpp"
#include "godot_cpp/templates/rid_owner.hpp"
#include "godot_cpp/templates/safe_refcount.hpp"
#include "godot_cpp/templates/search_array.hpp"
#include "godot_cpp/templates/self_list.hpp"
#include "godot_cpp/templates/sort_array.hpp"
#include "godot_cpp/templates/spin_lock.hpp"
#include "godot_cpp/templates/thread_work_pool.hpp"
#include "godot_cpp/templates/vector.hpp"
#include "godot_cpp/templates/vmap.hpp"
#include "godot_cpp/templates/vset.hpp"

#endif // TESTS_H
```




# [Cpp Godot Project](https://github.com/godotengine/godot-cpp/tree/master/test/demo)

## [`example.gdextension`](https://github.com/godotengine/godot-cpp/blob/master/test/demo/default_env.tres)
```
[configuration]

entry_symbol = "example_library_init"

[libraries]

macos.debug = "res://bin/libgdexample.macos.debug.framework"
macos.release = "res://bin/libgdexample.macos.release.framework"
windows.debug.x86_32 = "res://bin/libgdexample.windows.debug.x86_32.dll"
windows.release.x86_32 = "res://bin/libgdexample.windows.release.x86_32.dll"
windows.debug.x86_64 = "res://bin/libgdexample.windows.debug.x86_64.dll"
windows.release.x86_64 = "res://bin/libgdexample.windows.release.x86_64.dll"
linux.debug.x86_64 = "res://bin/libgdexample.linux.debug.x86_64.so"
linux.release.x86_64 = "res://bin/libgdexample.linux.release.x86_64.so"
linux.debug.arm64 = "res://bin/libgdexample.linux.debug.arm64.so"
linux.release.arm64 = "res://bin/libgdexample.linux.release.arm64.so"
linux.debug.rv64 = "res://bin/libgdexample.linux.debug.rv64.so"
linux.release.rv64 = "res://bin/libgdexample.linux.release.rv64.so"
```
## [`project.godot`](https://github.com/godotengine/godot-cpp/blob/master/test/demo/project.godot)
```
; Engine configuration file.
; It's best edited using the editor UI and not directly,
; since the parameters that go here are not all obvious.
;
; Format:
;   [section] ; section goes between []
;   param=value ; assign values to parameters

config_version=5

[application]

config/name="GDExtension Test Project"
run/main_scene="res://main.tscn"
config/features=PackedStringArray("4.0")
config/icon="res://icon.png"

[native_extensions]

paths=["res://example.gdextension"]

[rendering]

environment/defaults/default_environment="res://default_env.tres"
```
## [`main.tscn`](https://github.com/godotengine/godot-cpp/blob/master/test/demo/main.tscn)
```
[gd_scene load_steps=2 format=3 uid="uid://dmx2xuigcpvt4"]

[ext_resource type="Script" path="res://main.gd" id="1_c326s"]

[node name="Node" type="Node"]
script = ExtResource( "1_c326s" )

[node name="Example" type="Example" parent="."]

[node name="ExampleMin" type="ExampleMin" parent="Example"]

[node name="Label" type="Label" parent="Example"]
offset_left = 194.0
offset_top = -2.0
offset_right = 234.0
offset_bottom = 21.0

[node name="Button" type="Button" parent="."]
offset_right = 79.0
offset_bottom = 29.0
text = "Click me!"

[connection signal="custom_signal" from="Example" to="." method="_on_Example_custom_signal"]
```
## [`main.gd`](https://github.com/godotengine/godot-cpp/blob/master/test/demo/main.gd)
```python
extends Node

func _ready():
	# Bind signals
	prints("Signal bind")
	$Button.button_up.connect($Example.emit_custom_signal.bind("Button", 42))

	prints("")

	# To string.
	prints("To string")
	prints("  Example --> ", $Example.to_string())
	prints("  ExampleMin --> ", $Example/ExampleMin.to_string())

	# Call static methods.
	prints("Static method calls")
	prints("  static (109)", Example.test_static(9, 100));
	Example.test_static2();

	# Property list.
	prints("Property list")
	$Example.property_from_list = Vector3(100, 200, 300)
	prints("  property value ", $Example.property_from_list)
	
	# Call methods.
	prints("Instance method calls")
	$Example.simple_func()
	($Example as Example).simple_const_func() # Force use of ptrcall
	prints("  returned", $Example.return_something("some string"))
	prints("  returned const", $Example.return_something_const())
	prints("  returned ref", $Example.return_extended_ref())
	prints("  returned ", $Example.get_v4())

	prints("VarArg method calls")
	var ref = ExampleRef.new()
	prints("  sending ref: ", ref.get_instance_id(), "returned ref: ", $Example.extended_ref_checks(ref).get_instance_id())
	prints("  vararg args", $Example.varargs_func("some", "arguments", "to", "test"))
	prints("  vararg_nv ret", $Example.varargs_func_nv("some", "arguments", "to", "test"))
	$Example.varargs_func_void("some", "arguments", "to", "test")

	prints("Method calls with default values")
	prints("  defval (300)", $Example.def_args())
	prints("  defval (250)", $Example.def_args(50))
	prints("  defval (150)", $Example.def_args(50, 100))

	prints("Array and Dictionary")
	prints("  test array", $Example.test_array())
	prints("  test tarray", $Example.test_tarray())
	prints("  test dictionary", $Example.test_dictionary())
	var array: Array[int] = [1, 2, 3]
	$Example.test_tarray_arg(array)

	prints("Properties")
	prints("  custom position is", $Example.group_subgroup_custom_position)
	$Example.group_subgroup_custom_position = Vector2(50, 50)
	prints("  custom position now is", $Example.group_subgroup_custom_position)

	prints("Constnts")
	prints("  FIRST", $Example.FIRST)
	prints("  ANSWER_TO_EVERYTHING", $Example.ANSWER_TO_EVERYTHING)
	prints("  CONSTANT_WITHOUT_ENUM", $Example.CONSTANT_WITHOUT_ENUM)

func _on_Example_custom_signal(signal_name, value):
	prints("Example emitted:", signal_name, value)
```