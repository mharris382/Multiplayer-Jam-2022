# Assert keyword[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#assert-keyword "Permalink to this headline")

The `assert` keyword can be used to check conditions in debug builds. These assertions are ignored in non-debug builds. This means that the expression passed as argument won't be evaluated in a project exported in release mode. Due to this, assertions must **not** contain expressions that have side effects. Otherwise, the behavior of the script would vary depending on whether the project is run in a debug build.

```
# Check that 'i' is 0. If 'i' is not 0, an assertion error will occur.
assert(i == 0)
```

When running a project from the editor, the project will be paused if an assertion error occurs.