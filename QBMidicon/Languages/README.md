# QBMidicon language packs

`LanguagePacks` contains the language packs distributed with QBMidicon. Copy only
the cultures you need into this `Languages` directory before starting the app.

For example:

```text
LanguagePacks/de-DE/  ->  Languages/de-DE/
```

Each culture directory must contain a non-empty `QBMidicon.Resources.resx` file.
An optional flag image uses the lowercase ISO 3166-1 alpha-2 region code, such as
`de.png` for `de-DE`. A localized guide can be placed under `Manual/index.html`.

Missing resource keys fall back to the built-in English resources.
