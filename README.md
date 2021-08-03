# HolonEForth

HolonEForth combines Chen-Hanson Ting's universal eforth.fs and eForthOverview.pdf.

It handles source code and documentation together in a CMS and presents it in a book view.



---

## File eforth.fs

- List of source code definitions and comments.
- Comment lines create structure.

![EForth-File](./Reference/efBilder/EForth-File.png)



## File eforthOverview.pdf

- Documentation of eForth. 

- Detailed description of the eForth words and functions.

  ![eForthOverview](./Reference/efBilder/eForthOverview.png)



---



## Book HolonEForth

- Content Management System.
- The whole project combined.
- Chapters create files if the name has a file extension.
- Chapters, sections and units have a page each. 
- Chapter and section pages collect project documentation.
- Unit pages contain source definition and separate comments.
- Automatic Hypertext links throughout the system.



### eForth Chapter eforth.fs

- No chapter text in eforth.fs.
- Only one Section - eForth creates sections in the code with comment lines 
- Chapter files collect the code units of the chapter.
- Here the units are the eforth files with the kernel and high-level words.
- The chapter file combines both.

![EForth-Chapter](./Reference/efBilder/EForth-Chapter.png)

### eForth Unit "High Level Definitions"

- A unit can contain as much code as you want, there is no defined limit.
- Usually, a Holon unit contains one source-code definition.
- The definitions are separated in holon-eforth.fs  

![EForth-Unit](./Reference/efBilder/EForth-Unit.png)



### Chapter holon-eforth.fs

- The chapter page contains the introduction to OverviewEforth.pdf
- The sections are extracted from the Overview.
- The Units of the current section.

![HolonEForth-Chapter](./Reference/efBilder/HolonEForth-Chapter.png)

### HolonEForth Section Comparison

![HolonEForth-Section](./Reference/efBilder/HolonEForth-Section2.png)



### HolonEForth Unit U<

- Unit page with source code definition and a separate pane for comments.
- The source code is written to the chapter file at the start of a session and on every change.

![HolonEForth-Unit](./Reference/efBilder/HolonEForth-Unit.png)



## File holon-eforth.fs

- Chapter file of HolonEForth.
- Pipe to the interpreter/compiler.
- Instantly updated with changes in the browser.

![HolonEForth-File](./Reference/efBilder/HolonEForth-File.png)

---



## Run HolonEForth

HolonEForth works in Windows, macOS and Linux. Use the appropriate commandfile.

#### RunWindows.bat

#### RunMacOS.command

####  RunLinux.sh

--

#### tclsh

HolonEForth runs in Tcl/Tk. If this is not present in your system:

Download and install the free Tcl/Tk via https://docs.activestate.com/activetcl/8.6/



---

## Notes

HolonEForth is a [HolonCode](https://github.com/wejgaard/HolonCode) project. 

#### holoncode/holoncode.tcl

This is the projects code, copied from the HolonCode repository.

#### HolonEForth.hdb

This is the database of the Holon CMS. It contains the complete project.

The source files are recreated at each new session. Thus don't need to be saved.




---

## Using HolonEForth

#### Hyperlinks

In the texts and definitions unit names (Forth words) are hyperlinks.

Click to see the unit, control-click to see where the unit is used.





