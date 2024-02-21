## Unity Editor API: Custom Inspector Tabs Instruction

### Overview
This guide details the use of Custom Inspector Tabs within the Unity Editor API, with a focus on the `ObjectBuilder` function. This allows for the addition of objects in the inspector by specifying their position and model.

### Getting Started with TabExample

1. **Selection**:
    - Navigate to the **Hierarchy** view in the Unity Editor.
    - Select `TabExample` to activate its custom inspector tabs.

2. **Interaction**:
    - Buttons that appear in the Inspector window are clickable.
    - Edit values in "integer" and "strings" buttons with the respective string and integer values.

### Using ObjectBuilder

1. **Selection**:
    - In the **Hierarchy** view, select `ObjectBuilder`.

2. **Object Selection**:
    - Click the circle icon next to the object field in the Inspector to choose a model.

3. **Positioning**:
    - Enter a value into the "Spawn point" field to set the object's position.

4. **Build Object**:
    - Click the “Build Object” button in the Inspector.
    - The object will appear in both the Scene and Hierarchy.

### Notes

- Ensure all inputs are correctly filled before building an object to avoid errors.
- `ObjectBuilder` enhances workflow by enabling dynamic object addition to the scene for rapid prototyping.

Follow these instructions to utilize Custom Inspector Tabs and the `ObjectBuilder` function in Unity Editor API for efficient object addition and scene building in your Unity projects.  

*Reference*: [Unity Editor Scripting: Custom Inspector Tabs](https://www.youtube.com/watch?v=vAi7-unj1Ww "悬停显示")   
