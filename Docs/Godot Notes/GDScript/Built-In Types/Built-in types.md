## Built-in types[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#built-in-types "Permalink to this headline")

Built-in types are stack-allocated. They are passed as values. This means a copy is created on each assignment or when passing them as arguments to functions. The only exceptions are `[[Array]]`s and `Dictionaries`, which are passed by reference so they are shared. (Pooled arrays such as `PoolByteArray` are still passed as values.)

### Basic built-in types[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#basic-built-in-types "Permalink to this headline")

A variable in GDScript can be assigned to several built-in types.

#### null[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#null "Permalink to this headline")

`null` is an empty data type that contains no information and can not be assigned any other value.

#### [bool](https://docs.godotengine.org/en/stable/classes/class_bool.html#class-bool)[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#bool "Permalink to this headline")

Short for "boolean", it can only contain `true` or `false`.

#### [int](https://docs.godotengine.org/en/stable/classes/class_int.html#class-int)[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#int "Permalink to this headline")

Short for "integer", it stores whole numbers (positive and negative). It is stored as a 64-bit value, equivalent to "int64_t" in C++.

#### [float](https://docs.godotengine.org/en/stable/classes/class_float.html#class-float)[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#float "Permalink to this headline")

Stores real numbers, including decimals, using floating-point values. It is stored as a 64-bit value, equivalent to "double" in C++. Note: Currently, data structures such as Vector2, Vector3, and PoolRealArray store 32-bit single-precision "float" values.

#### [String](https://docs.godotengine.org/en/stable/classes/class_string.html#class-string)[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#string "Permalink to this headline")

A sequence of characters in [Unicode format](https://en.wikipedia.org/wiki/Unicode). Strings can contain the following escape sequences:
Escape sequence | Expands to
---|---
`\n` | Newline (line feed)
`\t` | Horizontal tab character
`\r` | Carriage return
`\a` | Alert (beep/bell)
`\b` | Backspace
`\f` | Formfeed page break
`\v` | Vertical tab character
`\"` | Double quote
`\'` | Single quote
`\\` | Backslash
`\uXXXX` | Unicode codepoint XXXX (hexadecimal, case-insensitive)

GDScript also supports [GDScript format strings](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_format_string.html#doc-gdscript-printf).

![[Vector Built-in Types]]

![[Engine Built-in Types]]

[[Container Built-in Types]]
