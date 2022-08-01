`Operator` | Description
---|---
`x[index]` | Subscription (highest priority)
`x.attribute` | Attribute reference
`foo()` | Function call
`is` | Instance type checker
`~` | Bitwise NOT
`-x` | Negative / Unary negation
`* / %` | Multiplication / Division / Remainder. These operators have the same behavior as C++. Integer division is truncated rather than returning a fractional number, and the % operator is only available for ints ("fmod" for floats), and is additionally used for Format Strings
`+` | Addition / Concatenation of arrays
`-` | Subtraction
`<< >>` | Bit shifting
`&` | Bitwise AND
`^` | Bitwise XOR
`|` | Bitwise OR
`< > == != >= <=` | Comparisons
`in` | Content test
`! not` | Boolean NOT
`and &&` | Boolean AND
`or ||` | Boolean OR
`if x else` | Ternary if/else
`as` | Type casting
`= += -= *= /= %= &= <<= >>=` | Assignment (lowest priority)
Assignment also accepts `|=` but it breaks the markdown table formatting so I removed it