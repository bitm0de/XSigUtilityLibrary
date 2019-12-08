# XSigUtilityLibrary
Intersystem Communications (ISC) library for SIMPL#.

![modules](https://img.shields.io/badge/S%23-Modules-brightgreen.svg) [![issues](https://img.shields.io/github/issues/bitm0de/XSigUtilityLibrary.svg?style=flat)](https://github.com/bitm0de/XSigUtilityLibrary/issues) [![license](https://img.shields.io/badge/license-MIT-brightgreen.svg)](https://github.com/bitm0de/XSigUtilityLibrary/blob/master/LICENSE)

## Example
```cs
var buffer = new byte[] {
    0x80, 0x0A,                                                  // \x80\x0A (Digital = 1, Index = 11)
    0xC0, 0x00, 0x09, 0x52,                                      // \xC0\x00\x09R (Analog = 1234, Index = 1)
    0xC8, 0x05, (byte)'1', (byte)'2', (byte)'3', (byte)'4', 0xFF // \xC8\x051234\xFF (Serial = "1234", Index = 6)
};
using (var xsigStream = new XSigTokenStreamReader(new MemoryStream(buffer))) {
    XSigToken token;
    while ((token = xsigStream.ReadXSigToken()) != null) CrestronConsole.PrintLine(token.GetType().Name + ": " + token.Index + " = " + token);
}

CrestronConsole.PrintLine(string.Concat(XSigHelpers.GetBytes(15, true).Select(b => "\\x" + b.ToString("X2")).ToArray()));
```
