Literal | Type
---|---
`45` | Base 10 integer
`0x8f51` | Base 16 (hexadecimal) integer
`0b101010` | Base 2 (binary) integer
`3.14, 58.1e-10` | Floating-point number (real)
`"Hello", "Hi"` | Strings
`"""Hello"""` | Multiline string
`@"Node/Label"` | NodePath or StringName
`$NodePath` | Shorthand for get_node("NodePath")
Integers and floats can have their numbers separated with `_` to make them more readable. The following ways to write numbers are all valid:

```
12_345_678  # Equal to 12345678.
3.141_592_7  # Equal to 3.1415927.
0x8080_0000_ffff  # Equal to 0x80800000ffff.
0b11_00_11_00  # Equal to 0b11001100.
```