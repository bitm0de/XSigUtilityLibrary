# XSigUtilityLibrary
Intersystem Communications (ISC) library for SIMPL#.

![modules](https://img.shields.io/badge/S%23-Modules-brightgreen.svg) [![issues](https://img.shields.io/github/issues/bitm0de/XSigUtilityLibrary.svg?style=flat)](https://github.com/bitm0de/XSigUtilityLibrary/issues) [![license](https://img.shields.io/github/license/bitm0de/XSigUtilityLibrary.svg?style=flat)](https://github.com/bitm0de/XSigUtilityLibrary/blob/master/LICENSE)

## Example 1: Reading tokens from a byte array
```cs
var buffer = new byte[] {
    0x80, 0x0A,                                                  // \x80\x0A (Digital = 1, Index = 11)
    0xC0, 0x00, 0x09, 0x52,                                      // \xC0\x00\x09R (Analog = 1234, Index = 1)
    0xC8, 0x05, (byte)'1', (byte)'2', (byte)'3', (byte)'4', 0xFF // \xC8\x051234\xFF (Serial = "1234", Index = 6)
};
using (var tokenReader = new XSigTokenStreamReader(new MemoryStream(buffer))) {
    XSigToken token;
    while ((token = tokenReader.ReadXSigToken()) != null)
        CrestronConsole.PrintLine(token.GetType().Name + ": " + token.Index + " = " + token);
}

CrestronConsole.PrintLine(string.Concat(XSigHelpers.GetBytes(15, true).Select(b => "\\x" + b.ToString("X2")).ToArray()));
```

## Example 2: Writing tokens to a byte array (and string)
```cs
using (var memoryStream = new MemoryStream())
{
    using (var tokenWriter = new XSigTokenStreamWriter(memoryStream, true))
        tokenWriter.WriteXSigData(new XSigToken[] {
        new XSigAnalogToken(1, 0x1234),         // aout1
        new XSigSerialToken(2, "Hello world!"), // aout2
        new XSigDigitalToken(3, true),          // dig_out1
    });
    byte[] bytes = memoryStream.ToArray();
    CrestronConsole.PrintLine(string.Concat(bytes.Select(b => "\\x" + b.ToString("X2")).ToArray());
}
```

## Example 3: Serialization/deserialization
```cs
public class StateObject : IXSigSerialization
{
    public bool Active { get; set; }
    public string Message { get; set; }

    public IEnumerable<XSigToken> Serialize()
    {
        return new XSigToken[] {
            new XSigDigitalToken(1, Active),
            new XSigSerialToken(2, Message)
        };
    }

    public T Deserialize<T>(IEnumerable<XSigToken> tokens)
        where T : class, IXSigSerialization
    {
        var obj = new StateObject();
        foreach (var token in tokens)
        {
            switch (token.TokenType)
            {
                case XSigTokenType.Digital:
                    if (token.Index == 1)
                    {
                        obj.Active = ((XSigDigitalToken)token).Value;
                        continue;
                    }
                    break;
                case XSigTokenType.Analog:
                    break;
                case XSigTokenType.Serial:
                    if (token.Index == 2)
                    {
                        obj.Message = ((XSigSerialToken)token).Value;
                        continue;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new XSigSerializationException("Unexpected data within XSig information.");
        }

        return obj as T;
    }
}
```

#### Serialization:
Writing an instance of an object which implements the `IXSigSerialization` interface (i.e. StateObject above):
```cs
using (var memoryStream = new MemoryStream())
{
    using (var tokenWriter = new XSigTokenStreamWriter(memoryStream, true))
        tokenWriter.WriteXSigData(stateObject);
}
```


>Note: The indexes are 1-based, and are set based on the output signal index. For instance, if nothing has been expanded on an XSIG symbol in SIMPL Windows where you have aout1 and dig_out1, aout1 is index 1 and dig_out1 is index 2.

>Important: Always ensure that your signal alignment matches your code to mitigate error log spam. If your datatypes and data don't match the symbol defined in your SIMPL Windows program, your logfile will be full of "Signal Mismatch in receive of Intersystem Communications" messages.
