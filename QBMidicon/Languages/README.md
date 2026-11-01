# QBMidicon language packs
更新者：Codex
最終更新日時：2026-10-20 09:27

Japanese and English are the built-in languages. This release does not include
or distribute additional language packs.

If a separately distributed language pack is available, copy its culture folder
into the `Languages` directory next to the application before starting it. The
application loads external language resources from that location.

For example:

```text
<application directory>/Languages/de-DE/
```

Each culture directory must contain a non-empty `QBMidicon.Resources.resx` file.
An optional flag image uses the lowercase ISO 3166-1 alpha-2 region code, such as
`de.png` for `de-DE`. A localized guide can be placed under `Manual/index.html`.

Missing resource keys fall back to the built-in English resources.
