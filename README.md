# UIF
UI Framework for Unity Engine to manage complex UI in a web-like fashion.

#### Installation:
Add an entry in your manifest.json as follows:
```C#
"com.kaiyum.uif": "https://github.com/kaiyumcg/UIF.git"
```


Since unity does not support git dependencies, you need the following entries as well:
```C#
"com.kaiyum.attributeext" : "https://github.com/kaiyumcg/AttributeExt.git",
"com.kaiyum.unityext": "https://github.com/kaiyumcg/UnityExt.git",
"com.kaiyum.editorutil": "https://github.com/kaiyumcg/EditorUtil.git"
```
Add them into your manifest.json file in "Packages\" directory of your unity project, if they are already not in manifest.json file.