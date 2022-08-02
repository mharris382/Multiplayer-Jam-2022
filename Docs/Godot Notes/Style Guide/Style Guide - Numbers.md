
### Numbers[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#numbers "Permalink to this headline")

Don't omit the leading or trailing zero in floating-point numbers. Otherwise, this makes them less readable and harder to distinguish from integers at a glance.

**Good**:
```
var float_number = 0.234
var other_float_number = 13.0
```
**Bad**:
```
var float_number = .234
var other_float_number = 13.
```
Use lowercase for letters in hexadecimal numbers, as their lower height makes the number easier to read.

**Good**:
```
var hex_number = 0xfb8c0b
```
**Bad**:
```
var hex_number = 0xFB8C0B
```
Take advantage of GDScript's underscores in literals to make large numbers more readable.

**Good**:
```
var large_number = 1_234_567_890
var large_hex_number = 0xffff_f8f8_0000
var large_bin_number = 0b1101_0010_1010
# Numbers lower than 1000000 generally don't need separators.
var small_number = 12345
```
**Bad**:
```
var large_number = 1234567890
var large_hex_number = 0xfffff8f80000
var large_bin_number = 0b110100101010
# Numbers lower than 1000000 generally don't need separators.
var small_number = 12_345
```
