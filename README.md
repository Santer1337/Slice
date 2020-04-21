# slice.to - The best solution for authentication!
The official API wrappers of Slice Authentication services.

[slice.to](https://slice.to/) allows you to easily integrate secure and simple verification services into your projects without having to worry about the technical things. Our API allows you to create your own wrappers of our authentication in any language you want, the full documentation is on our website.

Features:
------

- Power End-to-end encryption (.NET and Native)
- Server-Sided variables
- Server-sided encrypted files *(Coming soon!)*
- Full modification of your user's info
- Custom token durations
- IP Locking users
- Application Freemode (All logins will return true)
- File integrity checking (.NET and Native)
- Spam & Attack protection

Online Panel:
------

Below are pictures of our online panel, used for managing your Applications:
![Dashboard](https://cdn.discordapp.com/attachments/696138250053746778/702013396756856922/unknown.png)
![Resources](https://cdn.discordapp.com/attachments/696138250053746778/702013868074991626/unknown.png)
![Manage](https://cdn.discordapp.com/attachments/696138250053746778/702014278244237312/unknown.png)

FAQ
===

Is Old Rod a deobfuscator?
-------------------------
No. It only disassembles the code and recompiles it. It will not simplify control flow, nor will it decrypt your strings, simplify arithmetic expressions, rename all symbols, decrypt resources, or anything like that. For this, other tools exist.

Heeeeeelp! it...
-----------------

- ... crashes,
- ... prints errors I don't want to read,
- ... produces corrupted files.

These are features, not bugs. You can turn them off by using:
```
OldRod.exe <input-file> --dont-crash --no-errors --no-output-corruption
```

Filing a bug report
-------------------
If the above does not work, please consider going to the [issue tracker](https://github.com/Washi1337/OldRod/issues) and file a _detailed_ bug report, taking the following into account:
- Be aware I do this project in my little free time.
- Because of this, when filing a report it is important to narrow down the issue as much as possible to your ability.
    - Issues simply stating "it doesn't work" will be ignored.
- Respect original authors of copyrighted software. **Don't upload copyrighted executables** protected by KoiVM. These issues will be **deleted** immediately.
- Look at the troubleshooting tips in the readme.

Also, be aware this is a **work in progress**. Sometimes the Magikarp has a tendency to randomly splash around and reach havoc in the file for unknown reasons. Little can be done here other than waiting for the beast to finally mature.


How do I troubleshoot Old Rod?
-----------------------------
Old Rod has quite a few diagnostics built-in that might help you out:
- `--verbose` (`-v`) or `--very-verbose` (`-vv`) will print debug and full error messages to the standard output.
- `--log-file` will produce a `report.log` in the output directory containing a log that is similar to enabling `--verbose`. You don't need to include `--verbose` to get a verbose output in the log file.
- `--dump-il`, `--dump-cil`, `--dump-cfg` and/or `--dump-cfg-all` will create all kinds of dumps of intermediate steps of the devirtualisation process in the output directory.
- `--rename-symbols` will rename most (but not all) symbols in the KoiVM runtime library to something more meaningful.
- `--only-export 1,2,3` or `--ignore-export 1,2,3` will only include or exclude exports 1, 2 and 3 respectively.
- `--salvage` will let the devirtualiser try to recover from errors as much as possible. Note that this is a very mysterious feature, and enabling this feature might have cool side-effects and result in incorrect binaries being produced.

Why did you create this?
------------------------
I thought it would be a cool project. 

Why did you release this?
-------------------------
I noticed quite a few people using KoiVM illegitimately (e.g. for protecting malware). Also KoiVM is now open source for anyone to grab, so I thought it wouldn't hurt the original author.

Why is Old Rod slower than other deobfuscators or devirtualizers?
-----------------------------------------------------------------
Because the project is complicated.

Why is the project so complicated?
----------------------------------
Because KoiVM is more complicated than the average VM that is out there for .NET. Check out the [docs](doc/) to find out how the recompiler works.

Also I am probably not the best coder or reverse engineer.

Couldn't you just use pattern matching for every CIL instruction like normal people?
------------------------------------------------------------------------------------
Yes, but I am stubborn, I don't like to write countless of patterns, and I like writing compilers.

What is the OldRod.Core.CodeGen namespace that is injected?
-----------------------------------------------------------
Not all instructions are always perfectly translated to CIL, and still require some of the original features of KoiVM (most notably, the flags register as the CLR does not have one). For this, the code generator might inject some code to emulate the behaviour of these features. This is put into this namespace.

What's with the name and the Magikarp?
--------------------------------------
In the original release of KoiVM, the plugin description mentions a Magikarp virtualising your code. In the original Pokémon games, the best way to catch a Magikarp is using an old rod. 

...

Honestly, I don't know, I am probably weird...
