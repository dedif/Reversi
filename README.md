# Reversi

## Installation

1. Install Chocolatey from <https://chocolatey.org/install>
2. Install Microsoft Visual Studio 2019 Community Edition:

**Windows command line**

```choco install visualstudio2019community```

3. Install Microsoft Visual Studio 2019 Build Tools:

**Windows command line**

```choco install visualstudio2017buildtools```

4. Install the Microsoft Visual C++ workload in Microsoft Visual Studio 2017 Build Tools with the Microsoft Visual Studio Installer.
5. Open the solution. Visual Studio will tell you to install the required workloads. Install these workloads.
6. Install .NET Core 2.2 from <https://dotnet.microsoft.com/download/dotnet/2.2>. Get the .NET Core SDK 2.2.207
7. Install nvm:

**Windows command line**

```choco install nvm```

8. Install Node 8 using nvm (we must use Node 8 because `gulp-sass` 4 requires `node-sass` 4 (set in `package-lock.json to 4.8.3 to be precise), which in turn requires an older version of Node. We picked Node 8 to be safe.):
9. Set the latest Node version using nvm

(Execute stepts 9 to 12 only when running into security problems in the global npm directory)

10. Run npm to retrieve the location of your global npm package:

**Windows command line**

```npm```

11. Go to that folder and then go to its grandparent folder



12. Initialise it as an npm package:

**Windows command line**

```npm init```

Press Enter on every step but the first one. Enter "global" there.

13. Create a package-lock.json:

**Windows command line**

```npm i --package-lock-only```

14. For Gulp Sass we need Python 2. Install Python 2.7.18 from <https://www.python.org/downloads/release/python-2718/>. Use a global install for all users.
15. Set the Python version used by npm:

**Windows command line**

```npm config set python C:\Python27\python.exe```

16. Using the Microsoft Visual Studio Installer, install the Visual C++ toolchain version 14.1x. Install the following individual components:

  * MSVC v141 - VS 2017 C++ x64/x86 build tools (v14.16)
  * MSVC v141 - VS 2017 C++ x64/x86 Spectre-mitigated libs (v14.16) (this might be unnecessary, will be tested later on)

17. Install Microsoft Visual Studio Code:

**Windows command line**

```choco install vscode```

18. Within Microsoft Visual Studio Code, install the recommended Markdown linter (davidanson.vscode-markdownlint)

19. If your PC allows its fairly high performance requirements, install Jetbrains ReSharper:

```choco install resharper```

20. In the project, open a shell and go to the `API` directory.
21. Install the packages there:

**Windows command line**

```npm i```

22. Go to the `wwwroot` directory and install the packages there:

**Windows command line**

```npm i```

23. Go back to the `API`directory
24. Build the project with Gulp:

**Windows command line**

```npx gulp```

25. Serve the project:

```npx gulp serve```