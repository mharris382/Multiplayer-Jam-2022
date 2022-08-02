### Blank lines[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#blank-lines "Permalink to this headline")

Surround [[Functions]] and [[Classes |class]] definitions with two blank lines:

```
func heal(amount):
    health += amount
    health = min(health, max_health)
    emit_signal("health_changed", health)

func take_damage(amount, effect=null):
    health -= amount
    health = max(0, health)
    emit_signal("health_changed", health)
```

Use one blank line inside [[Functions]] to separate logical sections.