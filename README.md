# HolonEForth 

HolonEForth combines Chen-Hanson Ting' s universal eforth.fs and eForthOverview.pdf in a CMS, 

and presents and handles source code and documentation together in a book view.

Chapters are related to files, Sections structure the program units into logical parts, 

Unit pages contain separate spaces for definitions and comments.

HolonEForth is a [HolonCode](https://github.com/wejgaard/HolonCode) project. 



## Textfile eforth.fs
- List of source code definitions and comments
- Comment lines create structure

![EForth-File](./Reference/efBilder/EForth-File.png)


---
## Browser HolonEForth

#### Content Management System

#### Book with Chapters, Sections and Pages

- Chapters create files with the code of the contained units. 
- Each chapter and section offers a page for project documentation 
- Unit pages are divided into separate panes for code and comments 
- The title pane contains the name of the definition 
- The title name is a target for hypertext links 
- Database is Tcl Metakit.  



## Chapter eforth.fs

- Chapter, Sections and Units have a Forth screen as page. 
- No chapter text in eforth.fs.
- Only one Section - eForth creates sections in the code with comment lines 
- The units code fields contain the respective eForth files
- The chapter file contains the code of the contained units 
  - thus the  x86 Code Words and High Level Definitions

![EForth-Chapter](./Reference/efBilder/EForth-Chapter.png)

## Chapter HolonEForth.fs

- The page contains the first part of OverviewEforth.pdf
- Sections are extracted from the document
- Units of the current section 

![HolonEForth-Chapter](./Reference/efBilder/HolonEForth-Chapter.png)

## Section

- Section page in eForth Overview 
- Units of section

![HolonEForth-Section](./Reference/efBilder/HolonEForth-Section.png)

## Unit

- Units pages with source code definition and a separate pane for comments.
- The source code is written to the chapter file at the start of a session and on changes.

![HolonEForth-Unit](./Reference/efBilder/HolonEForth-Unit.png)

## File HolonEForth.fs

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

