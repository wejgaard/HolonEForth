# HolonEForth 

HolonEForth combines Chen-Hanson Ting' s universal eforth.fs and eForthOverview.pdf in a CMS, 

and presents and handles source code and documentation together in a book view.

Chapters create files, sections collect related program units, units have separate spaces for comments.

HolonEForth is a [HolonCode](https://github.com/wejgaard/HolonCode) project. 



## File eforth.fs

- List of source code definitions and comments
- Comment lines create structure

![EForth-File](./Reference/efBilder/EForth-File.png)



## File eforthOverview.pdf

- Documentation of eForth 
- Detailed description of the eForth words and functions.

![eForthOverview](/Users/wolfwejgaard/Desktop/HolonEForth BIlder/eForthOverview.png)



---



## Book HolonEForth

The book contains eforth.fs and eforthOverview.pdf together tightly connected

- Content Management System
- The whole project combined.
- Chapters create files.
- Chapter and section pages collect project documentation
- Units contains source definition and separate comments 
- Automatic Hypertext links through complete system



### Chapter eforth.fs

- Chapter, Sections and Units have a Forth screen as page. 
- No chapter text in eforth.fs.
- Only one Section - eForth creates sections in the code with comment lines 
- The units code fields contain the respective eForth files
- The chapter file contains the code of the contained units 
  - thus the  x86 Code Words and High Level Definitions

![EForth-Chapter](./Reference/efBilder/EForth-Chapter.png)

### Chapter holon-eforth.fs

- The page contains the start of OverviewEforth.pdf
- Sections are extracted from the document
- Units of the current section 

![HolonEForth-Chapter](./Reference/efBilder/HolonEForth-Chapter.png)

### Section Comparison

- Shows the section page in eForthOverview 
- And the eforth.fs units of the section

![HolonEForth-Section](./Reference/efBilder/HolonEForth-Section.png)

### Unit U<

- Units page with source code definition and a separate pane for comments.
- The source code is written to the chapter file at the start of a session and on every change.

![HolonEForth-Unit](./Reference/efBilder/HolonEForth-Unit.png)



## File holon-eforth.fs

- Chapterfile of HolonEForth.
- Pipe to the interpreter/compiler.
- Instantly updated with changes in the browser.

![HolonEForth-File](./Reference/efBilder/HolonEForth-File.png)



## Run HolonEForth

#### Windows

```
tclsh .\src\holoncode.tcl HolonEForth.hdb
````

#### macOS and Linux

````
#!/bin/bash
cd `dirname $0` 
tclsh ./src/holoncode.tcl HolonEForth.hdb &
````

---

#### Notes

##### tclsh

The Tcl/Tk shell, used to be included in operating systems, if not available on your's: 

Download and install the free Tcl/Tk via https://docs.activestate.com/activetcl/8.6/

##### src/holoncode.tcl

Project source code copied from the HolonCode repo.

##### HolonEForth.hdb

The database of the CMS. Contains the complete project, recreates the source files at a new session.
