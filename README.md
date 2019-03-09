# localegen
Excel file to JS object/JSON converter with support for multiple locales

# Release:
For the last release, visit the [releases](https://github.com/mttssz/localegen/releases) tab of the repository.

# Usage:

Use the following syntax to generate your locale file:
```
localegen <input_file>.(xls|xlsx) <output_file>.(js|json)
```

The input file must be an excel workbook with the content on the first worksheet. The very first cell (A1) is ignored by the generator. The first column is used to hold identifiers for the translated text. The first row after the first cell is used to hold your language codes. You can use however many you want, the converter will dynamically generate every language you add.

If you specify an input file with a .js extension, the output will use the following syntax:
```js
module.exports = {
   ...
}
```

If you ues a .json extension the output will be a standard JSON object:
```js
{
    ...
}
```
