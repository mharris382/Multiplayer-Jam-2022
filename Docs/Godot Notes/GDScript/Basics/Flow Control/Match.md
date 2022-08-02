 # match[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#match "Permalink to this headline")

A `match` statement is used to branch execution of a program. It's the equivalent of the `switch` statement found in many other languages, but offers some additional features.

Basic syntax:

```
match [expression]:
    [pattern](s):
        [block]
    [pattern](s):
        [block]
    [pattern](s):
        [block]
```

**Crash-course for people who are familiar with switch statements**:

1.  Replace `switch` with `match`.
2.  Remove `case`.
3.  Remove any `break`s. If you don't want to `break` by default, you can use `continue` for a fallthrough.
4.  Change `default` to a single underscore.

-   Constant pattern
    
    Constant primitives, like numbers and strings:
    
  ```
  match x:
        1:
            print("We are number one!")
        2:
            print("Two are better than one!")
        "test":
            print("Oh snap! It's a string!")
    
```
-   Variable pattern
    
    Matches the contents of a variable/enum:
    
   ```
 match typeof(x):
        TYPE_REAL:
            print("float")
        TYPE_STRING:
            print("text")
        TYPE_ARRAY:
            print("array")
```
    
-   Wildcard pattern
    
    This pattern matches everything. It's written as a single underscore.
    It can be used as the equivalent of the `default` in a `switch` statement in other languages:
    
```
    match x:
        1:
            print("It's one!")
        2:
            print("It's one times two!")
        _:
            print("It's not 1 or 2. I don't care to be honest.")
```
    
-   Binding pattern
    
    A binding pattern introduces a new variable. Like the wildcard pattern, it matches everything - and also gives that value a name. It's especially useful in array and dictionary patterns:
    
```
    match x:
        1:
            print("It's one!")
        2:
            print("It's one times two!")
        var new_var:
            print("It's not 1 or 2, it's ", new_var)
```
    
-   Array pattern
    
    Matches an array. Every single element of the array pattern is a pattern itself, so you can nest them.
    The length of the array is tested first, it has to be the same size as the pattern, otherwise the pattern doesn't match.
    **Open-ended array**: An array can be bigger than the pattern by making the last subpattern `..`.
    Every subpattern has to be comma-separated.
    
```
    match x:
        []:
            print("Empty array")
        [1, 3, "test", null]:
            print("Very specific array")
        [var start, _, "test"]:
            print("First element is ", start, ", and the last is \"test\"")
        [42, ..]:
            print("Open ended array")
```
    
-  **Dictionary pattern**
    
    Works in the same way as the array pattern. Every key has to be a constant pattern.
    The size of the dictionary is tested first, it has to be the same size as the pattern, otherwise the pattern doesn't match   **Open-ended dictionary**: A dictionary can be bigger than the pattern by making the last subpattern `..`.
    Every subpattern has to be comma separated.   If you don't specify a value, then only the existence of the key is checked. A value pattern is separated from the key pattern with a `:`.
    
```
    match x:
        {}:
            print("Empty dict")
        {"name": "Dennis"}:
            print("The name is Dennis")
        {"name": "Dennis", "age": var age}:
            print("Dennis is ", age, " years old.")
        {"name", "age"}:
            print("Has a name and an age, but it's not Dennis :(")
        {"key": "godotisawesome", ..}:
            print("I only checked for one entry and ignored the rest")
```
    
-   Multiple patterns
    
    You can also specify multiple patterns separated by a comma. These patterns aren't allowed to have any bindings in them.
    
```
    match x:
        1, 2, 3:
            print("It's 1 - 3")
        "Sword", "Splash potion", "Fist":
            print("Yep, you've taken damage")
```