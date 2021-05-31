# Reversi

## Installation

1. Install Chocolatey from <https://chocolatey.org/install>
2. Install Microsoft Visual Studio Community Edition (at the time of writing: 2019 edition):

**Windows command line**

```choco install visualstudio2019community```

3. Install .NET Core 2.2 from <https://dotnet.microsoft.com/download/dotnet/2.2>. Get the .NET Core SDK 2.2.207
4. Install nvm:

**Windows command line**

```choco install nvm```

5. Install the latest Node version using nvm
6. Set the latest Node version using nvm
7. Run npm to retrieve the location of your global npm package:

**Windows command line**

```npm```

8. Go to that folder and then go to its grandparent folder

(Execute stepts 9 to 10 only when running into security problems in the global npm directory)

9. Initialise it as an npm package:

**Windows command line**

```npm init```

Press Enter on every step but the first one. Enter "global" there.

10. Create a package-lock.json:

**Windows command line**

```npm i --package-lock-only```

(Steps 11 to 12, the installation of pretty outdated tools, might be necessary because the tested node version (16.1.0) might have been shipped with an outdated version of node-gyp. Global installation of the recent version of node-gyp is not tested, but might lead to a simpler, more modern toolchain)

11. For Gulp Sass we need Python 2. Install Python 2.7.18 from <https://www.python.org/downloads/release/python-2718/>. Use a global install for all users.
12. Set the Python version used by npm:

**Windows command line**

```npm config set python C:\Python27\python.exe```

13. Using the Microsoft Visual Studio Installer, install the Visual C++ toolchain version 14.1x. Install the following individual components:

  * MSVC v141 - VS 2017 C++ x64/x86 build tools (v14.16)
  * MSVC v141 - VS 2017 C++ x64/x86 Spectre-mitigated libs (v14.16) (this might be unnecessary, will be tested later on)
  
14. Set the correct build path for npm:

**Windows command line**

```npm config set msbuild_path "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"```

15. Install Microsoft Visual Studio Code:

**Windows command line**

```choco install vscode```

16. Within Microsoft Visual Studio Code, install the recommended Markdown linter (davidanson.vscode-markdownlint)

17. If your PC allows its fairly high performance consumption, install Jetbrains ReSharper:

```choco install resharper```
